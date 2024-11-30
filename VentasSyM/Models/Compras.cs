using System.ComponentModel.DataAnnotations;

namespace VentasSyM.Models
{
    public class Compras
    {
        [Key]
        public int CompraId { get; set; }
        [Required]
        public DateTime FechaCompra {  get; set; }
        [Required]
        public string Comprador { get; set; }
        [Required]
        public int ProductoId { get; set; }
       
        [Required]
        public int UnidadesPorLote { get; set; }

        [Required]
        public decimal Total {  get; set; }



    }
}
