using System.Text.Json.Serialization;

namespace PresTechBackEnd.Models
{
    public class OfertaPrestamo
    {
        public int OfertaPrestamoId { get; set; }

        public required string Categoria { get; set; }
        public required decimal MontoMin { get; set; }
        public required decimal MontoMax { get; set; }
        public required decimal Interes { get; set; }
        public required int Cuotas { get; set; }
        public required string Frecuencia { get; set; }
        public string? Descripcion { get; set; }
        public bool Disponible { get; set; } = true;    
        public int PrestamistaId { get; set; }

        [JsonIgnore]
        public Prestamista? Prestamista { get; set; }

        public ICollection<Prestamo>? Prestamos { get; set; }
    }
}

