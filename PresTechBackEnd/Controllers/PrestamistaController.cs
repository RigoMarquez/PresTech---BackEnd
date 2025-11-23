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
    public class PrestamistaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PrestamistaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Prestamista
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prestamista>>> GetPrestamistas()
        {
            return await _context.Prestamistas.ToListAsync();
        }

        // GET: api/Prestamista/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Prestamista>> GetPrestamista(int id)
        {
            var prestamista = await _context.Prestamistas.FindAsync(id);

            if (prestamista == null)
            {
                return NotFound();
            }

            return prestamista;
        }

        // PUT: api/Prestamista/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrestamista(int id, Prestamista prestamista)
        {
            if (id != prestamista.PrestamistaId)
            {
                return BadRequest();
            }

            _context.Entry(prestamista).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrestamistaExists(id))
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

        // POST: api/Prestamista
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Prestamista>> PostPrestamista(Prestamista prestamista)
        {
            _context.Prestamistas.Add(prestamista);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrestamista", new { id = prestamista.PrestamistaId }, prestamista);
        }

        // DELETE: api/Prestamista/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrestamista(int id)
        {
            var prestamista = await _context.Prestamistas.FindAsync(id);
            if (prestamista == null)
            {
                return NotFound();
            }

            _context.Prestamistas.Remove(prestamista);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrestamistaExists(int id)
        {
            return _context.Prestamistas.Any(e => e.PrestamistaId == id);
        }

        [HttpGet("{prestamistaId}/Clientes")]
        public async Task<IActionResult> GetClientesDePrestamista(int prestamistaId)
        {
            var clientes = await _context.Prestamos
                .Where(p => p.OfertaPrestamo.PrestamistaId == prestamistaId)
                .Include(p => p.Prestatario)
                    .ThenInclude(pr => pr.Persona)
                .GroupBy(p => new
                {
                    p.Prestatario.PrestatarioId,
                    p.Prestatario.Persona.Nombre,
                    p.Prestatario.Persona.Email,
                    p.Prestatario.Persona.Telefono,
                    p.Prestatario.Persona.Identificacion,
                    p.Prestatario.Persona.Sexo,
                    p.Prestatario.Persona.Ciudad
                })
                .Select(g => new
                {
                    PrestatarioId = g.Key.PrestatarioId,
                    Nombre = g.Key.Nombre,
                    Email = g.Key.Email,
                    Telefono = g.Key.Telefono,


                    CantidadPrestamosActivos = g.Count(p => p.Estado == "Activo"),


                    TotalSaldoRestante = g.Sum(p => p.SaldoRestante),

                    Prestamos = g.Select(p => new
                    {
                        PrestamoId = p.PrestamoId,
                        SaldoPrestamo = p.SaldoPrestamo,
                        SaldoRestante = p.SaldoRestante,
                        Estado = p.Estado,
                        FechaInicio = p.FechaInicio
                    })
                    .OrderByDescending(p => p.FechaInicio)
                    .ToList()
                })
                .ToListAsync();

            return Ok(clientes);
        }

        [HttpGet("DetalleCliente/{prestatarioId}/{prestamistaId}")]
        public async Task<IActionResult> GetDetalleClienteSimple(int prestatarioId, int prestamistaId)
        {
            // Filtramos los préstamos del prestatario que pertenecen a este prestamista
            var prestamos = await _context.Prestamos
                .Where(p => p.PrestatarioId == prestatarioId && p.OfertaPrestamo.PrestamistaId == prestamistaId)
                .Include(p => p.OfertaPrestamo)
                .Include(p => p.Prestatario.Persona)
                .OrderByDescending(p => p.PrestamoId)
                .ToListAsync();

            if (!prestamos.Any())
                return NotFound(new { mensaje = "No hay préstamos de este prestamista para este prestatario." });

            var persona = prestamos.First().Prestatario.Persona;

            return Ok(new
            {
                PrestatarioId = prestamos.First().PrestatarioId,
                Nombre = persona.Nombre,
                Email = persona.Email,
                Telefono = persona.Telefono,
                Identificacion = persona.Identificacion,
                Sexo = persona.Sexo,
                Ciudad = persona.Ciudad,

                CantidadPrestamosActivos = prestamos.Count(p => p.Estado == "Activo"),
                TotalSaldo = prestamos.Sum(p => p.SaldoPrestamo),
                TotalSaldoRestante = prestamos.Sum(p => p.SaldoRestante),

                Prestamos = prestamos.Select(p => new
                {
                    p.PrestamoId,
                    p.OfertaPrestamoId,
                    p.SaldoPrestamo,
                    p.SaldoRestante,
                    p.Estado,
                    p.FechaInicio,
                    p.FechaProxPago,
                    Categoria = p.OfertaPrestamo.Categoria,
                    PrestamistaId = p.OfertaPrestamo.PrestamistaId
                })
            });
            }

        [HttpGet("{prestamistaId}/dashboard")]
        public async Task<ActionResult<DashboardPrestamistaDTO>> ObtenerDashboardPrestamista(int prestamistaId)
        {
            // Total de préstamos activos (monto original del préstamo)
            var totalPrestamosActivos = await _context.Prestamos
                .Where(p =>
                    p.OfertaPrestamo.PrestamistaId == prestamistaId 
                )
                .SumAsync(p => p.SaldoPrestamo);

            var inicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var finMes = inicioMes.AddMonths(1);

            var pagosRecibidosMes = await _context.Transacciones
                .Where(t =>
                    t.Prestamo.OfertaPrestamo.PrestamistaId == prestamistaId &&
                    t.FechaPago >= inicioMes &&
                    t.FechaPago < finMes
                )
                .SumAsync(t => t.Monto);

            var pagosRecibidos = await _context.Transacciones
                .Where(t =>
                    t.Prestamo.OfertaPrestamo.PrestamistaId == prestamistaId
                )
                .SumAsync(t => t.Monto);

            // Clientes activos (únicos)
            var clientesActivos = await _context.Prestamos
                .Where(p =>
                    p.OfertaPrestamo.PrestamistaId == prestamistaId &&
                    p.Estado == "Activo"
                )
                .Select(p => p.PrestatarioId)
                .Distinct()
                .CountAsync();

            // Cantidad de ofertas creadas por el prestamista
            var ofertasCreadas = await _context.OfertasPrestamo
                .Where(o => o.PrestamistaId == prestamistaId)
                .CountAsync();

            // Últimos 5 préstamos del prestamista
            var prestamosRecientes = await _context.Prestamos
                .Include(p => p.Prestatario)
                    .ThenInclude(pr => pr.Persona)
                .Where(p => p.OfertaPrestamo.PrestamistaId == prestamistaId)
                .OrderByDescending(p => p.FechaInicio)
                .Take(5)
                .Select(p => new PrestamoRecienteDTO
                {
                    PrestamoId = p.PrestamoId,
                    OfertaPrestamoId = p.OfertaPrestamoId,
                    Categoria = p.OfertaPrestamo.Categoria,
                    FechaInicio = p.FechaInicio,
                    Monto = p.SaldoPrestamo
                })
                .ToListAsync();

            var dashboard = new DashboardPrestamistaDTO
            {
                TotalPrestamosActivos = totalPrestamosActivos,
                PagosRecibidosMes = pagosRecibidosMes,
                PagosRecibidos = pagosRecibidos,
                ClientesActivos = clientesActivos,
                OfertasCreadas = ofertasCreadas,
                PrestamosRecientes = prestamosRecientes
            };

            return Ok(dashboard);
        }
    }
}
