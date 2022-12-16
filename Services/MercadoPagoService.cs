using LojaVirtual.Data;
using LojaVirtual.Migrations;
using LojaVirtual.Requests;
using LojaVirtual.Response;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Fatura = LojaVirtual.Models.Fatura;

namespace LojaVirtual.Services;

public class MercadoPagoService
{
    private readonly AppDbContext _context;

    public MercadoPagoService(AppDbContext context)
    {
        this._context = context;
    }

    public async Task<MercadoPagoGetInitResponse> GetInitPoint(InitPointRequest init)
    {
        MercadoPagoConfig.AccessToken = Settings.AccessToken;

        var request = new PreferenceRequest
        {
            Items = new List<PreferenceItemRequest>
            {
                new PreferenceItemRequest
                {
                    Title = init.ProductTitle,
                    Quantity = 1,
                    CurrencyId = "BRL",
                    UnitPrice = (decimal)init.Price,
                    Description = ""
                },
            },
            NotificationUrl = init.NotificationUrl,
            ExternalReference = init.ExternalReference,
            BackUrls = init.BackUrls
        };

        var client = new PreferenceClient();
        Preference preference = await client.CreateAsync(request);

        return new MercadoPagoGetInitResponse()
        {
            InitPoint = preference.InitPoint,
            PreferenceMpId = preference.Id
        };
    }

    public async Task<bool> BackUrl(string id, string external_reference)
    { 
        Fatura fatura = _context.Faturas.FirstOrDefault(x => x.ExternalReference == external_reference);
        
        using (var httpClient = new HttpClient())
        {
            using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://api.mercadopago.com/v1/payments/{id}"))
            {
                request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Settings.AccessToken}"); 

                var response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                FindMercadoPagoResponse res = JsonConvert.DeserializeObject<FindMercadoPagoResponse>(content);
                fatura.FaturaStatus = res.status;
                fatura.IPAddress = res.additional_info.ip_address;
                fatura.PreferenceMPId = res.id.ToString();
                fatura.MetodoDePagamento = res.payment_method_id;
                fatura.CreatedDate = DateTime.Now.AddDays(1).ToString();
            }
        }
        
        await _context.SaveChangesAsync();
        return true;
    }
}