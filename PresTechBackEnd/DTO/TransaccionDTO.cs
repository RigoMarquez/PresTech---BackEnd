namespace PresTechBackEnd.DTO
{
    public class TransaccionDTO
    {
        public int TransaccionId { get; set; }
        public int PrestamoId { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal MontoPagado { get; set; }

        public string TipoPago { get; set; }
    }
}
