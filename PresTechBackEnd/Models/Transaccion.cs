namespace PresTechBackEnd.Models
{
    public class Transaccion
    {
        public int TransaccionId { get; set; }

        // FK hacia Prestamo
        public int PrestamoId { get; set; }

        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public string TipoTransaccion { get; set; } = string.Empty;

        // Relación
        public Prestamo Prestamo { get; set; } = null!;
    }
}
