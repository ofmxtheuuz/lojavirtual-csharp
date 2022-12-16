using MercadoPago.Client.Preference;

namespace LojaVirtual.Requests;

public class InitPointRequest
{
    public string ProductTitle { get; set; }
    public double Price { get; set; }
    public string NotificationUrl { get; set; }
    public string ExternalReference { get; set; }
    public PreferenceBackUrlsRequest BackUrls { get; set; }
}