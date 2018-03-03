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
    [Route("api/Paises")]
    public class PaisesController : Controller
    {
        private readonly CompeticoesDbContext _context;

        public PaisesController(CompeticoesDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Pais> GetPaises()
        {
            return _context.Paises;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPais([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pais = await _context.Paises.FindAsync(id);

            if (pais == null)
            {
                return NotFound();
            }

            return Ok(pais);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPais([FromRoute]Guid id, [FromBody] Pais pais)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pais.PaisId)
            {
                return BadRequest();
            }

            _context.Entry(pais).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PaisExists(id))
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

        [HttpPost]
        public async Task<IActionResult> PostPais([FromBody] Pais pais)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.Paises.AddAsync(pais);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPais", new { id = pais.PaisId }, pais);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePais([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pais = await _context.Paises.FindAsync(id);

            if (pais == null)
            {
                return NotFound();
            }

            _context.Paises.Remove(pais);
            await _context.SaveChangesAsync();
            

            return Ok(pais);            
        }


        private async Task<bool> PaisExists(Guid id) => await _context.Paises.AnyAsync(a => a.PaisId == id);
        
    }
}