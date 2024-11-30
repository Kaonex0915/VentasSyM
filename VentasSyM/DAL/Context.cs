using Microsoft.EntityFrameworkCore;
using VentasSyM.Models;

namespace VentasSyM.DAL;

public class Context:DbContext
{
    public Context (DbContextOptions<Context> options) : base(options) { }

    public DbSet<Productos> Productos { get; set; }
    public DbSet<Compras> Compras { get; set; }
    public DbSet<Ventas> Ventas { get; set; }
    public DbSet<ComprasDetalle> ComprasDetalle { get; set; }
    public DbSet<VentasDetalle> VentasDetalles { get; set; }
}
