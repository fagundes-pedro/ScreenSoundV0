using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.API.Endpoints;

public static class MusicasExtensions
{
    public static void AddEndPointMusicas(this WebApplication app)
    {
        #region Endpoint Musicas
        app.MapGet("/Musicas", ([FromServices] DAL<Musica> dal) =>
        {
            var listaMusicas = EntityListToResponseList(dal.Listar());
            return Results.Ok(listaMusicas);
        });

        app.MapGet("/Musicas/{nome}", ([FromServices] DAL<Musica> dal, string nome) =>
        {
            var listaMusicas = EntityListToResponseList(dal.Listar());
            var musica = listaMusicas.Where(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
            if (musica is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(musica);
        });
        app.MapPost("/Musicas", ([FromServices] DAL<Musica> dalMusica, [FromBody] MusicaRequest musicaRequest) =>
        {
            //var context = new ScreenSoundContext();
            //var dalArtista = new DAL<Artista>(context);
            var musica = new Musica(musicaRequest.Nome);
            //musica.Artista = dalArtista.RecuperarPor(a => a.Id.Equals(musicaRequest.ArtistaID));
            musica.AnoLancamento = musicaRequest.AnoLancamento;
            dalMusica.Adicionar(musica);
            return Results.Created();
        });

        app.MapDelete("/Musicas/{id}", ([FromServices] DAL<Musica> dal, int id) =>
        {
            var musica = dal.RecuperarPor(a => a.Id.Equals(id));
            if (musica is null)
            {
                return Results.NotFound();
            }
            dal.Deletar(musica);
            return Results.NoContent();
        });

        app.MapPut("/Musicas", ([FromServices] DAL<Musica> dal, [FromBody] MusicaRequestEdit musica) =>
        {
            var musicaAAlterar = dal.RecuperarPor(a => a.Id.Equals(musica.Id));
            if (musicaAAlterar is null)
            {
                return Results.NotFound();
            }
            musicaAAlterar.Nome = musica.Nome;
            musicaAAlterar.AnoLancamento = musica.AnoLancamento;

            dal.Atualizar(musicaAAlterar);
            return Results.Ok();
        });
        #endregion
    }
    private static ICollection<MusicaResponse> EntityListToResponseList(IEnumerable<Musica> musicaList)
    {
        return musicaList.Select(a => EntityToResponse(a)).ToList();
    }

    private static MusicaResponse EntityToResponse(Musica musica)
    {
        return new MusicaResponse(musica.Id, musica.Nome!, musica.Artista!.Id, musica.Artista.Nome);
    }
}