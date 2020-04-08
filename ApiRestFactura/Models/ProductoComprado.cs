using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestFactura.Models
{
    public class ProductoComprado
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }

        [ForeignKey("Factura")]
        public int FacturaId { get; set; }
        [JsonIgnore]
        public Factura Factura { get; set; }
    }
}
