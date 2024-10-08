﻿using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using ScreenSound.Shared.Modelos.Modelos;

namespace ScreenSound.API.Endpoints;

public static class MusicasExtensions
{
    public static void AddEndPointMusicas(this WebApplication app)
    {
        #region Endpoint Musicas
        app.MapGet("/Musicas", async ([FromServices] DAL<Musica> dal) =>
        {
            var listaMusicas = await dal.ListarAsync();
            return Results.Ok(EntityListToResponseList(listaMusicas));
        });

        app.MapGet("/Musicas/{nome}", async ([FromServices] DAL<Musica> dal, string nome) =>
        {
            var listaMusicas = await dal.ListarAsync();
            var musica = listaMusicas.FirstOrDefault(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
            if (musica is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(EntityToResponse(musica));
        });
        app.MapPost("/Musicas", async ([FromServices] DAL<Musica> dalMusica, [FromServices] DAL<Genero> dalGenero, [FromBody] MusicaRequest musicaRequest) =>
        {
            var musica = new Musica(musicaRequest.Nome)
            {
                ArtistaId = musicaRequest.ArtistaID,
                AnoLancamento = musicaRequest.AnoLancamento,
                Generos = musicaRequest.Generos is not null ? GeneroRequestConverter(musicaRequest.Generos, dalGenero) : new List<Genero>()
            };
            await dalMusica.AdicionarAsync(musica);
            return Results.Created();
        });

        app.MapDelete("/Musicas/{id}", async ([FromServices] DAL<Musica> dal, int id) =>
        {
            var musica = await dal.RecuperarPorAsync(a => a.Id.Equals(id));
            if (musica is null)
            {
                return Results.NotFound("Musica não encontrada");
            }
            await dal.DeletarAsync(musica);
            return Results.NoContent();
        });

        app.MapPut("/Musicas", async ([FromServices] DAL<Musica> dal, [FromBody] MusicaRequestEdit musica) =>
        {
            var musicaAAlterar = await dal.RecuperarPorAsync(a => a.Id.Equals(musica.Id));
            if (musicaAAlterar is null)
            {
                return Results.NotFound("Musica não encontrada");
            }
            musicaAAlterar.Nome = musica.Nome;
            musicaAAlterar.AnoLancamento = musica.AnoLancamento;
            musicaAAlterar.ArtistaId = musica.ArtistaId;

            await dal.AtualizarAsync(musicaAAlterar);
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
    private static ICollection<Genero> GeneroRequestConverter(ICollection<GeneroRequest> generos, DAL<Genero> dalGenero)
    {
        var listaDeGeneros = new List<Genero>();
        foreach (var item in generos)
        {
            var entity = RequestToEntity(item);
            var genero = dalGenero.RecuperarPor(g => g.Nome.ToUpper().Equals(item.Nome.ToUpper()));
            if (genero is not null)
            {
                listaDeGeneros.Add(genero);
            }
            else
            {
                listaDeGeneros.Add(entity);
            }
        }
        return listaDeGeneros;
    }
    private static Genero RequestToEntity(GeneroRequest genero)
    {
        return new Genero(genero.Nome) {Descricao = genero.Descricao};
    }
}