using System.ComponentModel.DataAnnotations;

namespace VentasSyM.Models;

public class Productos
{
    [Key]
    public int ProductoId { get; set; }
  
    [Required]
    public string Nombre {  get; set; }

    [Required]
    public string Descripcion { get; set; }
    
    [Required]
    public DateTime FechaVencimiento { get; set; }
    
    [Required]
    public string Categoria { get; set; }

    [Required]
    public Decimal Costo { get; set; }
    [Required]
    public Decimal Precio { get; set; }
    [Required]
    public int Existencia { get; set; }
    [Required]
    public int Cantidad { get; set; }
    public bool TieneFechaVencimiento { get; set; }

}
