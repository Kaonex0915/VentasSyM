using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VentasSyM.Models;

public class Ventas
{
    [Key]
    public int VentaId { get; set; }
    [Required]
    public DateTime FechaVendido { get; set; }

    [Required]
    public string Comprador { get; set; }

    [Required]
    public string Vendedor { get; set; }
    [Required]
    public int ProductoId { get; set; }

    [Required]
    public int UnidadesPorLote { get; set; }

    [Required]
    public decimal Total { get; set; }

    [ForeignKey("VentaId")]
    public List<VentasDetalle> VentasDetalles { get; set; } = new List<VentasDetalle>();
}
