namespace bvnote_restapi.Model;
public class Verse
{
    public int Id { get; set; }
    public int Chapter { get; set; }
    public int VerseNumber { get; set; }
    public string Text { get; set; } = null!;
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
}
