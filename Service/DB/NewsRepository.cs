using Microsoft.Data.Sqlite;

public class NewsRepository
{
    private readonly string _connectionString = "Data Source=news.db";

    public NewsRepository()
    {
        using SqliteConnection connection = new SqliteConnection(_connectionString);
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = 
            @"CREATE TABLE IF NOT EXISTS SeenNews (
                Hash TEXT PRIMARY KEY
            );";
        command.ExecuteNonQuery();
    }

    // Проверяем, видели ли мы уже эту новость
    public bool HasBeenSeen(string hash)
    {
        using SqliteConnection connection = new SqliteConnection(_connectionString);
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM SeenNews WHERE Hash = $hash";
        command.Parameters.AddWithValue("$hash", hash);
        int count = Convert.ToInt32(command.ExecuteScalar());
        return count > 0;
    }

    // Помечаем новость как увиденную
    public void MarkAsSeen(string hash)
    {
        using SqliteConnection connection = new SqliteConnection(_connectionString);
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = "INSERT OR IGNORE INTO SeenNews (Hash) VALUES ($hash)";
        command.Parameters.AddWithValue("$hash", hash);
        command.ExecuteNonQuery();
    }
}