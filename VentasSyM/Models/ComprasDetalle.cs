using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VentasSyM.Models;

public class ComprasDetalle
{
    [Key]
    public int DetalleId { get; set; } 
    [Required]
    public int CompraId { get; set; }
    [Required]
    public int ProductoId{ get; set; }
    [Required]
    public int UnidadUtilizada { get; set; }
    [Required]
    public decimal Precio {  get; set; }

    [NotMapped]
    public decimal PrecioTotal {
        get {  return UnidadUtilizada * Precio; }
    } 
}
