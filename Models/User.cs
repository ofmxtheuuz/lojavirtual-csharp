using Microsoft.AspNetCore.Identity;

namespace LojaVirtual.Models;

public class User : IdentityUser
{
    public string CarrinhoComprasId { get; set; }
    public string APIKey { get; set; }
    public DateTime CreatedDate { get; set; }
}