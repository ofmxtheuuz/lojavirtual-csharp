namespace LojaVirtual.ViewModels;

public class HomeViewModel
{
    public List<LojaVirtual.Models.Produto> Produtos { get; set; }
    public List<LojaVirtual.Models.CarrinhoCompraItem> Carrinho { get; set; }
}