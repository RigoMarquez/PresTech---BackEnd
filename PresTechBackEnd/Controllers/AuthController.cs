using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresTech.Data;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        public class LoginRequest
        {
            public required string Email { get; set; }
            public required string Contraseña { get; set; }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest req)
        {
            var persona = await _context.Personas
                .FirstOrDefaultAsync(p => p.Email == req.Email);

            if (persona == null)
                return Unauthorized(new { message = "Email no registrado" });

            if (persona.Contraseña != req.Contraseña)
                return Unauthorized(new { message = "Contraseña incorrecta" });

            // Buscar relación según el rol
            var prestamista = await _context.Prestamistas
                .FirstOrDefaultAsync(p => p.PersonaId == persona.PersonaId);

            var prestatario = await _context.Prestatarios
                .FirstOrDefaultAsync(p => p.PersonaId == persona.PersonaId);

            string rol = prestamista != null ? "prestamista"
                     : prestatario != null ? "prestatario"
                     : "desconocido";

            return Ok(new
            {
                message = "Login exitoso",
                persona = new
                {
                    persona.PersonaId,
                    persona.Nombre,
                    persona.Email,
                    rol
                },
                prestamistaId = prestamista?.PrestamistaId,
                prestatarioId = prestatario?.PrestatarioId   
            });
        }

    }
}
