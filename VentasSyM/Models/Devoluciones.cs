namespace VentasSyM.Models
{
    public class Devoluciones
    {
        public int DevolucionId { get; set; }
        public string Comprador { get; set; }
        public string Vendedor { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public List<DevolucionDetalle> DevolucionDetalles { get; set; } = new();
    }
}
