using Estacionamento.Domain;
using Microsoft.EntityFrameworkCore;

namespace Estacionamento.Web.Data;

public class EstacionamentoDbContext : DbContext
{
    public EstacionamentoDbContext(DbContextOptions<EstacionamentoDbContext> options) : base(options)
    {
    }

    public DbSet<RegistroEstacionamento> RegistrosEstacionamento => Set<RegistroEstacionamento>();
    public DbSet<TabelaPreco> TabelasPreco => Set<TabelaPreco>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RegistroEstacionamento>()
            .HasIndex(r => r.Placa);
    }
}