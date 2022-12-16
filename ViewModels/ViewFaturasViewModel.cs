using LojaVirtual.Models;

namespace a;

public class ViewFaturasViewModel
{
    public Pedido Pedido { get; set; }
    public List<PedidoDetalhe> PedidoDetalhes { get; set; }
    public Fatura Fatura { get; set; }
}