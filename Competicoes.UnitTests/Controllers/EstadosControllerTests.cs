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
    public class EstadosControllerTests
    {
        private DbContextOptions<CompeticoesDbContext> Options =>
            new DbContextOptionsBuilder<CompeticoesDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        private async Task<Estado> AdicionarEstado(
            DbContextOptions<CompeticoesDbContext> options)
        {
            var estado = new Estado()
            {
                EstadoId = Guid.NewGuid(),
                Nome = "Rio Grande do Sul",
                Sigla = "RS",
            };

            using (CompeticoesDbContext context = new CompeticoesDbContext(options))
            {
                await context.Estados.AddAsync(estado);
                await context.SaveChangesAsync();
            }

            return estado;
        }

        [Fact]
        public async Task GetEstados()
        {
            var options = Options;

            var estado = await AdicionarEstado(options);

            using (CompeticoesDbContext context = new CompeticoesDbContext(options))
            {
                EstadosController controller = new EstadosController(context);

                var result = controller.GetEstados();
                var estadoResult = result.First();

                Assert.IsAssignableFrom<IEnumerable<Estado>>(result);
                Assert.True(result.Count() == await context.Estados.CountAsync());
                Assert.Equal(estado.EstadoId, estadoResult.EstadoId);
            }
        }

        [Fact]
        public async Task GetEstado_BadRequest()
        {
            var options = Options;

            using (CompeticoesDbContext context = new CompeticoesDbContext(options))
            {
                var controller = new EstadosController(context);

                controller.ModelState.AddModelError("key", "error");

                var result = await controller.GetEstado(Guid.Empty);

                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var valueResult = Assert.IsType<SerializableError>(badRequestResult.Value);
                var value = Assert.IsType<string[]>(valueResult["key"]);
                Assert.Equal("error", value[0]);
            }
        }

        [Fact]
        public async Task GetEstado_NotFound()
        {
            var options = Options;

            using (CompeticoesDbContext context = new CompeticoesDbContext(options))
            {
                var controller = new EstadosController(context);

                var result = await controller.GetEstado(Guid.NewGuid());

                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetEstado_Ok()
        {
            var options = Options;

            Estado estado = await AdicionarEstado(options);

            using (var context = new CompeticoesDbContext(options))
            {
                var controller = new EstadosController(context);

                var result = await controller.GetEstado(estado.EstadoId);

                var okResult = Assert.IsType<OkObjectResult>(result);
                var estadoResult = Assert.IsType<Estado>(okResult.Value);
                Assert.Equal(estado.EstadoId, estadoResult.EstadoId);
            }
        }

        [Fact]
        public async Task PutEstado_BadRequest_ModelState_Invalid()
        {
            var options = Options;

            using (var context = new CompeticoesDbContext(options))
            {
                var controller = new EstadosController(context);

                controller.ModelState.AddModelError("key", "error");

                var result = await controller.PutEstado(Guid.NewGuid(), new Estado());

                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var valueResult = Assert.IsType<SerializableError>(badRequestResult.Value);
                var value = Assert.IsType<string[]>(valueResult["key"]);
                Assert.Equal("error", value[0]);
            }
        }

        [Fact]
        public async Task PutEstado_BadRequest_Id_Diferente()
        {
            var options = Options;

            using (var context = new CompeticoesDbContext(options))
            {
                var controller = new EstadosController(context);

                var result = await controller.PutEstado(Guid.NewGuid(), new Estado());

                var badRequestResult = Assert.IsType<BadRequestResult>(result);
                Assert.Equal(400, badRequestResult.StatusCode);
            }
        }

        [Fact]
        public async Task PutEstado_NoContet()
        {
            var options = Options;

            var estado = await AdicionarEstado(options);

            using (var context = new CompeticoesDbContext(options))
            {
                estado.Nome = "Santa Catarina";
                var controller = new EstadosController(context);

                var result = await controller.PutEstado(estado.EstadoId, estado);

                var noContentResult = Assert.IsType<NoContentResult>(result);
                Assert.Equal(204, noContentResult.StatusCode);
            }

            using (var context = new CompeticoesDbContext(options))
            {
                var controller = new EstadosController(context);
                var result = await controller.GetEstado(estado.EstadoId);
                var okResult = Assert.IsType<OkObjectResult>(result);
                var estadoResult = Assert.IsType<Estado>(okResult.Value);
                Assert.Equal(estado.EstadoId, estadoResult.EstadoId);
                Assert.Equal(estado.Nome, estadoResult.Nome);
            }
        }

        [Fact]
        public async Task PutEstado_ConcurrencyException_NotExists_NotFound()
        {
            var options = Options;

            var estado = await AdicionarEstado(options);

            using (var context = new CompeticoesDbContext(options))
            {
                context.Estados.Remove(estado);
                await context.SaveChangesAsync();
            }

            using (var context = new CompeticoesDbContext(options))
            {
                estado.Nome = "Santa Catarina";
                var controller = new EstadosController(context);

                var result = await controller.PutEstado(estado.EstadoId, estado);

                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task PostEstado_BadRequest_ModelState_Invalid()
        {
            var options = Options;

            using (var context = new CompeticoesDbContext(options))
            {
                var controller = new EstadosController(context);

                controller.ModelState.AddModelError("key", "error");

                var result = await controller.PostEstado(new Estado());

                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var valueResult = Assert.IsType<SerializableError>(badRequestResult.Value);
                var value = Assert.IsType<string[]>(valueResult["key"]);
                Assert.Equal("error", value[0]);
            }
        }

        [Fact]
        public async Task PostEstado_Created()
        {
            var options = Options;
            var estado = new Estado()
            {
                EstadoId = Guid.NewGuid(),
                Nome = "Rio Grande do Sul",
                Sigla = "RS",
            };

            using (var context = new CompeticoesDbContext(options))
            {
                var controller = new EstadosController(context);

                var result = await controller.PostEstado(estado);

                var createdResult = Assert.IsType<CreatedAtActionResult>(result);
                var estadoResult = Assert.IsType<Estado>(createdResult.Value);
                Assert.Equal(estado, estadoResult);
                Assert.Equal("GetEstado", createdResult.ActionName);
                Assert.Equal(estado.EstadoId, createdResult.RouteValues["id"]);
            }

            using (var context = new CompeticoesDbContext(options))
            {
                var estadoResult = await context.Estados.FirstOrDefaultAsync();
                var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(estadoResult);
                var estadoJson = Newtonsoft.Json.JsonConvert.SerializeObject(estado);

                Assert.Equal(estadoJson, jsonResult);
            }
        }
        [Fact]
        public async Task DeleteEstado_BadRequest_ModelState_Invalid()
        {
            var options = Options;

            using (var context = new CompeticoesDbContext(options))
            {
                var controller = new EstadosController(context);

                controller.ModelState.AddModelError("key", "error");

                var result = await controller.DeleteEstado(Guid.NewGuid());

                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var valueResult = Assert.IsType<SerializableError>(badRequestResult.Value);
                var value = Assert.IsType<string[]>(valueResult["key"]);
                Assert.Equal("error", value[0]);
            }
        }

        [Fact]
        public async Task DeleteEstado_NotFound()
        {
            var options = Options;

            using (var context = new CompeticoesDbContext(options))
            {
                var controller = new EstadosController(context);

                var result = await controller.DeleteEstado(Guid.NewGuid());
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task DeleteEstado_Ok()
        {
            var options = Options;

            var estado = await AdicionarEstado(options);

            using(var context = new CompeticoesDbContext(options))
            {
                var controller = new EstadosController(context);

                var result = await controller.DeleteEstado(estado.EstadoId);

                var okResult = Assert.IsType<OkObjectResult>(result);
                var estadoResult = Assert.IsType<Estado>(okResult.Value);
            }

            using (var context = new CompeticoesDbContext(options))
            {
                Assert.True(!(await context.Estados
                    .AnyAsync(a => a.EstadoId == estado.EstadoId)));
            }
        }
    }
}
