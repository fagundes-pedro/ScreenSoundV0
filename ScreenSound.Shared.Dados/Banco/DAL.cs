using Microsoft.EntityFrameworkCore;
using ScreenSound.Modelos;
using ScreenSound.Shared.Modelos.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSound.Banco;
public class DAL<T> where T : class
{
    private readonly ScreenSoundContext context;

    public DAL(ScreenSoundContext context)
    {
        this.context = context;
    }

    public IEnumerable<T> Listar()
    {
        return context.Set<T>().ToList();
    }
    public void Adicionar(T objeto)
    {
        context.Set<T>().Add(objeto);
        context.SaveChanges();
    }
    public void Atualizar(T objeto)
    {
        context.Set<T>().Update(objeto);
        context.SaveChanges();
    }
    public void Deletar(T objeto)
    {
        context.Set<T>().Remove(objeto);
        context.SaveChanges();
    }

    public T? RecuperarPor(Func<T, bool> condicao)
    {
        return context.Set<T>().FirstOrDefault(condicao);
    }
    public async Task AdicionarAsync(T objeto)
    {
        await context.Set<T>().AddAsync(objeto);
        await context.SaveChangesAsync();
    }
    public async Task DeletarAsync(T objeto)
    {
        context.Set<T>().Remove(objeto);
        await context.SaveChangesAsync();
    }
    public async Task<IEnumerable<T>> ListarAsync()
    {
        var lista = await context.Set<T>().ToListAsync();
        return lista;
    }
    public async Task<T?> RecuperarPorAsync(Expression<Func<T, bool>> condicao)
    {
        return await context.Set<T>().FirstOrDefaultAsync(condicao);
    }
    public async Task AtualizarAsync(T objeto)
    {
        context.Set<T>().Update(objeto);
        await context.SaveChangesAsync();
    }
}
