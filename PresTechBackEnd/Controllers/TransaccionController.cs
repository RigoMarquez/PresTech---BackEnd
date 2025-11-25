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
    public class TransaccionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TransaccionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Transaccion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaccion>>> GetTransacciones()
        {
            return await _context.Transacciones.ToListAsync();
        }

        // GET: api/Transaccion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaccion>> GetTransaccion(int id)
        {
            var transaccion = await _context.Transacciones.FindAsync(id);

            if (transaccion == null)
            {
                return NotFound();
            }

            return transaccion;
        }

        // PUT: api/Transaccion/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaccion(int id, Transaccion transaccion)
        {
            if (id != transaccion.TransaccionId)
            {
                return BadRequest();
            }

            _context.Entry(transaccion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransaccionExists(id))
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

        // POST: api/Transaccion
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Transaccion>> PostTransaccion(Transaccion transaccion)
        {
            _context.Transacciones.Add(transaccion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransaccion", new { id = transaccion.TransaccionId }, transaccion);
        }

        // DELETE: api/Transaccion/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaccion(int id)
        {
            var transaccion = await _context.Transacciones.FindAsync(id);
            if (transaccion == null)
            {
                return NotFound();
            }

            _context.Transacciones.Remove(transaccion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransaccionExists(int id)
        {
            return _context.Transacciones.Any(e => e.TransaccionId == id);
        }

        private DateTime CalcularProximaFechaPago(DateTime fechaActual, string frecuencia)
        {
            switch (frecuencia.ToLower())
            {
                case "semanal":
                    return fechaActual.AddDays(7);

                case "quincenal":
                    return fechaActual.AddDays(15);

                case "mensual":
                    return fechaActual.AddMonths(1);

                case "anual":
                    return fechaActual.AddYears(1);

                default:
                    return fechaActual;
            }
        }

        [HttpPost("registrar-pago")]
        public async Task<IActionResult> RegistrarPago([FromBody] TransaccionDTO dto)
        {
            if (dto == null) return BadRequest("Payload vacío.");

            if (dto.MontoPagado <= 0) return BadRequest("El monto debe ser mayor a 0.");

            var prestamo = await _context.Prestamos
                .Include(p => p.OfertaPrestamo)
                .FirstOrDefaultAsync(p => p.PrestamoId == dto.PrestamoId);

            if (prestamo == null)
                return NotFound(new { mensaje = "Préstamo no encontrado" });

            if (prestamo.OfertaPrestamo == null || string.IsNullOrWhiteSpace(prestamo.OfertaPrestamo.Frecuencia))
                return BadRequest(new { mensaje = "No se encontró la frecuencia del préstamo (OfertaPrestamo)." });

            // Registrar la transacción
            var transaccion = new Transaccion
            {
                PrestamoId = dto.PrestamoId,
                Monto = dto.MontoPagado,
                FechaPago = DateTime.Now,
                TipoTransaccion = dto.TipoPago ?? string.Empty
            };

            _context.Transacciones.Add(transaccion);

            prestamo.SaldoRestante -= dto.MontoPagado;
            if (prestamo.SaldoRestante < 0) prestamo.SaldoRestante = 0;

            if (prestamo.CuotasRestantes > 0)
                prestamo.CuotasRestantes--;

            prestamo.FechaProxPago = CalcularProximaFechaPago(prestamo.FechaProxPago, prestamo.OfertaPrestamo.Frecuencia);

            // Si ya queda en 0 → marcar como pagado
            if (prestamo.SaldoRestante <= 0 || prestamo.CuotasRestantes <= 0)
            {
                prestamo.SaldoRestante = 0;
                prestamo.CuotasRestantes = 0;
                prestamo.Estado = "Pagado";
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Pago registrado con éxito",
                prestamoId = prestamo.PrestamoId,
                nuevoSaldoRestante = prestamo.SaldoRestante,
                cuotasRestantes = prestamo.CuotasRestantes,
                fechaProxPago = prestamo.FechaProxPago,
                estado = prestamo.Estado
            });
        }

        [HttpGet("por-prestatario/{prestatarioId}")]
        public async Task<ActionResult<IEnumerable<TransaccionDTO>>> GetTransaccionesPorPrestatario(int prestatarioId)
        {
            try
            {
                var transacciones = await _context.Transacciones
                    .Where(t => t.Prestamo != null && t.Prestamo.PrestatarioId == prestatarioId)
                    .Select(t => new TransaccionDTO
                    {
                        TransaccionId = t.TransaccionId,
                        FechaPago = t.FechaPago,
                        MontoPagado = t.Monto,
                        TipoPago = t.TipoTransaccion,
                        PrestamoId = t.PrestamoId
                    })
                    .OrderByDescending(t => t.FechaPago)
                    .ToListAsync();

                return Ok(transacciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("resumen-prestatario/{prestatarioId}")]
        public async Task<IActionResult> ObtenerResumenPrestatario(int prestatarioId)
        {
            // Obtener transacciones del prestatario
            var transacciones = await _context.Transacciones
                .Include(t => t.Prestamo)
                .Where(t => t.Prestamo.PrestatarioId == prestatarioId)
                .ToListAsync();

            // Obtener préstamos activos del prestatario
            var prestamos = await _context.Prestamos
                .Where(p => p.PrestatarioId == prestatarioId)
                .ToListAsync();

            if (!prestamos.Any())
                return NotFound(new { mensaje = "No se encontraron préstamos para este prestatario" });

            var resumen = new ResumenPrestamoDTO
            {
                TotalPagado = transacciones.Sum(t => t.Monto),
                NumeroPagos = transacciones.Count,
                SaldoRestante = prestamos.Sum(p => p.SaldoRestante)
            };

            return Ok(resumen);
        }

        [HttpGet("/prestamista-transacciones/{prestamistaId}")]
        public async Task<ActionResult<List<TransaccionPrestamistaDTO>>> ObtenerTransaccionesPrestamista(int prestamistaId)
        {
            var transacciones = await _context.Transacciones
            .Where(t => t.Prestamo.OfertaPrestamo.PrestamistaId == prestamistaId)
            .Select(t => new TransaccionPrestamistaDTO
            {
                TransaccionId = t.TransaccionId,
                PrestamoId = t.PrestamoId,
                OfertaPrestamoId = t.Prestamo.OfertaPrestamoId,
                Categoria = t.Prestamo.OfertaPrestamo.Categoria,    
                ClienteNombre = t.Prestamo.Prestatario.Persona.Nombre,
                ClienteDocumento = t.Prestamo.Prestatario.Persona.Identificacion,
                ClienteTelefono = t.Prestamo.Prestatario.Persona.Telefono,
                ClienteEmail = t.Prestamo.Prestatario.Persona.Email,
                Monto = t.Monto,
                FechaPago = t.FechaPago,
                TipoTransaccion = t.TipoTransaccion,
                SaldoRestante = t.Prestamo.SaldoRestante,
            })
            .OrderByDescending(t => t.FechaPago)
            .ToListAsync();

            return Ok(transacciones);
        }


    }
}
