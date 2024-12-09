using System.ComponentModel.DataAnnotations;

namespace VentasSyM.Models;

public class Ofertas
{
    [Key]
    public int OfertaId { get; set; }
    public int ProductoId { get; set; }
    public decimal? Descuento { get; set; }
    public DateTime FechaFin { get; set; } = DateTime.Now;
}
