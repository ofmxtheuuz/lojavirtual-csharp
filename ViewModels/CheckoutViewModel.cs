using LojaVirtual.Requests;

namespace LojaVirtual.ViewModels;

public class CheckoutViewModel
{
    public List<LojaVirtual.Models.CarrinhoCompraItem> Carrinho { get; set; }
    public int QuantidadeProduto { get; set; }
    public double PrecoTotal { get; set; }
}