namespace LojaVirtual.Requests;

public class CheckoutRequest
{
    public string PrimeiroNome { get; set; }
    public string Sobrenome { get; set; }
    public string Email { get; set; }
    public string Endereco { get; set; }
    public string Pais { get; set; }
    public string Estado { get; set; }
    public string CodigoPostal { get; set; }
}