using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestFactura.Models
{
    public class Factura
    {
        public Factura()
        {
            ProductosComprados = new List<ProductoComprado>();
        }

        public int Id { get; set; }
        public string Ruc { get; set; }
        public int NoFactura { get; set; }
        public string NombreLocal { get; set; }
        public string DireccionLocal { get; set; }
        public int Telefono { get; set; }

        public string UsuarioVendedor { get; set; }
        public string NombreVendedor { get; set; }
        public int NoCaja { get; set; }
        public string FechaCompra { get; set; }

        public string NombreCliente { get; set; }
        public string IdentificacionCliente { get; set; }

        public List<ProductoComprado> ProductosComprados { get; set; }

        public double TotalPago { get; set; }

        public string UsuarioUltimaModificacion { get; set; }
        public string FechaUltimaModificacion { get; set; }
    }
}
