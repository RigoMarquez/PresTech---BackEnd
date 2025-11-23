namespace PresTechBackEnd.DTO
{
    public class SolicitarPrestamoDTO
    {
        public int PrestamoId { get; set; }
        public int PrestatarioId { get; set; }
        public int OfertaPrestamoId { get; set; }
        public string SaldoPrestamo { get; set; } = string.Empty;
        public decimal SaldoRestante { get; set; }
        public string CuotasRestantes { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaProxPago { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
