using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Competicoes.Data;
using Competicoes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Competicoes.Controllers
{
    [Produces("application/json")]
    [Route("api/Estados")]
    public class EstadosController : Controller
    {
        private readonly CompeticoesDbContext _context;

        public EstadosController(CompeticoesDbContext context)
        {
            _context = context;
        }

        // GET: api/Estados
        [HttpGet]
        public IEnumerable<Estado> GetEstados()
        {
            return _context.Estados;
        }

        // GET: api/Estados/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEstado([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var estado = await _context.Estados.FindAsync(id);

            if (estado == null)
            {
                return NotFound();
            }

            return Ok(estado);
        }

        // PUT: api/Estados/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstado([FromRoute] Guid id, 
            [FromBody] Estado estado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != estado.EstadoId)
            {
                return BadRequest();
            }

            _context.Entry(estado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Estados
        [HttpPost]
        public async Task<IActionResult> PostEstado([FromBody] Estado estado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Estados.Add(estado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstado", new { id = estado.EstadoId }, estado);
        }

        // DELETE: api/Estados/5
        [HttpDelete]
        public async Task<IActionResult> DeleteEstado([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var estado = await _context.Estados.FindAsync(id);
            if (estado == null)
            {
                return NotFound();
            }

            _context.Estados.Remove(estado);
            await _context.SaveChangesAsync();

            return Ok(estado);
        }

        private bool EstadoExists(Guid id) 
            => _context.Estados.Any(e => e.EstadoId == id);
    }
}