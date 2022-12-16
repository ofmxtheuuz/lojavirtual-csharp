using System.ComponentModel.DataAnnotations;

namespace LojaVirtual.Models;

public class  Fatura
{
    public string FaturaId { get; set; }

    public Pedido Pedido { get; set; }
    [Display(Name = "Pedido ID")]
    public int PedidoId { get; set; }
    
    [Display(Name = "Status da fatura")]
    public string FaturaStatus { get; set; }
    [Display(Name = "Referência externa")]
    public string ExternalReference { get; set; }
    [Display(Name = "Endereço de IP")]
    public string IPAddress { get; set; }
    [Display(Name = "ID da preferência")]
    public string PreferenceMPId { get; set; } 
    [Display(Name = "Preço")]
    public double Preco { get; set; }
    [Display(Name = "Método de pagamento")]
    public string MetodoDePagamento  { get; set; }
    [Display(Name = "Data de criação")]
    
    public string CreatedDate { get; set; }
    [Display(Name = "Última atualização")]
    public string LastUpdatedDate { get; set; }

}