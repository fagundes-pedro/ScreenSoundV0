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
        app.MapGet("/Generos", ([FromServices] DAL<Genero> dal) => {
            var listaGeneros = EntityListToResponseList(dal.Listar());
            return Results.Ok(listaGeneros);
        });
        app.MapGet("/Generos/{nome}", ([FromServices] DAL<Genero> dal, string nome) => {
            var listaGeneros = EntityListToResponseList(dal.Listar());
            var genero = listaGeneros.Where(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
            if (genero is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(genero);
        });
        app.MapPost("/Generos", ([FromServices] DAL<Genero> dal, [FromBody] GeneroRequest generoRequest) =>
        {
            var genero = new Genero(generoRequest.Nome)
            {
                Descricao = generoRequest.Descricao
            };
            dal.Adicionar(genero);
            return Results.Created();
        });
        app.MapDelete("/Generos", ([FromServices] DAL<Genero> dal, int id) =>
        {
            var genero = dal.RecuperarPor(g => g.Id.Equals(id));
            if (genero is null)
            {
                return Results.NotFound();
            }
            dal.Deletar(genero);
            return Results.NoContent();
        });
        #endregion
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
