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
        app.MapGet("/Artistas", ([FromServices] DAL<Artista> dal) =>
        {
            var listaArtistas = EntityListToResponseList(dal.Listar());
            return Results.Ok(listaArtistas);
        });

        app.MapGet("/Artistas/{nome}", ([FromServices] DAL<Artista> dal, string nome) =>
        {
            var listaArtistas = EntityListToResponseList(dal.Listar());
            var artista = listaArtistas.Where(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
            if (artista is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(artista);
        });

        app.MapPost("/Artistas", ([FromServices] DAL<Artista> dal, [FromBody] ArtistaRequest artistaRequest) =>
        {
            var artista = new Artista(artistaRequest.Nome, artistaRequest.Bio);
            dal.Adicionar(artista);
            return Results.Created();
        });

        app.MapDelete("/Artistas/{id}", ([FromServices] DAL<Artista> dal, int id) =>
        {
            var artista = dal.RecuperarPor(a => a.Id.Equals(id));
            if (artista is null)
            {
                return Results.NotFound();
            }
            dal.Deletar(artista);
            return Results.NoContent();
        });

        app.MapPut("/Artistas", ([FromServices] DAL<Artista> dal, [FromBody] ArtistaRequestEdit artista) =>
        {
            var artistaAAlterar = dal.RecuperarPor(a => a.Id.Equals(artista.Id));
            if (artistaAAlterar is null)
            {
                return Results.NotFound();
            }
            artistaAAlterar.Nome = artista.Nome;
            artistaAAlterar.Bio = artista.Bio;
            artistaAAlterar.FotoPerfil = artista.FotoPerfil;
            dal.Atualizar(artistaAAlterar);
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
