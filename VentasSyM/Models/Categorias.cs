using System.ComponentModel.DataAnnotations;

namespace VentasSyM.Models;

public class Categorias
{
    [Key]
    public int CategoriaId { get; set; }
    [Required]
    public string Nombre { get; set; }
}
