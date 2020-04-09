using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiRestFactura.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ApiRestFactura
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("facturasDB"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = "yourdomain.com",
                     ValidAudience = "yourdomain.com",
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Secret:Key"])),
                     ClockSkew = TimeSpan.Zero
                 });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(ConfigureJson);
        }

        private void ConfigureJson(MvcJsonOptions obj)
        {
            obj.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMvc();

            if (!context.Facturas.Any())
            {
                context.Facturas.AddRange(new List<Factura>() {

                    new Factura() {
                        Ruc = "1111111111001",
                        NoFactura = 1,
                        NombreLocal = "Pepito S.A.",
                        DireccionLocal = "Av Quito",
                        Telefono = 042032312,
                        UsuarioVendedor = "vendedorA",
                        NombreVendedor = "Primer Vendedor",
                        NoCaja = 1,
                        FechaCompra = "01/04/2020",
                        NombreCliente = "Luis Ponce",
                        IdentificacionCliente = "0932323232",
                        ProductosComprados = new List<ProductoComprado>
                        {
                            new ProductoComprado { Codigo = 111, Cantidad = 2, Descripcion = "Producto A", Precio = 100 },
                            new ProductoComprado { Codigo = 222, Cantidad = 1, Descripcion = "Producto B", Precio = 200 }
                        },
                        TotalPago = 300
                    },
                    new Factura(){
                        Ruc = "1111111111001",
                        NoFactura = 2,
                        NombreLocal = "Pepito S.A.",
                        DireccionLocal = "Av Quito",
                        Telefono = 042032312,
                        UsuarioVendedor = "vendedorB",
                        NombreVendedor = "Segundo Vendedor",
                        NoCaja = 1,
                        FechaCompra = "01/04/2020",
                        NombreCliente = "Lupe Sánchez",
                        IdentificacionCliente = "0921212121",
                        ProductosComprados = new List<ProductoComprado>
                        {
                            new ProductoComprado { Codigo = 111, Cantidad = 2, Descripcion = "Producto A", Precio = 10 },
                            new ProductoComprado { Codigo = 222, Cantidad = 1, Descripcion = "Producto B", Precio = 20 }
                        },
                        TotalPago = 30
                    },
                    new Factura(){
                        Ruc = "2222222222001",
                        NoFactura = 3,
                        NombreLocal = "Juanito S.A.",
                        DireccionLocal = "Av Machala",
                        Telefono = 042032312,
                        UsuarioVendedor = "vendedorB",
                        NombreVendedor = "Segundo Vendedor",
                        NoCaja = 5,
                        FechaCompra = "07/04/2020",
                        NombreCliente = "Luis Ponce",
                        IdentificacionCliente = "0932323232",
                        ProductosComprados = new List<ProductoComprado>
                        {
                            new ProductoComprado { Codigo = 99, Cantidad = 2, Descripcion = "Producto A", Precio = 30 },
                            new ProductoComprado { Codigo = 10, Cantidad = 1, Descripcion = "Producto B", Precio = 30 }
                        },
                        TotalPago = 90
                    }
                });

                context.SaveChanges();
            }
        }
    }
}
