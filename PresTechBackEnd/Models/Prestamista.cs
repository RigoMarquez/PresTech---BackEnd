namespace PresTechBackEnd.Models
{
    public class Prestamista
    {
        public int PrestamistaId { get; set; }

        // FK hacia Persona
        public int PersonaId { get; set; }

        // Relación 1–1 obligatoria
        public Persona Persona { get; set; } = null!;
    }
}
