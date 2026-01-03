using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyCOLL.Data.Entities;

namespace MyCOLL.Data.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options) {
    public DbSet<TipoColecionavel> TiposColecionaveis { get; set; }
    public DbSet<Pais> Paises { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<ModoDisponibilizacao> ModosDisponibilizacao { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Encomenda> Encomendas { get; set; }
    public DbSet<ItemEncomenda> ItensEncomenda { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);

        // Configurar relação Produto -> Fornecedor
        builder.Entity<Produto>()
            .HasOne(p => p.Fornecedor)
            .WithMany(u => u.ProdutosFornecidos)
            .HasForeignKey(p => p.FornecedorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configurar relação Encomenda -> Cliente
        builder.Entity<Encomenda>()
            .HasOne(e => e.Cliente)
            .WithMany(u => u.Encomendas)
            .HasForeignKey(e => e.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
