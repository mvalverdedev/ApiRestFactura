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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FacturaController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public FacturaController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Factura> Get()
        {
            return context.Facturas;
        }

        [HttpGet("{Id}", Name = "FacturaCreada")]
        public IActionResult Get(int Id)
        {
            var Factura = context.Facturas.Include(x => x.ProductosComprados).FirstOrDefault(x => x.Id == Id);
            if (Factura != null)
            {
                return Ok(Factura);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Factura Factura)
        {
            if (ModelState.IsValid)
            {
                context.Facturas.Add(Factura);
                context.SaveChanges();
                return new CreatedAtRouteResult("FacturaCreada", new { NoFactura = Factura.Id }, Factura);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{Id}")]
        public IActionResult Put(int Id, [FromBody] Factura Factura)
        {
            if (!context.Facturas.Any(m => m.Id == Id))
            {
                return NotFound();
            }
            else if (Id != Factura.Id)
            {
                return BadRequest();
            }

            context.Entry(Factura).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            var FacturaEliminar = context.Facturas.FirstOrDefault(m => m.Id == Id);

            if (FacturaEliminar == null)
            {
                return NotFound();
            }

            context.Facturas.Remove(FacturaEliminar);
            context.SaveChanges();
            return Ok(FacturaEliminar);
        }



        //[HttpGet]
        //[Route("api/GetFacturasPorVendedor/{UsuarioVendedor}")]
        //public ActionResult<IEnumerable<Factura>> GetFacturasPorVendedor(string UsuarioVendedor)
        //{
        //    return Facturas.Where(x => x.UsuarioVendedor == UsuarioVendedor).ToList();
        //}

        //[HttpGet("{Ruc}")]
        //public ActionResult<IEnumerable<Factura>> GetFacturasPorRuc(string Ruc)
        //{
        //    return Facturas.Where(x => x.Ruc == Ruc).ToList();
        //}

        //[HttpGet("{IdentificacionCliente}")]
        //public ActionResult<IEnumerable<Factura>> GetFacturasPorCliente(string IdentificacionCliente)
        //{
        //    return Facturas.Where(x => x.IdentificacionCliente == IdentificacionCliente).ToList();
        //}

    }
}