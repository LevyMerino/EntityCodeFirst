using Microsoft.EntityFrameworkCore;

namespace DB
{
    public class HirCasaContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Ajuste> Ajustes { get; set; }
        public HirCasaContext(DbContextOptions<HirCasaContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>().ToTable("Cliente");  
            modelBuilder.Entity<Ajuste>().ToTable("Ajuste");  
            modelBuilder.Entity<Pago>().ToTable("Pago");  
        }
    }
}