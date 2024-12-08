namespace VentasSyM.Models
{
    public class DevolucionDetalle
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public string Motivo { get; set; }

        public int UnidadDevuelta { get; set; }
    }
}
