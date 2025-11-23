using System.Text.Json.Serialization;

namespace PresTechBackEnd.Models
{
    public class Prestatario
    {
        public int PrestatarioId { get; set; }

        // Clave foránea hacia Persona
        public int PersonaId { get; set; }

        // Relación 1–1
        public Persona Persona { get; set; } = null!;

        // Relación inversa hacia Prestamos
        [JsonIgnore]
        public ICollection<Prestamo>? Prestamos { get; set; }
    }
}
