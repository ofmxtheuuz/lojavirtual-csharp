using System.Globalization;
using LojaVirtual.Data;
using LojaVirtual.Requests;
using LojaVirtual.Response;
using LojaVirtual.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LojaVirtual.Controllers;

public class MercadoPagoController : Controller
{
    private readonly MercadoPagoService _mercadoPagoService;
    private readonly AppDbContext _context;
    private readonly CarrinhoService _carrinhoService;
    private readonly MailService _mail;

    public MercadoPagoController(MercadoPagoService mercadoPagoService, AppDbContext context, CarrinhoService carrinhoService, MailService mail)
    {
        this._mercadoPagoService = mercadoPagoService;
        _context = context;
        _carrinhoService = carrinhoService;
        _mail = mail;
    }

    [HttpGet("/v2/mp/back")]
    public async Task<RedirectToActionResult> BacksUrl(string status, string payment_id, string external_reference)
    {
        
        var task = await _mercadoPagoService.BackUrl(payment_id, external_reference);
        if (task)
        {
            await _context.SaveChangesAsync();
            return RedirectToAction("Successfully");
        }
        else
        {
            return RedirectToAction("Error");
        }
    }
    [HttpPost("/v2/mp/notification")]
    public async Task Notification([FromBody] NotificationRequest req)
    {
        using (var httpClient = new HttpClient())
        {
            using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://api.mercadopago.com/v1/payments/{req.data.id}"))
            {
                request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Settings.AccessToken}"); 

                var response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                FindMercadoPagoResponse res = JsonConvert.DeserializeObject<FindMercadoPagoResponse>(content)!;
                var fatura = await _context.Faturas.FirstOrDefaultAsync(fatura => fatura.ExternalReference == res.external_reference);
                if (fatura != null)
                {
                    fatura.FaturaStatus = res.status;
                    fatura.LastUpdatedDate = res.date_last_updated.ToString(CultureInfo.CurrentCulture);
                }
                await _context.SaveChangesAsync();
            }
        }
    }

    [Authorize]
    public IActionResult Successfully()
    { 
        var carrinhoId = User.Claims.FirstOrDefault(x => x.Type == "USER_CARRINHO_ID")?.Value;
        if (carrinhoId != null) _carrinhoService.LimparCarrinho(carrinhoId);
        return View();
    }
    
    [Authorize]
    public IActionResult Error()
    {
        ViewBag.Error = "::err";
        return View();
    }
}