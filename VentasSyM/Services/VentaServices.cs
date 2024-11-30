using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VentasSyM.DAL;
using VentasSyM.Models;

namespace VentasSyM.Services
{
    public class VentaServices(IDbContextFactory<Context> DbFactory)
    {
        private readonly Context _context;

        public async Task<bool> Existe(int VentaId)
        {
            await using var _context = await DbFactory.CreateDbContextAsync();
            return await _context.Ventas.AnyAsync(v => v.VentaId == VentaId);

        }

        public async Task<bool> Insertar(Ventas ventas)
        {
            await using var _context = await DbFactory.CreateDbContextAsync();

            foreach (var venta in ventas.VentasDetalles)
            {
                var producto = await BuscarProductos(venta.ProductoId);

                if (producto != null)
                {
                    if (producto.Existencia < venta.UnidadUtilizada)
                    {
                        return false;
                    }
                    producto.Existencia -= venta.UnidadUtilizada;
                    _context.Productos.Update(producto);
                    await _context.SaveChangesAsync();
                }
                else
                {

                    return false;
                }
            }
            _context.Ventas.Add(ventas);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<bool> Modificar(Ventas ventas)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();

            contexto.Update(ventas);
            return await contexto.SaveChangesAsync() > 0;
        }


        public async Task<bool> Guardar(Ventas ventas)
        {
            if (!await Existe(ventas.VentaId))
            {
                return await Insertar(ventas);
            }
            else
                return await Modificar(ventas);
        }


        public async Task<bool> Eliminar(int VentaId)
        {
            await using var _context = await DbFactory.CreateDbContextAsync();
            var detalles = await BuscarVentaDetalle(VentaId);

            foreach (var detalle in detalles)
            {
                var producto = await BuscarProductos(detalle.ProductoId);
                if (producto != null)
                {
                    producto.Existencia += detalle.UnidadUtilizada;
                    await ActualizarProducto(producto);
                }
            }
            var cobro = await _context.Ventas
                        .Include(c => c.VentasDetalles)
                        .FirstOrDefaultAsync(v => v.VentaId == VentaId);

            if (cobro == null) return false;

            _context.VentasDetalles.RemoveRange(cobro.VentasDetalles);
            _context.Ventas.Remove(cobro);

            var cantidad = await _context.SaveChangesAsync();
            return cantidad > 0;
        }


        public async Task<bool> EliminarDetalle(int detalleId)
        {
            await using var _context = await DbFactory.CreateDbContextAsync();
            if (await ExisteDetalle(detalleId))
            {
                var ventaDetalle = await _context.VentasDetalles.FirstOrDefaultAsync(v => v.DetalleId == detalleId);

                var producto = await _context.Productos.FindAsync(ventaDetalle.ProductoId);

                if (producto is null)
                {
                    return false;
                }
                else
                {
                    producto.Existencia += ventaDetalle.UnidadUtilizada;
                    _context.Productos.Update(producto);
                }
                _context.VentasDetalles.Remove(ventaDetalle);
            }

            else
            {
                var ventas = await _context.VentasDetalles.FirstOrDefaultAsync(v => v.DetalleId == detalleId);

                if (ventas is null)
                {
                    return false;
                }

                _context.VentasDetalles.Remove(ventas);
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


        public async Task<Ventas> Buscar(int VentaId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Ventas
                .Include(c => c.VentasDetalles)
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.VentaId == VentaId);
        }


        public async Task<List<Ventas>> Listar(Expression<Func<Ventas, bool>> criterio)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Ventas
                .Include(v => v.VentasDetalles)
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


        public async Task<List<VentasDetalle>> BuscarVentaDetalle(int VentaId)
        {
            await using var _context = await DbFactory.CreateDbContextAsync();
            return await _context.VentasDetalles
                .Include(p => p.ProductoId)
                .Where(v => v.VentaId == VentaId)
                 .AsNoTracking()
                .ToListAsync();
        }


        public async Task<List<VentasDetalle>> ListarVentasDetalle(int VentaId)
        {
            await using var _context = await DbFactory.CreateDbContextAsync();
            var cotizacionDetalle = await _context.VentasDetalles
                .Where(v => v.VentaId == VentaId)
                .ToListAsync();

            return cotizacionDetalle;
        }


        private async Task<bool> ExisteDetalle(int detalleId)
        {
            await using var _context = await DbFactory.CreateDbContextAsync();
            return await _context.VentasDetalles.AnyAsync(ed => ed.DetalleId == detalleId);
        }
    }

}
