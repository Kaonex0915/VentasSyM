using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VentasSyM.Data;
using VentasSyM.Models;

namespace VentasSyM.Services;

public class OfertaServices(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    private readonly ApplicationDbContext _context;
    public async Task<bool> Existe(int OfertaId)
    {
        await using var _context = await DbFactory.CreateDbContextAsync();
        return await _context.Ofertas.AnyAsync(o => o.OfertaId == OfertaId);
    }

    public async Task<Ofertas?> ObtenerPorId(int ofertaId)
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        return await context.Ofertas.FindAsync(ofertaId);
    }

    public async Task<bool> Insertar(Ofertas ofertas)
    {
        ofertas.FechaFin = DateTime.Today.AddDays(1).AddHours(23).AddMinutes(59).AddSeconds(59);
        await using var _context = await DbFactory.CreateDbContextAsync();

        _context.Ofertas.Add(ofertas);
        return await _context.SaveChangesAsync() > 0;
    }


    public async Task<bool> Modificar(Ofertas ofertas)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        contexto.Update(ofertas);
        return await contexto.SaveChangesAsync() > 0;
    }


    public async Task<bool> Guardar(Ofertas ofertas)
    {
        if (!await Existe(ofertas.OfertaId))
        {
            return await Insertar(ofertas);
        }
        else
            return await Modificar(ofertas);
    }

    public async Task<bool> Eliminar(int ofertaId)
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        var ofertas = await context.Ofertas.FindAsync(ofertaId);

        if (ofertas == null)
        {
            return false;
        }

        context.Ofertas.Remove(ofertas);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Ofertas>? Buscar(int OfertaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Ofertas.AsNoTracking().FirstOrDefaultAsync(o => o.OfertaId == OfertaId);
    }
    public void EliminarOfertasExpiradas()
    {
        using var contexto = DbFactory.CreateDbContext();
        var ahora = DateTime.Now;
        var ofertasExpiradas = contexto.Ofertas.Where(o => o.FechaFin <= ahora).ToList();
        if (ofertasExpiradas.Any())
        {
            contexto.Ofertas.RemoveRange(ofertasExpiradas);
            contexto.SaveChanges();
        } 
    }
 

    public async Task<List<Ofertas>> Listar(Expression<Func<Ofertas, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Ofertas
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}