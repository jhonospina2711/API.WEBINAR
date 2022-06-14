using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Entities.Utils
{
    public class GenericUtil
    {
        private readonly HttpClient _httpClient;

        public GenericUtil(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public GenericUtil()
        {
        }

        public DateTime GetDateZone() => DateTime.UtcNow.AddHours(-5);

        public async static void SendOTPMessage(string email, int codeValidation)
        {
            var client = new SendGridClient(Environment.GetEnvironmentVariable("SendGridKey"));
            var from = new EmailAddress(Environment.GetEnvironmentVariable("EmailSendGrid"));
            var to = new EmailAddress(email, email);
            var msg = MailHelper.CreateSingleTemplateEmail(from, to, Environment.GetEnvironmentVariable("WellcomeTemplate"), new SendGridTemplateData() { Code = Convert.ToString(codeValidation), Email = "" });
            await client.SendEmailAsync(msg);
        }

        public async static void SendOTPResetPasswordMessage(string email, int codeValidation)
        {
            var client = new SendGridClient(Environment.GetEnvironmentVariable("SendGridKey"));
            var from = new EmailAddress(Environment.GetEnvironmentVariable("EmailSendGrid"));
            var to = new EmailAddress(email, email);
            var msg = MailHelper.CreateSingleTemplateEmail(from, to, Environment.GetEnvironmentVariable("ResetPasswordTemplate"), new SendGridTemplateData() { Code = Convert.ToString(codeValidation), Email = "" });
            await client.SendEmailAsync(msg);
        }

        public async static void SendOTPSupport(string email, int codeValidation, List<string> tos)
        {
            var client = new SendGridClient(Environment.GetEnvironmentVariable("SendGridKey"));
            var from = new EmailAddress(Environment.GetEnvironmentVariable("EmailSendGrid"));
            List<EmailAddress> emailAddresses = (from to in tos
                                                 select new EmailAddress(to, to)).ToList();
            var msg = MailHelper.CreateSingleTemplateEmailToMultipleRecipients(from, emailAddresses, Environment.GetEnvironmentVariable("NoveltyTemplate"), new SendGridTemplateData() { Code = Convert.ToString(codeValidation), Email = email });
            await client.SendEmailAsync(msg);
            SendOTPMessage(email, codeValidation);
        }

    }

}