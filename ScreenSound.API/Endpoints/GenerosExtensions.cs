using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using ScreenSound.Shared.Modelos.Modelos;

namespace ScreenSound.API.Endpoints;

public static class GenerosExtensions
{
    public static void AddEndPointGeneros (this WebApplication app)
    {
        #region Endpoint Genero
        app.MapGet("/Generos", async ([FromServices] DAL<Genero> dal) => {
            var listaGeneros = await dal.ListarAsync();
            return Results.Ok(EntityListToResponseList(listaGeneros));
        });
        app.MapGet("/Generos/{nome}", async ([FromServices] DAL<Genero> dal, string nome) => {
            var listaGeneros = await dal.ListarAsync();
            var genero = listaGeneros.FirstOrDefault(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
            if (genero is null)
            {
                return Results.NotFound("Gênero não encontrado.");
            }
            return Results.Ok(EntityToResponse(genero));
        });
        app.MapPost("/Generos", async ([FromServices] DAL<Genero> dal, [FromBody] GeneroRequest generoRequest) =>
        {
            if (string.IsNullOrWhiteSpace(generoRequest.Nome))
            {
                return Results.BadRequest("Nome do gênero é obrigatório");
            }
            var genero = RequestToEntity(generoRequest);
            await dal.AdicionarAsync(genero);
            return Results.Created($"/Generos/{genero.Id}", EntityToResponse(genero));
        });
        app.MapDelete("/Generos", async ([FromServices] DAL<Genero> dal, int id) =>
        {
            var genero = await dal.RecuperarPorAsync(g => g.Id.Equals(id));
            if (genero is null)
            {
                return Results.NotFound("Gênero não encontrado.");
            }
            await dal.DeletarAsync(genero);
            return Results.NoContent();
        });
        #endregion
    }
    private static Genero RequestToEntity(GeneroRequest generoRequest)
    {
        return new Genero(generoRequest.Nome) { Descricao = generoRequest.Descricao };
    }
    private static ICollection<GeneroResponse> EntityListToResponseList(IEnumerable<Genero> listaDeGeneros)
    {
        return listaDeGeneros.Select(g => EntityToResponse(g)).ToList();
    }
    private static GeneroResponse EntityToResponse(Genero genero)
    {
        return new GeneroResponse(genero.Id, genero.Nome!, genero.Descricao);
    }
}
