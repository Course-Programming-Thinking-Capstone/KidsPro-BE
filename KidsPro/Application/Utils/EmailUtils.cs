using System.Net;
using System.Net.Mail;
using Application.ErrorHandlers;

namespace Application.Utils;

public static class EmailUtils
{
    public static void SendEmail(string toEmail, string toSubject, string toContent)
    {
        // set up send email
        string sendto = toEmail;
        string subject = toSubject;
        string content = toContent;
        // this is sender email
        string fromEmail = "nguyentuanvu020901@gmail.com";
        string fromPasswordEmail = "fhnwtwqisekdqzcr";
        try
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            //set property for email you want to send
            mail.From = new MailAddress(fromEmail);
            mail.To.Add(sendto);
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = content;
            mail.Priority = MailPriority.High;
            //set smtp port
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            //set gmail pass sender
            smtp.Credentials = new NetworkCredential(fromEmail, fromPasswordEmail);
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
        catch (Exception ex)
        {
            throw new BadRequestException($"Send to {toEmail} failed");
        }
    }
}