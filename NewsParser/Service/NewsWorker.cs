using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

public class NewsWorker : BackgroundService
{
    private readonly IConfiguration _config;
    private readonly ILogger<NewsWorker> _logger;
    private readonly NewsParser _parser;
    private readonly NewsRepository _repository;
    private readonly NewsFilter _filter;
    private readonly TelegramNotifier _notifier;

    public NewsWorker(ILogger<NewsWorker> logger, IConfiguration config)
    {
        _config = config;
        _logger = logger;
        _parser = new NewsParser();
        _repository = new NewsRepository();
        _filter = new NewsFilter(_config);
        _notifier = new TelegramNotifier(config["TelegramSettings:BotToken"]!, long.Parse(config["TelegramSettings:ChatId"]!));


    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int delay = int.Parse(_config["NewsSettings:CheckIntervalMinutes"] ?? "5");
        List<string> feedUrls = _config.GetSection("NewsSettings:FeedUrls").Get<List<string>>() ?? throw new InvalidOperationException("Не задан список FeedUrls в appsettings.json");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Начинаю проверку новостей...");
            
            foreach(string feedUrl in feedUrls)
            {
                List<NewsItem> newsItems = await _parser.ParseRssFeedAsync(feedUrl);

                foreach(NewsItem news in newsItems)
                {
                    var hash = news.GetHash();

                    if(!_repository.HasBeenSeen(hash))
                    {
                        if(_filter.IsRelevant(news))
                        {
                            string? keyword = _filter.GetMatchedKeyword(news);
                            string? theme = string.Empty;

                            if(keyword != null)
                            {
                                theme = $" по теме '{keyword}'";
                            }
                            
                            _logger.LogInformation($"Найдена новая новость{theme}: {news.Title}  {news.Link}");
                            await _notifier.SendMessage(news, theme);
                        }

                        _repository.MarkAsSeen(hash);
                    }
                }
            }    
        }

        await Task.Delay(TimeSpan.FromSeconds(delay), stoppingToken);
    }
}