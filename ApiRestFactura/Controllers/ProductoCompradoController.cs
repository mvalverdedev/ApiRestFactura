using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiRestFactura.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiRestFactura.Controllers
{
    [Route("api/Factura/{FacturaId}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductoCompradoController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ProductoCompradoController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<ProductoComprado> GetProductosPorFactura(int FacturaId)
        {
            return context.ProductosComprados.Where(x => x.FacturaId == FacturaId);
        }

        [HttpGet("{Id}", Name = "ProductoCreado")]
        public IActionResult Get(int Id)
        {
            var ProductoComprado = context.ProductosComprados.FirstOrDefault(x => x.Id == Id);
            if (ProductoComprado != null)
            {
                return Ok(ProductoComprado); //new ObjectResult(ProductoComprado); //Es lo mismo
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Post(int FacturaId, [FromBody] ProductoComprado ProductoComprado)
        {
            ProductoComprado.FacturaId = FacturaId;

            if (ModelState.IsValid)
            {
                context.ProductosComprados.Add(ProductoComprado);
                context.SaveChanges();
                return new CreatedAtRouteResult("ProductoCreado", new { Id = ProductoComprado.Id }, ProductoComprado);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{Id}")]
        public IActionResult Put(int Id, int FacturaId, [FromBody] ProductoComprado ProductoComprado)
        {
            if (!context.ProductosComprados.Any(m => m.Id == Id))
            {
                return NotFound();
            }
            else if (Id != ProductoComprado.Id)
            {
                return BadRequest();
            }

            ProductoComprado.FacturaId = FacturaId;

            context.Entry(ProductoComprado).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            var ProductoEliminar = context.ProductosComprados.FirstOrDefault(m => m.Id == Id);

            if (ProductoEliminar == null)
            {
                return NotFound();
            }

            context.ProductosComprados.Remove(ProductoEliminar);
            context.SaveChanges();
            return Ok(ProductoEliminar);
        }
    }
}