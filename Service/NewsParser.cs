using System.ServiceModel.Syndication;
using System.Xml;

public class NewsParser
{
    public async Task<List<NewsItem>> ParseRssFeedAsync(string url)
    {
        List<NewsItem> newsItems = new List<NewsItem>();
        using HttpClient httpClient = new HttpClient();

         // Устанавливаем User-Agent
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; MyNewsBot/1.0)");

        try
        {
            string response = await httpClient.GetStringAsync(url);
            using StringReader stringReader = new StringReader(response);
            using XmlReader xmlReader = XmlReader.Create(stringReader);

            SyndicationFeed feed = SyndicationFeed.Load(xmlReader);

            foreach(SyndicationItem item in feed.Items)
            {
                newsItems.Add(new NewsItem
                {
                    Title = item.Title.Text,
                    Link = item.Links.FirstOrDefault()?.Uri.ToString() ?? "",
                    PublishDate = item.PublishDate
                });
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Ошибка при парсинге {url}: {ex.Message}");
        }

        return newsItems;
    }
}