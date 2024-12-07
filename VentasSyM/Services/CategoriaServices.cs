using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VentasSyM.Data;
using VentasSyM.Models;

namespace VentasSyM.Services
{
    public class CategoriaServices(IDbContextFactory<ApplicationDbContext> DbFactory)
    {
        private readonly ApplicationDbContext _context;
        public async Task<bool> Existe(int CategoriaId)
        {
            await using var _context = await DbFactory.CreateDbContextAsync();
            return await _context.Categorias.AnyAsync(c => c.CategoriaId == CategoriaId);
        }

        public async Task<Categorias?> ObtenerPorId(int categoriaId)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return await context.Categorias.FindAsync(categoriaId);
        }

        public async Task<bool> Insertar(Categorias categorias)
        {
            await using var _context = await DbFactory.CreateDbContextAsync();

            _context.Categorias.Add(categorias);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<bool> Modificar(Categorias categorias)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();

            contexto.Update(categorias);
            return await contexto.SaveChangesAsync() > 0;
        }


        public async Task<bool> Guardar(Categorias categorias)
        {
            if (!await Existe(categorias.CategoriaId))
            {
                return await Insertar(categorias);
            }
            else
                return await Modificar(categorias);
        }

        public async Task<bool> Eliminar(int categoriaId)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var categorias = await context.Categorias.FindAsync(categoriaId);

            if (categorias == null)
            {
                return false;
            }

            context.Categorias.Remove(categorias);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Categorias>? Buscar(int CategoriaId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Categorias.AsNoTracking().FirstOrDefaultAsync(c => c.CategoriaId == CategoriaId);
        }

        public async Task<List<Categorias>> Listar(Expression<Func<Categorias, bool>> criterio)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Categorias
                .Where(criterio)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
