using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VentasSyM.Data;
using VentasSyM.Models;

namespace VentasSyM.Services
{
    public class CompraServices(IDbContextFactory<ApplicationDbContext> DbFactory)
    {
        private readonly ApplicationDbContext _context;

        public async Task<bool> Existe(int CompraId)
        {
            await using var _context = await DbFactory.CreateDbContextAsync();
            return await _context.Compras.AnyAsync(c => c.CompraId == CompraId);

        }

        public async Task<bool> Insertar(Compras compras)
        {
            await using var _context = await DbFactory.CreateDbContextAsync();

            foreach (var compra in compras.comprasDetalles)
            {
                var producto = await BuscarProductos(compra.ProductoId);

                if (producto != null)
                {
                    if (producto.Existencia < compra.UnidadUtilizada)
                    {
                        return false;
                    }
                    producto.Existencia += compra.UnidadUtilizada;
                    _context.Productos.Update(producto);
                    await _context.SaveChangesAsync();
                }
                else
                {

                    return false;
                }
            }
            _context.Compras.Add(compras);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<bool> Modificar(Compras compras)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();

            contexto.Update(compras);
            return await contexto.SaveChangesAsync() > 0;
        }


        public async Task<bool> Guardar(Compras compras)
        {
            if (!await Existe(compras.CompraId))
            {
                return await Insertar(compras);
            }
            else
                return await Modificar(compras);
        }


        public async Task<bool> Eliminar(int CompraId)
        {
            await using var _context = await DbFactory.CreateDbContextAsync();
            var detalles = await BuscarCompraDetalle(CompraId);

            foreach (var detalle in detalles)
            {
                var producto = await BuscarProductos(detalle.ProductoId);
                if (producto != null)
                {
                    producto.Existencia += detalle.UnidadUtilizada;
                    await ActualizarProducto(producto);
                }
            }
            var cobro = await _context.Compras
                        .Include(c => c.comprasDetalles)
                        .FirstOrDefaultAsync(c => c.CompraId == CompraId);

            if (cobro == null) return false;

            _context.ComprasDetalle.RemoveRange(cobro.comprasDetalles);
            _context.Compras.Remove(cobro);

            var cantidad = await _context.SaveChangesAsync();
            return cantidad > 0;
        }


        public async Task<bool> EliminarDetalle(int detalleId)
        {
            await using var _context = await DbFactory.CreateDbContextAsync();
            if (await ExisteDetalle(detalleId))
            {
                var compraDetalle = await _context.ComprasDetalle.FirstOrDefaultAsync(c => c.DetalleId == detalleId);

                var producto = await _context.Productos.FindAsync(compraDetalle.ProductoId);

                if (producto is null)
                {
                    return false;
                }
                else
                {
                    producto.Existencia += compraDetalle.UnidadUtilizada;
                    _context.Productos.Update(producto);
                }
                _context.ComprasDetalle.Remove(compraDetalle);
            }

            else
            {
                var compras = await _context.ComprasDetalle.FirstOrDefaultAsync(c => c.DetalleId == detalleId);

                if (compras is null)
                {
                    return false;
                }

                _context.ComprasDetalle.Remove(compras);
            }

            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<Productos> BuscarProductos(int id)
        {
            await using var _context = await DbFactory.CreateDbContextAsync();
            return await _context.Productos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductoId == id);
        }


        public async Task<bool> ActualizarProducto(Productos productos)
        {
            await using var _context = await DbFactory.CreateDbContextAsync();
            _context.Productos.Update(productos);
            return await _context
                .SaveChangesAsync() > 0;
        }


        public async Task<Compras> Buscar(int CompraId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Compras
                .Include(c => c.comprasDetalles)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CompraId == CompraId);
        }


        public async Task<List<Compras>> Listar(Expression<Func<Compras, bool>> criterio)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Compras
                .Include(c => c.comprasDetalles)
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


        public async Task<List<ComprasDetalle>> BuscarCompraDetalle(int CompraId)
        {
            await using var _context = await DbFactory.CreateDbContextAsync();
            return await _context.ComprasDetalle
                .Include(p => p.ProductoId)
                .Where(c => c.CompraId == CompraId)
                 .AsNoTracking()
                .ToListAsync();
        }


        public async Task<List<ComprasDetalle>> ListarComprasDetalle(int CompraId)
        {
            await using var _context = await DbFactory.CreateDbContextAsync();
            var cotizacionDetalle = await _context.ComprasDetalle
                .Where(c => c.CompraId == CompraId)
                .ToListAsync();

            return cotizacionDetalle;
        }


        private async Task<bool> ExisteDetalle(int detalleId)
        {
            await using var _context = await DbFactory.CreateDbContextAsync();
            return await _context.ComprasDetalle.AnyAsync(ed => ed.DetalleId == detalleId);
        }
    }

}
