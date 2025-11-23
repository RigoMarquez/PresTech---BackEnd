namespace PresTechBackEnd.DTO
{
    public class PrestamoPrestamistaDTO
    {
        public int PrestamoId { get; set; }
        public int OfertaPrestamoId { get; set; }
        public string CategoriaPrestamo { get; set; }
        public decimal Tasas { get; set; }
        public decimal SaldoPrestamo { get; set; }
        public decimal SaldoRestante { get; set; }
        public int CuotasTotales { get; set; }
        public int CuotasRestantes { get; set; }
        public string Frecuencia { get; set; }
        public DateTime? FechaProxPago { get; set; }
        public string Estado { get; set; }
        
        public DateTime FechaPago { get; set; }

        public decimal MontoPagado { get; set; }

        public string TipoPago { get; set; }
        public int PrestatarioId { get; set; }
        public string NombrePrestatario { get; set; }

        public string EmailPrestatario { get; set; }

        public string TelefonoPrestatario { get; set; }

    }

}
