using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresTech.Data;
using System.Security.Cryptography;
using System.Text;

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

        // ---- FUNCIÓN HASH USADA TAMBIÉN EN REGISTRO ----
        private static string Hash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest req)
        {
            var persona = await _context.Personas
                .FirstOrDefaultAsync(p => p.Email == req.Email);

            if (persona == null)
                return Unauthorized(new { message = "Credenciales no validas" });

            // Hash de la contraseña enviada desde el login
            string hashedInput = Hash(req.Contraseña);

            if (persona.Contraseña != hashedInput)
                return Unauthorized(new { message = "Credenciales no validas" });

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
