namespace PresTechBackEnd.DTO
{
    public class DashboardPrestamistaDTO
    {
        public decimal TotalPrestamosActivos { get; set; }
        public decimal PagosRecibidosMes { get; set; }

        public decimal PagosRecibidos { get; set; }
        public int ClientesActivos { get; set; }
        public int OfertasCreadas { get; set; }
        public List<PrestamoRecienteDTO> PrestamosRecientes { get; set; }
    }
}
