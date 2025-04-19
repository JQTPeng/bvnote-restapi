using Microsoft.Data.Sqlite;
using bvnote_restapi.Model;
namespace bvnote_restapi.Data;

// TODO - Enable SQlite Write-Ahead-of-Time
public class BibleDb(string? connString, ILogger<BibleDb> logger) : IBibleDb
{
    public void CheckConection()
    {
        using SqliteConnection conn = new SqliteConnection(connString);
        conn.Open();
        logger.LogInformation("Connection to Bible DB: {s}", conn.State);
        conn.Close();
    }

    public List<Book> GetBooks()
    {
        List<Book> results = new List<Book>(66);
        using var conn = new SqliteConnection(connString);
        conn.Open();
        var command = conn.CreateCommand();
        command.CommandText = "SELECT * FROM books";
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                results.Add(new Book()
                {
                    Id = reader.GetInt16(0),
                    Title = reader.GetString(1),
                    OldTestament = reader.GetBoolean(2)
                });
            }
        }
        conn.Close();
        return results;
    }

    public List<Verse> GetVerses(int bookId, int chapterNo)
    {
        List<Verse> results = new();
        using var conn = new SqliteConnection(connString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText =
        @"
            SELECT * FROM verses_bsb 
            WHERE book_id = $bookId AND chapter = $chapterNo;
        ";
        cmd.Parameters.AddWithValue("$bookId", bookId);
        cmd.Parameters.AddWithValue("$chapterNo", chapterNo);
        
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            results.Add(new Verse()
            {
                Id = reader.GetInt16(0),
                Chapter = reader.GetInt16(1),
                VerseNumber = reader.GetInt16(2),
                Text = reader.GetString(3),
                BookId = reader.GetInt16(4)
            });
        }

        conn.Close();
        return results;
    }
}