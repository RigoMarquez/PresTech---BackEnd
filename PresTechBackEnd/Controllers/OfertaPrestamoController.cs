using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresTech.Data;
using PresTechBackEnd.Models;

namespace PresTechBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfertaPrestamoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OfertaPrestamoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/OfertaPrestamo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OfertaPrestamo>>> GetOfertasPrestamo()
        {
            return await _context.OfertasPrestamo
                .Where(o => o.Disponible == true)
                .ToListAsync();
        }


        // GET: api/OfertaPrestamo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OfertaPrestamo>> GetOfertaPrestamo(int id)
        {
            var ofertaPrestamo = await _context.OfertasPrestamo.FindAsync(id);

            if (ofertaPrestamo == null)
            {
                return NotFound();
            }

            return ofertaPrestamo;
        }

        // PUT: api/OfertaPrestamo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOfertaPrestamo(int id, OfertaPrestamo ofertaPrestamo)
        {
            if (id != ofertaPrestamo.OfertaPrestamoId)
            {
                return BadRequest();
            }

            _context.Entry(ofertaPrestamo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfertaPrestamoExists(id))
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

        // POST: api/OfertaPrestamo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OfertaPrestamo>> PostOfertaPrestamo(OfertaPrestamo ofertaPrestamo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.OfertasPrestamo.Add(ofertaPrestamo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOfertaPrestamo", new { id = ofertaPrestamo.OfertaPrestamoId }, ofertaPrestamo);
        }
        // DELETE: api/OfertaPrestamo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOfertaPrestamo(int id)
        {
            var ofertaPrestamo = await _context.OfertasPrestamo.FindAsync(id);
            if (ofertaPrestamo == null)
            {
                return NotFound();
            }

            _context.OfertasPrestamo.Remove(ofertaPrestamo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OfertaPrestamoExists(int id)
        {
            return _context.OfertasPrestamo.Any(e => e.OfertaPrestamoId == id);
        }

        [HttpGet("prestamista/{id}")]
        public async Task<ActionResult<IEnumerable<OfertaPrestamo>>> GetOfertasPorPrestamista(int id)
        {
            var ofertas = await _context.OfertasPrestamo
                .Where(o => o.PrestamistaId == id)
                .ToListAsync();

            return ofertas;
        }

    }
}
