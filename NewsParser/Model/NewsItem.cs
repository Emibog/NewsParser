using System.Security.Cryptography;
using System.Text;

public class NewsItem
{
    public string Title {get; set;} = string.Empty;
    public string Link {get; set;} = string.Empty;
    public DateTimeOffset PublishDate {get; set;}

    public string GetHash()
    {
        string input = $"{Title}||{Link}";
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = SHA256.HashData(bytes);

        return Convert.ToHexString(hashBytes);
    }

}