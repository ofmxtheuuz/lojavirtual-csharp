using LojaVirtual.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Data;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Produto> Produtos { get; set; }
    public DbSet<CarrinhoCompraItem> CarrinhoCompraItems { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<PedidoDetalhe> PedidoDetalhes { get; set; }
    public DbSet<Fatura> Faturas { get; set; }
}