using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VentasSyM.Data;
using VentasSyM.Models;

namespace VentasSyM.Services;

public class ProductoServices (IDbContextFactory<ApplicationDbContext> DbFactory)
{
    private readonly ApplicationDbContext _context;
    public async Task<bool> Existe(int ProductoId)
    {
        await using var _context = await DbFactory.CreateDbContextAsync();
        return await _context.Productos.AnyAsync(c => c.ProductoId == ProductoId);
    }

    public async Task<Productos?> ObtenerPorId(int productoId)
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        return await context.Productos.FindAsync(productoId);
    }

    public async Task<bool> Insertar(Productos productos)
    {
        await using var _context = await DbFactory.CreateDbContextAsync();

        _context.Productos.Add(productos);
        return await _context.SaveChangesAsync() > 0;
    }


    public async Task<bool> Modificar(Productos productos)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        contexto.Update(productos);
        return await contexto.SaveChangesAsync() > 0;
    }


    public async Task<bool> Guardar(Productos productos)
    {
        if (!await Existe(productos.ProductoId))
        {
            return await Insertar(productos);
        }
        else
            return await Modificar(productos);
    }

    public async Task<bool> Eliminar(int productoId)
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        var productos = await context.Productos.FindAsync(productoId);

        if (productos == null)
        {
            return false; 
        }

        context.Productos.Remove(productos);
        await context.SaveChangesAsync();
        return true; 
    }

    public async Task<List<Productos>> Listar(Expression<Func<Productos, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Productos
            .Include(p => p.Categoria) 
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Productos>? Buscar(int ProductoId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Productos.AsNoTracking().FirstOrDefaultAsync(p => p.ProductoId == ProductoId);
    }

}

