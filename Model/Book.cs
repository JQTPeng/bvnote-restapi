namespace bvnote_restapi.Model;
public class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public bool OldTestament { get; set; }
    public ICollection<Verse> Verses { get; set; } = new List<Verse>();
}
