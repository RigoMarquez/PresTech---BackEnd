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
    public class PrestamoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PrestamoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Prestamo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prestamo>>> GetPrestamos()
        {
            return await _context.Prestamos.ToListAsync();
        }

        [HttpGet("{prestamoId}/detalle-prestamista")]
        public async Task<ActionResult<PrestamoPrestamistaDTO>> ObtenerDetallePrestamoPrestamista(int prestamoId)
        {
            var prestamo = await _context.Prestamos
                .Include(p => p.OfertaPrestamo)
                .Include(p => p.Prestatario)
                .Include(p => p.Transacciones)
                .Where(p => p.PrestamoId == prestamoId)
                .Select(p => new PrestamoPrestamistaDTO
                {
                    PrestamoId = p.PrestamoId,
                    OfertaPrestamoId = p.OfertaPrestamoId,
                    CategoriaPrestamo = p.OfertaPrestamo.Categoria,
                    Tasas = p.OfertaPrestamo.Interes,
                    SaldoPrestamo = p.SaldoPrestamo,
                    SaldoRestante = p.SaldoRestante,
                    CuotasTotales = p.OfertaPrestamo.Cuotas,
                    CuotasRestantes = p.CuotasRestantes,
                    Frecuencia = p.OfertaPrestamo.Frecuencia,
                    FechaProxPago = p.FechaProxPago,
                    Estado = p.Estado,
                    PrestatarioId = p.PrestatarioId,
                    NombrePrestatario = p.Prestatario.Persona.Nombre,
                    EmailPrestatario = p.Prestatario.Persona.Email,
                    TelefonoPrestatario = p.Prestatario.Persona.Telefono,

                    // Solo la última transacción
                    FechaPago = p.Transacciones
                        .OrderByDescending(t => t.FechaPago)
                        .Select(t => t.FechaPago)
                        .FirstOrDefault(),
                    MontoPagado = p.Transacciones
                        .OrderByDescending(t => t.FechaPago)
                        .Select(t => t.Monto)
                        .FirstOrDefault(),
                    TipoPago = p.Transacciones
                        .OrderByDescending(t => t.FechaPago)
                        .Select(t => t.TipoTransaccion)
                        .FirstOrDefault() ?? ""
                })
                .FirstOrDefaultAsync();

            if (prestamo == null)
            {
                return NotFound(new { message = "Préstamo no encontrado" });
            }

            return Ok(prestamo);
        }

        [HttpGet("{prestamoId}/detalle-prestatario")]
        public async Task<ActionResult<PrestamoPrestatarioDTO>> ObtenerDetallePrestamoParaPrestatario(int prestamoId)
        {
            var prestamo = await _context.Prestamos
                .Include(p => p.OfertaPrestamo)
                    .ThenInclude(o => o.Prestamista)
                        .ThenInclude(pr => pr.Persona)
                .Include(p => p.Transacciones)
                .Where(p => p.PrestamoId == prestamoId)
                .Select(p => new PrestamoPrestatarioDTO
                {
                    PrestamoId = p.PrestamoId,
                    OfertaPrestamoId = p.OfertaPrestamoId,
                    CategoriaPrestamo = p.OfertaPrestamo.Categoria,
                    Tasas = p.OfertaPrestamo.Interes,
                    SaldoPrestamo = p.SaldoPrestamo,
                    SaldoRestante = p.SaldoRestante,
                    CuotasTotales = p.OfertaPrestamo.Cuotas,
                    CuotasRestantes = p.CuotasRestantes,
                    Frecuencia = p.OfertaPrestamo.Frecuencia,
                    FechaProxPago = p.FechaProxPago,
                    Estado = p.Estado,

                    PrestamistaId = p.OfertaPrestamo.PrestamistaId,
                    NombrePrestamista = p.OfertaPrestamo.Prestamista.Persona.Nombre,
                    EmailPrestamista = p.OfertaPrestamo.Prestamista.Persona.Email,
                    TelefonoPrestamista = p.OfertaPrestamo.Prestamista.Persona.Telefono,

                    // Solo la última transacción
                    FechaPago = p.Transacciones
                        .OrderByDescending(t => t.FechaPago)
                        .Select(t => t.FechaPago)
                        .FirstOrDefault(),
                    MontoPagado = p.Transacciones
                        .OrderByDescending(t => t.FechaPago)
                        .Select(t => t.Monto)
                        .FirstOrDefault(),
                    TipoPago = p.Transacciones
                        .OrderByDescending(t => t.FechaPago)
                        .Select(t => t.TipoTransaccion)
                        .FirstOrDefault() ?? ""
                })
                .FirstOrDefaultAsync();

            if (prestamo == null)
            {
                return NotFound(new { message = "Préstamo no encontrado" });
            }

            return Ok(prestamo);
        }




        // GET: api/Prestamo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Prestamo>> GetPrestamo(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);

            if (prestamo == null)
            {
                return NotFound();
            }

            return prestamo;
        }

        // PUT: api/Prestamo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrestamo(int id, Prestamo prestamo)
        {
            if (id != prestamo.PrestamoId)
            {
                return BadRequest();
            }

            _context.Entry(prestamo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrestamoExists(id))
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

        // DELETE: api/Prestamo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrestamo(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }

            _context.Prestamos.Remove(prestamo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrestamoExists(int id)
        {
            return _context.Prestamos.Any(e => e.PrestamoId == id);
        }

        [HttpPost]
        public async Task<ActionResult<SolicitarPrestamoDTO>> CrearPrestamo(Prestamo prestamo)
        {
            var prestatario = await _context.Prestatarios.FindAsync(prestamo.PrestatarioId);
            if (prestatario == null)
                return BadRequest("Prestatario no existe.");

            var oferta = await _context.OfertasPrestamo.FindAsync(prestamo.OfertaPrestamoId);
            if (oferta == null)
                return BadRequest("Oferta de préstamo no existe.");

            if (!oferta.Disponible)
                return BadRequest("La oferta ya fue utilizada.");

            DateTime fechaProximoPago = DateTime.Now;

            switch (oferta.Frecuencia.ToLower())
            {
                case "semanal":
                    fechaProximoPago = fechaProximoPago.AddDays(7);
                    break;

                case "quincenal":
                    fechaProximoPago = fechaProximoPago.AddDays(15);
                    break;

                case "mensual":
                    fechaProximoPago = fechaProximoPago.AddMonths(1);
                    break;

                case "anual":
                    fechaProximoPago = fechaProximoPago.AddMonths(12);
                    break;

                default:
                    fechaProximoPago = fechaProximoPago.AddMonths(1);
                    break;
            }

            prestamo.FechaInicio = DateTime.Now;
            prestamo.FechaProxPago = fechaProximoPago;
            prestamo.Estado = "Activo";
            prestamo.SaldoRestante = Convert.ToDecimal(prestamo.SaldoPrestamo);
            prestamo.CuotasRestantes = oferta.Cuotas;

            oferta.Disponible = false;

            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();

            var dto = new SolicitarPrestamoDTO
            {
                PrestamoId = prestamo.PrestamoId,
                PrestatarioId = prestamo.PrestatarioId,
                OfertaPrestamoId = prestamo.OfertaPrestamoId,
                SaldoPrestamo = prestamo.SaldoPrestamo.ToString(),
                SaldoRestante = prestamo.SaldoRestante,
                CuotasRestantes = prestamo.CuotasRestantes.ToString(),
                FechaInicio = prestamo.FechaInicio,
                FechaProxPago = prestamo.FechaProxPago,
                Estado = prestamo.Estado
            };

            return Ok(dto);
        }


        [HttpGet("por-prestamista/{prestamistaId}")]
        public async Task<ActionResult<IEnumerable<PrestamoPrestamistaDTO>>> ObtenerPrestamos(int prestamistaId)
        {
            var prestamos = await _context.Prestamos
                .Where(p => p.OfertaPrestamo.PrestamistaId == prestamistaId)
                .Select(p => new PrestamoPrestamistaDTO
                {
                    PrestamoId = p.PrestamoId,
                    OfertaPrestamoId = p.OfertaPrestamoId,
                    CategoriaPrestamo = p.OfertaPrestamo.Categoria,
                    Tasas = p.OfertaPrestamo.Interes,
                    SaldoPrestamo = p.SaldoPrestamo,
                    SaldoRestante = p.SaldoRestante,
                    CuotasTotales = p.OfertaPrestamo.Cuotas,
                    CuotasRestantes = p.CuotasRestantes,
                    Frecuencia = p.OfertaPrestamo.Frecuencia,
                    FechaProxPago = p.FechaProxPago,
                    Estado = p.Estado,
                })
                .ToListAsync();

            return Ok(prestamos);
        }

        [HttpGet("por-prestatario/{prestatarioId}")]
        public async Task<ActionResult<IEnumerable<PrestamoPrestatarioDTO>>> ObtenerPrestamosPorPrestatario(int prestatarioId)
        {
            var prestamos = await _context.Prestamos
                .Where(p => p.PrestatarioId == prestatarioId)
                .Select(p => new PrestamoPrestatarioDTO
                {
                    PrestamoId = p.PrestamoId,
                    OfertaPrestamoId = p.OfertaPrestamoId,
                    CategoriaPrestamo = p.OfertaPrestamo.Categoria,
                    SaldoPrestamo = p.SaldoPrestamo,
                    SaldoRestante = p.SaldoRestante,
                    Tasas = p.OfertaPrestamo.Interes,
                    CuotasTotales = p.OfertaPrestamo.Cuotas,
                    CuotasRestantes = p.CuotasRestantes,
                    Frecuencia = p.OfertaPrestamo.Frecuencia,
                    FechaProxPago = p.FechaProxPago,
                    Estado = p.Estado
                })
                .ToListAsync();

            return Ok(prestamos);
        }
    }
}
