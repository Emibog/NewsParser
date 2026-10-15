using Telegram.Bot;

public class TelegramNotifier
{
    private readonly ITelegramBotClient _botClient;
    private readonly long _chatId;

    public TelegramNotifier(string botToken, long chatId)
    {
        _botClient = new TelegramBotClient(botToken);
        _chatId = chatId;
    }

    public async Task SendMessage(NewsItem news, string theme)
    {
        string message = $"Новость{theme}\n\n<b>{news.Title}</b>\n\n{news.Link}";

        try
        {
            await _botClient.SendMessage(
                chatId: _chatId,
                text: message,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Html
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка отправки в Telegram: {ex.Message}");
        }
    }
}