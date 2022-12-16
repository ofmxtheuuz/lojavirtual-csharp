using LojaVirtual.Models;

namespace LojaVirtual.ViewModels;

public class CheckoutPagarViewModel
{
    public Fatura Fatura { get; set; }
    public Pedido Pedido { get; set; }
    public List<PedidoDetalhe> PedidoDetalhe { get; set; }
}