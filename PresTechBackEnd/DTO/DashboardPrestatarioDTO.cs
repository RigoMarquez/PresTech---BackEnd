namespace PresTechBackEnd.DTO
{
    public class DashboardPrestatarioDTO
{
    public decimal DeudaTotal { get; set; }
    public decimal TotalPagado { get; set; }
    public DateTime? ProximoPago { get; set; }
    public int PagosVencidos { get; set; }

    public List<PrestamoRecienteDTO> UltimosPrestamos { get; set; } = new();
}

}
