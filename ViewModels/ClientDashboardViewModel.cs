using LojaVirtual.Models;

namespace LojaVirtual.ViewModels;

public class ClientDashboardViewModel
{
    public List<Pedido> pedidos { get; set; }
    public List<PedidoDetalhe> pedidos_detalhes { get; set; }
    public List<Fatura> faturas { get; set; }
}