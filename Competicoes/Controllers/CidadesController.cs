using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Competicoes.Data;
using Competicoes.Models;

namespace Competicoes.Controllers
{
    [Produces("application/json")]
    [Route("api/Cidades")]
    public class CidadesController : Controller
    {
        private readonly CompeticoesDbContext _context;

        public CidadesController(CompeticoesDbContext context)
        {
            _context = context;
        }

        // GET: api/Cidades
        [HttpGet]
        public IEnumerable<Cidade> GetCidades()
        {
            return _context.Cidades;
        }

        // GET: api/Cidades/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCidade([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cidade = await _context.Cidades.SingleOrDefaultAsync(m => m.CidadeId == id);

            if (cidade == null)
            {
                return NotFound();
            }

            return Ok(cidade);
        }

        // PUT: api/Cidades/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCidade([FromRoute] Guid id, [FromBody] Cidade cidade)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cidade.CidadeId)
            {
                return BadRequest();
            }

            _context.Entry(cidade).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CidadeExists(id))
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

        // POST: api/Cidades
        [HttpPost]
        public async Task<IActionResult> PostCidade([FromBody] Cidade cidade)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Cidades.Add(cidade);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCidade", new { id = cidade.CidadeId }, cidade);
        }

        // DELETE: api/Cidades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCidade([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cidade = await _context.Cidades.SingleOrDefaultAsync(m => m.CidadeId == id);
            if (cidade == null)
            {
                return NotFound();
            }

            _context.Cidades.Remove(cidade);
            await _context.SaveChangesAsync();

            return Ok(cidade);
        }

        private bool CidadeExists(Guid id)
        {
            return _context.Cidades.Any(e => e.CidadeId == id);
        }
    }
}