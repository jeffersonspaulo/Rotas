using Microsoft.EntityFrameworkCore;
using Rotas.API.Entities;

namespace Rotas.API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Rota> Rotas { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rota>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Origem)
                      .IsRequired()
                      .HasMaxLength(3); 
                entity.Property(r => r.Destino)
                      .IsRequired()
                      .HasMaxLength(3);
                entity.Property(r => r.Valor)
                      .HasPrecision(10, 2);
            });
        }
    }
}
