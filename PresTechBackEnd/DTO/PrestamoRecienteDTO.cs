namespace PresTechBackEnd.DTO
{
    public class PrestamoRecienteDTO
    {
        public int PrestamoId { get; set; }
        public int OfertaPrestamoId { get; set; }
        public string Categoria { get; set; }
        public string Estado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
    }
}
