namespace PresTechBackEnd.Models
{
    public class Persona
    {
        public int PersonaId { get; set; }

        public required string Nombre { get; set; }

        // FK
        public int TipoDocumentoID { get; set; }
        public TipoDocumento TipoDocumento { get; set; } = null!;

        public required string Identificacion { get; set; }
        public required string Email { get; set; }

        public string? Sexo { get; set; }
        public string? Telefono { get; set; }

        public string? Direccion { get; set; }

        public string? Ciudad { get; set; }

        public required string Contraseña { get; set; }
    }
}
