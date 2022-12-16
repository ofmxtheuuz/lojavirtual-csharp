namespace LojaVirtual.Models;

public class CarrinhoCompraItem
{
    public int CarrinhoCompraItemId { get; set; }
    public Produto Produto { get; set; }
    public int ProdutoId { get; set; }
    public int Quantity { get; set; }
    public string CarrinhoCompraId { get; set; }
}