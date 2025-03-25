using EMS_Project.Logical_Layer.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
namespace EMS_Project.Logical_Layer.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";   
        private readonly int _port = 587;
        private readonly string _email = "devansh.otheruse0403@gmail.com";  
        private readonly string _password = "Devansh@Patel";


        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("EMS Project", _email));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpServer, _port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_email, _password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
