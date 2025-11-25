using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresTech.Data;
using PresTechBackEnd.Models;

namespace PresTechBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RegistroController(ApplicationDbContext context)
        {
            _context = context;
        }

        public class RegistroRequest
        {
            public required string Nombre { get; set; }
            public int TipoDocumentoID { get; set; }
            public required string Identificacion { get; set; }
            public required string Email { get; set; }
            public string? Sexo { get; set; }
            public string? Telefono { get; set; }
            public string? Direccion { get; set; }
            public string? Ciudad { get; set; }
            public required string Contraseña { get; set; }

            public required string Rol { get; set; }  
        }

        [HttpPost]
        public async Task<ActionResult> RegistrarUsuario([FromBody] RegistroRequest req)
        {
            var emailExiste = await _context.Personas
                .AnyAsync(p => p.Email.ToLower() == req.Email.ToLower());

            if (emailExiste)
                return BadRequest(new { mensaje = "El email ya está registrado." });

            var documentoExiste = await _context.Personas
                .AnyAsync(p => p.TipoDocumentoID == req.TipoDocumentoID &&
                               p.Identificacion == req.Identificacion);

            if (documentoExiste)
                return BadRequest(new { mensaje = "El número de documento ya está registrado para ese tipo de documento." });

            string hashPassword = Hash(req.Contraseña);

            var persona = new Persona
            {
                Nombre = req.Nombre,
                TipoDocumentoID = req.TipoDocumentoID,
                Identificacion = req.Identificacion,
                Email = req.Email,
                Sexo = req.Sexo,
                Telefono = req.Telefono,
                Direccion = req.Direccion,
                Ciudad = req.Ciudad,
                Contraseña = hashPassword
            };

            _context.Personas.Add(persona);
            await _context.SaveChangesAsync();

            // Crear rol
            if (req.Rol.ToLower() == "prestamista")
            {
                var prestamista = new Prestamista { PersonaId = persona.PersonaId };

                _context.Prestamistas.Add(prestamista);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Prestamista registrado", persona, prestamista });
            }

            if (req.Rol.ToLower() == "prestatario")
            {
                var prestatario = new Prestatario { PersonaId = persona.PersonaId };

                _context.Prestatarios.Add(prestatario);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Prestatario registrado", persona, prestatario });
            }

            return BadRequest("Rol inválido. Debe ser 'prestamista' o 'prestatario'.");
        }


        private static string Hash(string input)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

    }
}
