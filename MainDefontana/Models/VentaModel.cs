
namespace MainDefontana.Models
{
    public class VentaModel
    {
        public int CodigoVenta { get; set; }
        public int CodigoLocal { get; set; }
        public string NombreLocal { get; set; }
        public int CodigoProducto { get; set; }
        public string NombreProducto { get; set; }
        public int CodigoMarca { get; set; }
        public string NombreMarca { get; set; }
        public decimal PrecioUnitario { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public decimal TotalVenta { get; set; }
        public int Cantidad { get; set; }

    }
}
