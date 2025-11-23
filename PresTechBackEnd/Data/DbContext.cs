using Microsoft.EntityFrameworkCore;
using PresTechBackEnd.Models;

namespace PresTech.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TipoDocumento> TipoDocumentos { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Prestamista> Prestamistas { get; set; }
        public DbSet<Prestatario> Prestatarios { get; set; }
        public DbSet<OfertaPrestamo> OfertasPrestamo { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Prestatario)
                .WithMany(pr => pr.Prestamos)
                .HasForeignKey(p => p.PrestatarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.OfertaPrestamo)
                .WithMany(op => op.Prestamos)
                .HasForeignKey(p => p.OfertaPrestamoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TipoDocumento>().HasData(
                new TipoDocumento { TipoDocumentoId = 1, Nombre = "Cédula de Ciudadanía" },
                new TipoDocumento { TipoDocumentoId = 2, Nombre = "Cédula de Extranjería" },
                new TipoDocumento { TipoDocumentoId = 3, Nombre = "Pasaporte" }
            );
        }
    }
}
