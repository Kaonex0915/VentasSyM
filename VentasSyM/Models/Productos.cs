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
    public int CategoriaId { get; set; }

    [Required]
    public decimal Costo { get; set; }
    [Required]
    public decimal Precio { get; set; }
    [Required]
    public int Existencia { get; set; }
    [Required]
    public int Cantidad { get; set; }
    public bool TieneFechaVencimiento { get; set; }

}
