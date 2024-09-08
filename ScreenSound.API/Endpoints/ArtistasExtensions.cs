using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.API.Endpoints;

public static class ArtistasExtensions
{
    public static void AddEndPointArtistas (this WebApplication app)
    {
        #region Endpoint Artistas
        app.MapGet("/Artistas", async ([FromServices] DAL<Artista> dal) =>
        {
            var listaArtistas = await dal.ListarAsync();
            return Results.Ok(EntityListToResponseList(listaArtistas));
        });

        app.MapGet("/Artistas/{nome}", async ([FromServices] DAL<Artista> dal, string nome) =>
        {
            var listaArtistas = await dal.ListarAsync();
            var artista = listaArtistas.FirstOrDefault(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
            if (artista is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(EntityToResponse(artista));
        });

        app.MapPost("/Artistas", async ([FromServices] DAL<Artista> dal, [FromBody] ArtistaRequest artistaRequest) =>
        {
            var artista = new Artista(artistaRequest.Nome, artistaRequest.Bio);
            await dal.AdicionarAsync(artista);
            return Results.Created();
        });

        app.MapDelete("/Artistas/{id}", async ([FromServices] DAL<Artista> dal, int id) =>
        {
            var artista = await dal.RecuperarPorAsync(a => a.Id.Equals(id));
            if (artista is null)
            {
                return Results.NotFound();
            }
            await dal.DeletarAsync(artista);
            return Results.NoContent();
        });

        app.MapPut("/Artistas", async ([FromServices] DAL<Artista> dal, [FromBody] ArtistaRequestEdit artista) =>
        {
            var artistaAAlterar = await dal.RecuperarPorAsync(a => a.Id.Equals(artista.Id));
            if (artistaAAlterar is null)
            {
                return Results.NotFound();
            }
            artistaAAlterar.Nome = artista.Nome;
            artistaAAlterar.Bio = artista.Bio;
            artistaAAlterar.FotoPerfil = artista.FotoPerfil;
            await dal.AtualizarAsync(artistaAAlterar);
            return Results.Ok();
        });
        #endregion
    }
    private static ICollection<ArtistaResponse> EntityListToResponseList(IEnumerable<Artista> listaDeArtistas)
    {
        return listaDeArtistas.Select(a => EntityToResponse(a)).ToList();
    }

    private static ArtistaResponse EntityToResponse(Artista artista)
    {
        return new ArtistaResponse(artista.Id, artista.Nome, artista.Bio, artista.FotoPerfil);
    }
}
