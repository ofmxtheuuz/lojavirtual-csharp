using System.Diagnostics;
using a;
using LojaVirtual.Data;
using Microsoft.AspNetCore.Mvc;
using LojaVirtual.Models;
using LojaVirtual.Requests;
using LojaVirtual.Services;
using LojaVirtual.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        
        if (User.Identity.IsAuthenticated)
        {
            var id = User.Claims.FirstOrDefault(x => x.Type == "USER_CARRINHO_ID").Value;
            return View(new HomeViewModel()
            {
                Produtos = await _context.Produtos.ToListAsync(),
                Carrinho = await _context.CarrinhoCompraItems.Where(x => x.CarrinhoCompraId == id).ToListAsync()
            });
        }
        else
        {
            return View();
        }
    }
    
    public async Task<IActionResult> Suporte()
    {
        var email = User.Claims.FirstOrDefault(x => x.Type == "USER_EMAIL").Value;
        ViewBag.Email = email;
        var user = _context.Users.FirstOrDefault(x => x.Email == email);
        ViewBag.Nome = user.UserName;
        return View();
    }


    [Authorize(Roles = "Admin")]
    [Route("/admin/dashboard")]
    public IActionResult Painel()
    {
        return View("Painel");
    }

    [Authorize]
    [Route("/client/dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var email = User.Claims.FirstOrDefault(x => x.Type == "USER_EMAIL").Value;
        var pedidos = await _context.Pedidos.Include(x => x.PedidoItens).Where(x => x.Email == email).ToListAsync();
        if (pedidos.Count > 0)
        {
            var pedido_id = pedidos.FirstOrDefault().PedidoId;
            var pedidos_detalhes =
                await _context.PedidoDetalhes.Include(x => x.Lanche).Where(x => x.PedidoId == pedido_id).ToListAsync();
            var faturas = await _context.Faturas.Include(x => x.Pedido).Where(x => x.Pedido.Email == email).ToListAsync();
            return View("Dashboard", new ClientDashboardViewModel()
            {
                pedidos = pedidos,
                faturas = faturas,
                pedidos_detalhes = pedidos_detalhes
            });
        }
        else
        {
            return View("Dashboard", new ClientDashboardViewModel()
            {
                pedidos = null,
                faturas = null,
                pedidos_detalhes = null
            });
        }
    }

    [Authorize]
    [Route("/client/apikey")]
    public ActionResult ApiKey()
    {
        ViewBag.ApiKey = User.Claims.FirstOrDefault(x => x.Type == "APIKEY").Value;
        return View();
    }

    [Authorize]
    [Route("/client/fatura/")]
    public async Task<IActionResult> View(int id)
    {
        var pedido = await _context.Pedidos.Include(x => x.PedidoItens).FirstOrDefaultAsync(x => x.PedidoId == id);
        var pedido_detalhes = await _context.PedidoDetalhes.Include(x => x.Lanche).Where(x => x.PedidoId == id).ToListAsync();
        var fatura = await _context.Faturas.FirstOrDefaultAsync(x => x.PedidoId == id);
        return View(new ViewFaturasViewModel()
        {
            Pedido = pedido,
            Fatura = fatura,
            PedidoDetalhes = pedido_detalhes
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost("/services/suport")]
    public async Task<IActionResult> SuporteMail(SuporteRequest req, [FromServices] MailService _mail)
    {
        var res = await _mail.SendMail(new()
        {
            Subject = "Suporte | " + req.Email,
            EmailTo = "mategame2402@gmail.com",
            NameTo = "Suporte",
            plainText = "",
            htmlContent = $"E-mail: {req.Email} <br> Nome: {req.Nome} <br> Mensagem: {req.Mensagem}"
        });
        if (res)
        {
            TempData["res"] = "Sua mensagem de suporte foi enviada com sucesso! Após alguns minutos iremos lhe retornar, fique de olho em seu e-mail";
        }
        else
        {
            TempData["res"] = "Não foi possível enviar sua mensagem de suporte";
        };
        

        return RedirectToAction("Suporte");
    }
}