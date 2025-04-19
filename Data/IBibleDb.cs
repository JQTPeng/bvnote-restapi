using bvnote_restapi.Model;
namespace bvnote_restapi.Data;

public interface IBibleDb
{
    public void CheckConection();
    public List<Book> GetBooks();
    public List<Verse> GetVerses(int bookId, int chapterNo);
}