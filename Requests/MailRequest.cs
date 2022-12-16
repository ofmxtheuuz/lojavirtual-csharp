namespace LojaVirtual.Requests;

public class MailRequest
{
    public string Subject { get; set; }
    public string EmailTo { get; set; }
    public string NameTo { get; set; }
    public string plainText { get; set; }
    public string htmlContent { get; set; }
}