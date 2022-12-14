using System.ComponentModel.DataAnnotations.Schema;

namespace LojaVirtual.Models;

public class PedidoDetalhe
{
    public int PedidoDetalheId { get; set; }
    public int PedidoId { get; set; }
    public int LancheId { get; set; }
    public int Quantidade { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public double Preco { get; set; }

    public virtual Produto Lanche { get; set; }
    public virtual Pedido Pedido { get; set; }
}