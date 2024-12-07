using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VentasSyM.Models;

public class Compras
{
    [Key]
    public int CompraId { get; set; }
    [Required]
    public DateTime FechaCompra {  get; set; }
    [Required]
    public string Comprador { get; set; }
    [Required]
    public int UnidadesPorLote { get; set; }

    [Required]
    public decimal Total {  get; set; }

    [Required]
    public string Vendedor { get; set; }

    [ForeignKey("CompraId")]
    public List <ComprasDetalle> comprasDetalles { get; set; } = new List<ComprasDetalle>();
}
