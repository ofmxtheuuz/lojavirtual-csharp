using System.ComponentModel.DataAnnotations;

namespace LojaVirtual.Models;

public class Produto
{
    [Display(Name = "Id")]
    public int ProdutoId { get; set; }
    [Display(Name = "Nome")]
    public string Nome { get; set; }
    [Display(Name = "Preço")]
    public double Preco { get; set; }
    [Display(Name = "URL da imagem")]
    public string ImageUrl { get; set; }
}