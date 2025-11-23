namespace PresTechBackEnd.Models
{
    public class Prestamo
    {
        public int PrestamoId { get; set; }

        // FK hacia Prestatario
        public int PrestatarioId { get; set; }

        // FK hacia OfertaPrestamo
        public int OfertaPrestamoId { get; set; }

        public decimal SaldoRestante { get; set; }

        public decimal SaldoPrestamo { get; set; } 
        public int CuotasRestantes { get; set; } 

        public DateTime FechaInicio { get; set; }
        public DateTime FechaProxPago { get; set; }

        public string Estado { get; set; } = string.Empty;

        // Relaciones
        public OfertaPrestamo? OfertaPrestamo { get; set; }
        public Prestatario? Prestatario { get; set; }

        // Relación inversa
        public ICollection<Transaccion>? Transacciones { get; set; }
    }
}
