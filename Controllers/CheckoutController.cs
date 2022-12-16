using LojaVirtual.Data;
using LojaVirtual.Models;
using LojaVirtual.Services;
using LojaVirtual.ViewModels;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Controllers;

[Authorize]
public class CheckoutController : Controller
{
    private readonly AppDbContext _context;
    private readonly pedidoService _pedidoService;
    private readonly MercadoPagoService _mercadoPagoService;
    private readonly MailService _mail;

    public CheckoutController(AppDbContext context, pedidoService pedidoService, MercadoPagoService mercadoPagoService, MailService mail)
    {
        _context = context;
        _pedidoService = pedidoService;
        _mercadoPagoService = mercadoPagoService;
        _mail = mail;
    }

    [HttpGet]
    [Route("/checkout")]
    public async Task<IActionResult> Index()
    {
        var id = User.Claims.FirstOrDefault(x => x.Type == "USER_CARRINHO_ID")?.Value;
        var carrinho = await _context.CarrinhoCompraItems.Include(x => x.Produto).Where(task => task.CarrinhoCompraId == id).ToListAsync();
        var precototal = 0.00;

        foreach (var item in carrinho)
        {
            precototal += item.Produto.Preco * item.Quantity;
        }
        
        return View(new CheckoutViewModel()
        {
            Carrinho = carrinho,
            QuantidadeProduto = carrinho.Count,
            PrecoTotal = precototal
        });
    }

    [HttpPost]
    [Route("/CheckoutService")]
    public async Task<IActionResult> CheckoutService(string firstName, string lastName, string email, string address, string estado, string zip, string cidade)
    {
        
        var carrinhoid = User.Claims.FirstOrDefault(x => x.Type == "USER_CARRINHO_ID")?.Value;
        var carrinho = _context.CarrinhoCompraItems.Include(x => x.Produto).Where(car => car.CarrinhoCompraId == carrinhoid).ToList();
        double total = 0;
        foreach (var item in carrinho)
        {
            total += (item.Quantity * item.Produto.Preco);
        }
        

        var token = await _pedidoService.AddPedido(new Pedido()
        {
            Nome = firstName,
            Sobrenome = lastName,
            Email = email,
            Endereco = address,
            Estado = estado,
            Cep = zip,
            Cidade = cidade,
            TotalItensPedido = carrinho.Count,
            PedidoTotal = (decimal)total,
            PedidoEnviado = DateTime.Now
        }, Guid.NewGuid().ToString());
        await _context.SaveChangesAsync();
        

        var pedido = await _context.Pedidos.FirstOrDefaultAsync(request => request.token == token);
        foreach (var detail in carrinho)
        {
            if (pedido != null)
            {
                var pedidodetail = new PedidoDetalhe()
                {
                    Pedido = pedido,
                    Lanche = detail.Produto,
                    Quantidade = detail.Quantity,
                    Preco = detail.Produto.Preco * detail.Quantity
                };
                await _pedidoService.AddPedidoDetalhe(pedidodetail);
            }
        }
        await _context.SaveChangesAsync();


        var external = await _pedidoService.AddFatura(new Fatura() { Preco = total }, token);
        await _context.SaveChangesAsync();

        var fatura = await _context.Faturas.FirstOrDefaultAsync(x => x.ExternalReference == external);
        await _mail.SendMail(new ()
        {
            Subject = "Pedido criado com sucesso",
            EmailTo = email,
            NameTo = firstName + lastName,
            plainText = "",
            htmlContent = $"<h1>Olá {firstName}</h1> É com muita felicidade que nós podemos dizer que seu pedido foi aprovado com sucesso!"
        });
        return Redirect($"/checkout/pagar/?id={fatura?.FaturaId}");
    }

    [HttpGet("/checkout/pagar/")]
    public async Task<IActionResult> Pagar(string id)
    {
        Fatura fatura = (await _context.Faturas.Include(x => x.Pedido).FirstOrDefaultAsync(x => x.FaturaId == id) ?? null)!;
        
        var initPoint = await _mercadoPagoService.GetInitPoint(new()
        {
            ProductTitle = "Produtos Loja Virtual",
            Price = fatura.Preco,
            NotificationUrl = "https://mxtheuz.com.br/v2/notify",
            ExternalReference = fatura.ExternalReference,
            BackUrls = new PreferenceBackUrlsRequest()
            {
                Success = "https://localhost:7148/v2/mp/back?status=success",
                Failure = "https://localhost:7148/v2/mp/back?status=failure",
                Pending = "https://localhost:7148/v2/mp/back?status=pending"
            }
        });

        Pedido pedido = (await _context.Pedidos.FirstOrDefaultAsync(x => x.PedidoId == fatura.PedidoId))!;
        List<PedidoDetalhe> pedidoDetalhe = await _context.PedidoDetalhes.Include(x => x.Lanche).Where(x => x.PedidoId == pedido.PedidoId).ToListAsync();
        ViewBag.MpId = initPoint.PreferenceMpId;
        return View(new CheckoutPagarViewModel()
        {
            Pedido = pedido,
            Fatura = fatura,
            PedidoDetalhe = pedidoDetalhe
        });
    }
}