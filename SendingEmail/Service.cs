using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace SendingEmail
{
    public class Service
    {
        private readonly ILogger<Service> logger;

        public Service(ILogger<Service> logger)
        {
            this.logger = logger;
        }

        //System.Net.Mail.SmtpClient
        public void SendEmailDefault()
        {
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.IsBodyHtml = true; //тело сообщения в формате HTML
                message.From = new MailAddress("admin@mycompany.com", "Моя компания"); //отправитель сообщения
                message.To.Add("mail@yandex.ru"); //адресат сообщения
                message.Subject = "Сообщение от System.Net.Mail"; //тема сообщения
                message.Body = "<div style=\"color: red;\">Сообщение от System.Net.Mail</div>"; //тело сообщения
                message.Attachments.Add(new Attachment("... путь к файлу ...")); //добавить вложение к письму при необходимости

                using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.gmail.com")) //используем сервера Google
                {
                    client.Credentials = new NetworkCredential("mail@gmail.com", "secret"); //логин-пароль от аккаунта
                    client.Port = 587; //порт 587 либо 465
                    client.EnableSsl = true; //SSL обязательно

                    client.Send(message);
                    logger.LogInformation("Сообщение отправлено успешно!");
                }
            }
            catch (Exception e)
            {
                logger.LogError(e.GetBaseException().Message);
            }
        }

        //MailKit.Net.Smtp.SmtpClient
        public void SendEmailCustom()
        {
            try
            {
                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress("Моя компания", "admin@mycompany.com")); //отправитель сообщения
                message.To.Add(new MailboxAddress("mail@yandex.ru")); //адресат сообщения
                message.Subject = "Сообщение от MailKit"; //тема сообщения
                message.Body = new BodyBuilder() { HtmlBody = "<div style=\"color: green;\">Сообщение от MailKit</div>" }.ToMessageBody(); //тело сообщения (так же в формате HTML)

                using (MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, true); //либо использум порт 465
                    client.Authenticate("mail@gmail.com", "secret"); //логин-пароль от аккаунта
                    client.Send(message);

                    client.Disconnect(true);
                    logger.LogInformation("Сообщение отправлено успешно!");
                }
            }
            catch (Exception e)
            {
                logger.LogError(e.GetBaseException().Message);
            }
        }
    }
}
