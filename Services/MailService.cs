using LojaVirtual.Requests;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace LojaVirtual.Services;

public class MailService
{
    public async Task<bool> SendMail(MailRequest req)
    {
        var apiKey = Settings.SendGrid;
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("contato@mxtheuz.com.br", "Matheus");
        var subject = req.Subject;
        var to = new EmailAddress(req.EmailTo, req.NameTo);
        var plainTextContent = req.plainText;
        var htmlContent = req.htmlContent;
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        
        var response = await client.SendEmailAsync(msg);

        Console.WriteLine($"E-mail enviado com sucesso: {req.EmailTo}");
        return true;
    }
}