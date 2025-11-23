using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresTech.Data;
using PresTechBackEnd.DTO;
using PresTechBackEnd.Models;

namespace PresTechBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestatarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PrestatarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Prestatario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prestatario>>> GetPrestatarios()
        {
            return await _context.Prestatarios.ToListAsync();
        }

        // GET: api/Prestatario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Prestatario>> GetPrestatario(int id)
        {
            var prestatario = await _context.Prestatarios.FindAsync(id);

            if (prestatario == null)
            {
                return NotFound();
            }

            return prestatario;
        }

        // PUT: api/Prestatario/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrestatario(int id, Prestatario prestatario)
        {
            if (id != prestatario.PrestatarioId)
            {
                return BadRequest();
            }

            _context.Entry(prestatario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrestatarioExists(id))
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

        // POST: api/Prestatario
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Prestatario>> PostPrestatario(Prestatario prestatario)
        {
            _context.Prestatarios.Add(prestatario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrestatario", new { id = prestatario.PrestatarioId }, prestatario);
        }

        // DELETE: api/Prestatario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrestatario(int id)
        {
            var prestatario = await _context.Prestatarios.FindAsync(id);
            if (prestatario == null)
            {
                return NotFound();
            }

            _context.Prestatarios.Remove(prestatario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrestatarioExists(int id)
        {
            return _context.Prestatarios.Any(e => e.PrestatarioId == id);
        }

        [HttpGet("{prestatarioId}/dashboard")]
        public async Task<ActionResult<DashboardPrestatarioDTO>> ObtenerDashboardPrestatario(int prestatarioId)
        {
            var deudaTotal = await _context.Prestamos
                .Where(p => p.PrestatarioId == prestatarioId)
                .SumAsync(p => p.SaldoRestante);

            var totalPagado = await _context.Transacciones
                .Where(t => t.Prestamo.PrestatarioId == prestatarioId)
                .SumAsync(t => t.Monto);

            var proximoPago = await _context.Prestamos
                .Where(p => p.PrestatarioId == prestatarioId &&
                       p.Estado == "Activo")
                .OrderBy(p => p.FechaProxPago)
                .Select(p => p.FechaProxPago)
                .FirstOrDefaultAsync();

            var pagosVencidos = await _context.Prestamos
                .Where(p =>
                    p.PrestatarioId == prestatarioId &&
                    p.SaldoRestante == 0
                )
                .CountAsync();

            var ultimosPrestamos = await _context.Prestamos
                .Where(p => p.PrestatarioId == prestatarioId)
                .OrderByDescending(p => p.FechaInicio)
                .Take(5)
                .Select(p => new PrestamoRecienteDTO
                {
                    PrestamoId = p.PrestamoId,
                    OfertaPrestamoId = p.OfertaPrestamoId,
                    Monto = p.SaldoRestante,
                    Categoria = p.OfertaPrestamo.Categoria,
                    FechaPago = p.FechaProxPago,
                    Estado = p.Estado
                })
                .ToListAsync();

            var dashboard = new DashboardPrestatarioDTO
            {
                DeudaTotal = deudaTotal,
                TotalPagado = totalPagado,
                ProximoPago = proximoPago,
                PagosVencidos = pagosVencidos,
                UltimosPrestamos = ultimosPrestamos
            };

            return Ok(dashboard);
        }

    }

}
