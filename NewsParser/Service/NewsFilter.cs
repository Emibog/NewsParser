using Microsoft.Extensions.Configuration;

public class NewsFilter
{
    private readonly List<string> _keywords;
    private readonly List<string> _stopwords;
    private readonly StringComparison _comparison = StringComparison.OrdinalIgnoreCase;
    public NewsFilter(IConfiguration config)
    {
        _keywords = config.GetSection("NewsSettings:Keywords").Get<List<string>>() ?? new List<string>();
        _stopwords = config.GetSection("NewsSettings:StopWords").Get<List<string>>() ?? new List<string>();
    }

    public bool IsRelevant(NewsItem news)
    {
        if(IsStopWord(news))
            return false;
            
        if(_keywords.Count < 1)
            return true;

        if(IsContainsKeyword(news))
            return true;
        
        
        return false;
    }

    private bool IsContainsKeyword(NewsItem news)
    {
        string text = $"{news.Title} + {news.Link}";

        return _keywords.Any(kw =>
            !string.IsNullOrWhiteSpace(kw) &&
            text.Contains(kw, _comparison));

    }

    private bool IsStopWord(NewsItem news)
    {
        string text = $"{news.Title} + {news.Link}";

        return _stopwords.Any(sw => 
            !string.IsNullOrWhiteSpace(sw) &&
            text.Contains(sw, _comparison));
    }
}