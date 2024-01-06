using MainDefontana.Data;
using MainDefontana.Models;

namespace MainDefontana.Bussines
{
    public class Venta
    {
        public async Task<List<VentaModel>> ObtenerDatos()
        {
            try
            {
                SqlHelper slq = new();
                return await slq.SimpleQueryAsync<VentaModel>(@"SELECT V.ID_Venta AS 'CodigoVenta', v.Total as 'TotalVenta',v.ID_Local as 'CodigoLocal',l.Nombre as 'NombreLocal',vd.Precio_Unitario 'PrecioUnitaro',vd.ID_Producto as 'CodigoProducto',p.Nombre as 'NombreProducto',v.Fecha,vd.TotalLinea as 'Total',vd.Cantidad, M.ID_Marca AS 'CodigoMarca', M.Nombre AS 'NombreMarca' FROM Venta(NOLOCK) V, VentaDetalle(NOLOCK) VD, Producto(NOLOCK)P,Local(NOLOCK)L, Marca(NOLOCK)M WHERE V.ID_Venta = VD.ID_Venta AND V.ID_Local = L.ID_Local AND VD.ID_Producto = P.ID_Producto AND P.ID_Marca = M.ID_Marca"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(ex.Message);
            }
        } 

        public async void ConsultarDatos(int dias)
        {
            try
            {
                DateTime fechaLimite = DateTime.Now.AddDays(-dias);

                var datos = await ObtenerDatos();

                
                var Ventas = datos.Where(v => v.Fecha >= fechaLimite).ToList();

                // 1
                int numeroVentas = Ventas.Count;
                decimal totalVentas =  Ventas.Sum(v => v.Total);
                Console.WriteLine("1. EL TOTAL DE VENTAS DE LOS ÚLTIMOS 30 DÍAS (MONTO TOTAL Y Q DE VENTAS)");
                Console.WriteLine($"TOTAL VENTAS DE LOS ULTIMOS 30 DIAS => {totalVentas:n2}, Numero de Ventas => {numeroVentas}\n");
               
                // 2
                var ventaMasAlta = Ventas.OrderByDescending(v => v.TotalVenta).FirstOrDefault();
                Console.WriteLine("2. EL DÍA Y HORA EN QUE SE REALIZÓ LA VENTA CON EL MONTO MÁS ALTO (Y CUÁL ES AQUEL MONTO)");
                Console.WriteLine($"VENTA => {ventaMasAlta.CodigoVenta}, DIA => {ventaMasAlta.Fecha:dddd}, HORA => {ventaMasAlta.Fecha:HH:mm}, VENTA => {ventaMasAlta.TotalVenta:n2}\n");

                // 3
                var productoConMasVenta = Ventas.GroupBy(
                    x => new { x.CodigoProducto, x.NombreProducto }
                    ).Select( x => new {
                        x.Key.CodigoProducto,
                        x.Key.NombreProducto,
                        TotalVenta = x.Sum( c => c.Total ),
                }).OrderByDescending(x => x.TotalVenta).FirstOrDefault();
                Console.WriteLine("3. INDICAR CUÁL ES EL PRODUCTO CON MAYOR MONTO TOTAL DE VENTAS");
                Console.WriteLine($"PRODUCTO => {productoConMasVenta.CodigoProducto} - {productoConMasVenta.NombreProducto}, VENTA => {productoConMasVenta.TotalVenta} \n");

                // 4
                var localConMasVenta = Ventas.GroupBy(
                    x => new { x.CodigoLocal, x.NombreLocal }
                    ).Select(x => new {
                        x.Key.CodigoLocal,
                        x.Key.NombreLocal,
                        TotalVenta = x.Sum(c => c.Total),
                    }).OrderByDescending(x => x.TotalVenta).FirstOrDefault();
                Console.WriteLine("4. INDICAR EL LOCAL CON MAYOR MONTO DE VENTAS");
                Console.WriteLine($"LOCAL => {localConMasVenta.CodigoLocal} - {localConMasVenta.NombreLocal}, VENTA => {localConMasVenta.TotalVenta}\n");

                // 5
                var marcaConMasVenta = Ventas.GroupBy(
                    x => new { x.CodigoMarca, x.NombreMarca }
                    ).Select(x => new {
                        x.Key.CodigoMarca,
                        x.Key.NombreMarca,
                        TotalVenta = x.Sum(c => c.Total),
                    }).OrderByDescending(x => x.TotalVenta).FirstOrDefault();
                Console.WriteLine("5. ¿CUÁL ES LA MARCA CON MAYOR MARGEN DE GANANCIAS?");
                Console.WriteLine($"MARCA => {marcaConMasVenta.CodigoMarca} - {marcaConMasVenta.NombreMarca}, GANANCIA => {marcaConMasVenta.TotalVenta}\n");

                // 6
                var productosMasVendidosPorLocal = Ventas
                    .GroupBy(v => new { v.CodigoLocal, v.CodigoProducto })
                    .Select(g => new
                    {
                        g.Key.CodigoLocal,
                        g.Key.CodigoProducto,
                        TotalVentas = g.Sum(v => v.Total)
                    })
                    .GroupBy(g => g.CodigoLocal)
                    .Select(g => g.OrderByDescending(p => p.TotalVentas).First())
                    .ToList();
                Console.WriteLine("6. ¿CÓMO OBTENDRÍAS CUÁL ES EL PRODUCTO QUE MÁS SE VENDE EN CADA LOCAL?");
                foreach (var i in productosMasVendidosPorLocal)
                {
                    Console.WriteLine($"Local: {i.CodigoLocal}, Producto: {i.CodigoProducto}, Total Ventas: {i.TotalVentas}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(ex.Message);
            }
            
        }
    }
}
