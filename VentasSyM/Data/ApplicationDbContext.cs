using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VentasSyM.Models;

namespace VentasSyM.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Productos> Productos { get; set; }
        public DbSet<Compras> Compras { get; set; }
        public DbSet<Ventas> Ventas { get; set; }
        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<ComprasDetalle> ComprasDetalle { get; set; }
        public DbSet<VentasDetalle> VentasDetalles { get; set; }
        public DbSet<Devoluciones> Devoluciones { get; set; }
        public DbSet<DevolucionDetalle> DevolucionDetalles { get; set; }
        public DbSet<Ofertas> Ofertas { get; set; }

        /* protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
             modelBuilder.Entity<Productos>().HasData
             (
                 new Productos
                 {
                     ProductoId = 1,
                     Nombre = "Tanque De Gas ",
                     Descripcion = "25 Libras Bombona Cilindro Capacidad 5 Galones",
                     Costo = 3000,
                     Precio = 3500,
                     Existencia = 3,
                     Categoria = "Hogar y electrodomesticos",
                     TieneFechaVencimiento = false
                 },

                 new Productos
                 {
                     ProductoId = 2,
                     Nombre = "Lentes Realidad Virtual",
                     Descripcion = "GAFAS VRBOX 2.0",
                     Costo = 850,
                     Precio = 1000,
                     Existencia = 4,
                     Categoria = "Consolas y videojuegos",
                     TieneFechaVencimiento = false
                 },

                 new Productos
                 {
                     ProductoId = 3,
                     Nombre = "Taburete Tapizado De Cuero",
                     Descripcion = "Patas De Hierro, Silla Altahecho de asiento de piel sint�tica y marco de metal de hierro duradero, capacidad m�xima de peso: 440.9 lbs",
                     Costo = 2900,
                     Precio = 3100,
                     Existencia = 13,
                     Categoria = "Hogar y electrodomesticos",
                     TieneFechaVencimiento = false
                 },

                 new Productos
                 {
                     ProductoId = 4,
                     Nombre = "Kit de mancuernas 30Kg",
                     Descripcion = " Mancuerna ajustable, 30KG, cromada ajustable de primera clase 30KG: (.5KG+1.25KG+2.5KG)*2PCS+25mm*14\"T Bar x2",
                     Costo = 7020,
                     Precio = 8000,
                     Existencia = 10,
                     Categoria = "Deportes y fitness",
                     TieneFechaVencimiento = false

                 }
             );
         }*/
    }
}
