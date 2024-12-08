using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VentasSyM.Models;

public class Devoluciones
{
    [Key]
    public int DevolucionId { get; set; }
    public string Comprador { get; set; }
    public string Vendedor { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;

    [ForeignKey("DevolucionId")]
    public ICollection<DevolucionDetalle> DevolucionDetalle { get; set; } = new List<DevolucionDetalle>();
}
