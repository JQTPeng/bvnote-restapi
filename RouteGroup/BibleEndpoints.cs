using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using bvnote_restapi.Data;
namespace bvnote_restapi.RouteGroup;
public static class BibleEndpoints
{
    public static RouteGroupBuilder MapBible(this RouteGroupBuilder group)
    {
        group.MapGet("/books", [OutputCache] (IBibleDb db) => {
            var results = db.GetBooks();
            return results.Count > 0 ? Results.Ok(results) : Results.NotFound();
        });

        group.MapGet("/books/{bookId}/verses", [OutputCache] (int bookId, [FromQuery] int chapterNo, IBibleDb db) => {
            var results = db.GetVerses(bookId, chapterNo);
            return results.Count > 0 ? Results.Ok(results) : Results.NotFound();
        });

        return group;
    }

    public static RouteGroupBuilder MapDocument(this RouteGroupBuilder group)
    {
        group.MapGet("/documents", [OutputCache](IBibleDb db) =>
        {
            // TODO: page-based pagination
        });

        group.MapGet("/documents/{id}", (int id) =>
        {
            
        });
        return group;
    }
}