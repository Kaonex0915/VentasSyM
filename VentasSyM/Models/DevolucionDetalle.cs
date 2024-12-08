using System.ComponentModel.DataAnnotations;

namespace VentasSyM.Models;

public class DevolucionDetalle
{
    [Key]
    public int DetalleId { get; set; }
    public int ProductoId { get; set; }
    public int DevolucionId { get; set; }
    public int? Cantidad { get; set; }
    public decimal? Precio { get; set; }
    public string? Motivo { get; set; }

    public int UnidadDevuelta { get; set; }
}
