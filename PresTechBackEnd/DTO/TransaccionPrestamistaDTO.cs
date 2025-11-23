namespace PresTechBackEnd.DTO
{
    public class TransaccionPrestamistaDTO
    {

        public int TransaccionId { get; set; }
        public int PrestamoId { get; set; }
        public int OfertaPrestamoId { get; set; }
        public string Categoria { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteDocumento { get; set; }
        public string ClienteTelefono { get; set; }
        public string ClienteEmail { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public string TipoTransaccion { get; set; }
        public decimal SaldoRestante { get; set; }
    }
}
