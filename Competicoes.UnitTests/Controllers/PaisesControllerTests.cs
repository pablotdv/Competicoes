using Competicoes.Controllers;
using Competicoes.Data;
using Competicoes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Competicoes.UnitTests.Controllers
{
    public class PaisesControllerTests
    {

        [Fact]
        public async Task GetPaises()
        {
            var options = Options;

            Pais pais = await AdicionarPais(options);

            using(CompeticoesDbContext context = new CompeticoesDbContext(options))
            {
                PaisesController controller = new PaisesController(context);

                var result = controller.GetPaises();
                var paisResult = result.First();

                Assert.IsAssignableFrom<IEnumerable<Pais>>(result);
                Assert.True(result.Count() == await context.Paises.CountAsync());
                Assert.Equal(pais.PaisId, paisResult.PaisId);
            }
        }

        [Fact]
        public async Task GetPais_BadRequest()
        {
            var options = Options;

            using(CompeticoesDbContext context = new CompeticoesDbContext(options))
            {
                PaisesController controller = new PaisesController(context);

                controller.ModelState.AddModelError("Key", "Error");

                var result = await controller.GetPais(Guid.Empty);

                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var valueResult = Assert.IsType<SerializableError>(badRequestResult.Value);
                var value = Assert.IsType<string[]>(valueResult["Key"]);
                Assert.Equal("Error", value[0]);
            }
        }

        [Fact]
        public async Task GetPais_NotFound()
        {
            var options = Options;

            await AdicionarPais(options);

            using (CompeticoesDbContext context = new CompeticoesDbContext(options))
            {
                var controller = new PaisesController(context);

                var result = await controller.GetPais(Guid.NewGuid());

                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetPais_Ok()
        {
            var options = Options;

            var pais = await AdicionarPais(options);

            using(CompeticoesDbContext context = new CompeticoesDbContext(options))
            {
                var controller = new PaisesController(context);

                var result = await controller.GetPais(pais.PaisId);

                var okResult = Assert.IsType<OkObjectResult>(result);
                var paisResult = Assert.IsType<Pais>(okResult.Value);
                Assert.Equal(pais.PaisId, paisResult.PaisId);
            }
        }

        [Fact]
        public async Task PutPais_BadRequest_ModelState_Invalido()
        {
            var options = Options;

            using (CompeticoesDbContext context = new CompeticoesDbContext(options))
            {
                var controller = new PaisesController(context);

                controller.ModelState.AddModelError("Key", "Error");

                var result = await controller.PutPais(Guid.NewGuid(), new Pais());

                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var valueResult = Assert.IsType<SerializableError>(badRequestResult.Value);
                var value = Assert.IsType<string[]>(valueResult["Key"]);
                Assert.Equal("Error", value[0]);
            }
        }

        [Fact]
        public async Task PutPais_BadRequest_Id_Diferente()
        {
            var options = Options;

            using(CompeticoesDbContext context = new CompeticoesDbContext(options))
            {
                var controller = new PaisesController(context);

                var result = await controller.PutPais(Guid.NewGuid(), new Pais() { });

                var badResult = Assert.IsType<BadRequestResult>(result);
                Assert.Equal(400, badResult.StatusCode);
            }
        }

        [Fact]
        public async Task PutPais_NoContent()
        {
            var options = Options;

            var pais = await AdicionarPais(options);

            using (CompeticoesDbContext context = new CompeticoesDbContext(options))
            {
                pais.Nome = "Brazil";
                var controller = new PaisesController(context);

                var result = await controller.PutPais(pais.PaisId, pais);

                var noContentResult = Assert.IsType<NoContentResult>(result);
                Assert.Equal(204, noContentResult.StatusCode);
            }

            using(CompeticoesDbContext context = new CompeticoesDbContext(options))
            {
                var controller = new PaisesController(context);
                var result = await controller.GetPais(pais.PaisId);

                var okResult = Assert.IsType<OkObjectResult>(result);
                var paisResult = Assert.IsType<Pais>(okResult.Value);
                Assert.Equal(pais.PaisId, paisResult.PaisId);
                Assert.Equal(pais.Nome, paisResult.Nome);
            }
        }

        [Fact]
        public async Task PutPais_ConcurrencyExceptions_NotExists_NotFound()
        {
            var options = Options;

            var pais = await AdicionarPais(options);

            using (CompeticoesDbContext context = new CompeticoesDbContext(options))
            {
                context.Paises.Remove(pais);
                await context.SaveChangesAsync();
            }

            using (CompeticoesDbContext context = new CompeticoesDbContext(options))
            {
                pais.Nome = "Brazil";
                var controller = new PaisesController(context);

                var result = await controller.PutPais(pais.PaisId, pais);

                Assert.IsType<NotFoundResult>(result);
            }
        }       

        [Fact]
        public async Task PostPais_BadRequest_ModelState_Invalido()
        {
            var options = Options;

            using (CompeticoesDbContext context = new CompeticoesDbContext(options))
            {
                var controller = new PaisesController(context);

                controller.ModelState.AddModelError("Key", "Error");

                var result = await controller.PostPais(new Pais());

                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var valueResult = Assert.IsType<SerializableError>(badRequestResult.Value);
                var value = Assert.IsType<string[]>(valueResult["Key"]);
                Assert.Equal("Error", value[0]);
            }
        }

        [Fact]
        public async Task PostPais_Created()
        {
            var options = Options;

            var pais = new Pais()
            {
                PaisId = Guid.NewGuid(),
                Nome = "Brasil",
                Sigla = "BR",
            };

            using(CompeticoesDbContext context = new CompeticoesDbContext(options))
            {
                var controller = new PaisesController(context);

                var result = await controller.PostPais(pais);

                var createdResult = Assert.IsType<CreatedAtActionResult>(result);
                var paisResult = Assert.IsType<Pais>(createdResult.Value);
                Assert.Equal(pais, paisResult);
                Assert.Equal("GetPais", createdResult.ActionName);
                Assert.Equal(pais.PaisId, createdResult.RouteValues["id"]);
            }

            using(CompeticoesDbContext context = new CompeticoesDbContext(options))
            {
                var paisResult = await context.Paises.FirstOrDefaultAsync();
                var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(paisResult);
                var paisJson = Newtonsoft.Json.JsonConvert.SerializeObject(pais);

                Assert.Equal(paisJson, jsonResult);
            }
        }

        [Fact]
        public async Task DeletePais_BadRequest_ModelState_Invalido()
        {
            var options = Options;

            using (CompeticoesDbContext context = new CompeticoesDbContext(options))
            {
                PaisesController controller = new PaisesController(context);

                controller.ModelState.AddModelError("Key", "Error");

                var result = await controller.DeletePais(Guid.Empty);

                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var valueResult = Assert.IsType<SerializableError>(badRequestResult.Value);
                var value = Assert.IsType<string[]>(valueResult["Key"]);
                Assert.Equal("Error", value[0]);
            }
        }

        [Fact]
        public async Task DeletePais_NotFound()
        {
            var options = Options;

            await AdicionarPais(options);

            using (CompeticoesDbContext context = new CompeticoesDbContext(options))
            {
                var controller = new PaisesController(context);

                var result = await controller.DeletePais(Guid.NewGuid());

                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task DeltePais_Ok()
        {
            var options = Options;

            var pais = await AdicionarPais(options);

            using(CompeticoesDbContext context = new CompeticoesDbContext(options))
            {
                var controller = new PaisesController(context);

                var result = await controller.DeletePais(pais.PaisId);

                var okResult = Assert.IsType<OkObjectResult>(result);
                var paisResult = Assert.IsType<Pais>(okResult.Value);
            }

            using(CompeticoesDbContext context = new CompeticoesDbContext(options))
            {
                Assert.True(!(await context.Paises.AnyAsync(a => a.PaisId == pais.PaisId)));
            }
        }

        private async Task<Pais> AdicionarPais(DbContextOptions<CompeticoesDbContext> options)
        {
            var pais = new Pais()
            {
                PaisId = Guid.NewGuid(),
                Nome = "Brasil",
                Sigla = "BR"
            };

            using (CompeticoesDbContext context = new CompeticoesDbContext(options))
            {
                await context.Paises.AddAsync(pais);
                await context.SaveChangesAsync();
            }

            return pais;
        }

        public DbContextOptions<CompeticoesDbContext> Options => new DbContextOptionsBuilder<CompeticoesDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
    }
}
