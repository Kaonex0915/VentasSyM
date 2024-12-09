using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VentasSyM.Data;
using VentasSyM.Models;

namespace VentasSyM.Services;

public class DevolucionesServices(IDbContextFactory<ApplicationDbContext> DbFactory)
{

    private readonly ApplicationDbContext _context;

    public async Task<bool> Existe(int DevolucionId)
    {
        await using var _context = await DbFactory.CreateDbContextAsync();
        return await _context.Devoluciones.AnyAsync(d => d.DevolucionId == DevolucionId);
    }

    public async Task<bool> Insertar(Devoluciones devoluciones)
    {
        await using var _context = await DbFactory.CreateDbContextAsync();

        foreach (var devolucion in devoluciones.DevolucionDetalle)
        {
            var producto = await BuscarProductos(devolucion.ProductoId);

            if (producto != null)
            {
                producto.Existencia += devolucion.UnidadDevuelta;
                _context.Update(producto);
            }
            else
            {
                return false;
            }
        }

        _context.Devoluciones.Add(devoluciones);
        return await _context.SaveChangesAsync() > 0;
    }


    public async Task<bool> Modificar(Devoluciones devoluciones)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        contexto.Update(devoluciones);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Devoluciones devoluciones)
    {
        if (!await Existe(devoluciones.DevolucionId))
        {
            return await Insertar(devoluciones);
        }
        else
        {
            return await Modificar(devoluciones);
        }
    }

    public async Task<bool> Eliminar(int DevolucionId)
    {
        await using var _context = await DbFactory.CreateDbContextAsync();
        var devolucion = await _context.Devoluciones
            .Include(d => d.DevolucionDetalle)
            .FirstOrDefaultAsync(d => d.DevolucionId == DevolucionId);

        if (devolucion == null) return false;

        foreach (var detalle in devolucion.DevolucionDetalle)
        {
            var producto = await _context.Productos.FindAsync(detalle.ProductoId);
            if (producto != null)
            {
                producto.Existencia -= detalle.UnidadDevuelta;
                _context.Update(producto);
            }
        }

        _context.DevolucionDetalles.RemoveRange(devolucion.DevolucionDetalle);
        _context.Devoluciones.Remove(devolucion);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<Productos> BuscarProductos(int ProductoId)
    {
        await using var _context = await DbFactory.CreateDbContextAsync();
        return await _context.Productos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductoId == ProductoId);
    }

    public async Task<List<Devoluciones>> Listar(Expression<Func<Devoluciones, bool>> criterio)
    {
        await using var _context = await DbFactory.CreateDbContextAsync();
        return await _context.Devoluciones
            .Include(d => d.DevolucionDetalle)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<List<Productos>> GetProductos()
    {
        await using var _context = await DbFactory.CreateDbContextAsync();
        return await _context.Productos
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<Devoluciones> Buscar(int DevolucionId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Devoluciones
            .Include(d => d.DevolucionDetalle)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.DevolucionId == DevolucionId);
    }
}