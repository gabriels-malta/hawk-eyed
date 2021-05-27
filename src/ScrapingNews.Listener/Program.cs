using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ScrapingNews.Listener
{
    class Program
    {
        static async Task Main(string[] args)
        {
            EmailSettings emailSettings = new EmailSettings();
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            configuration.Bind("EmailSettings", emailSettings);

            var scrap = ScrapAnvisa.ScrapDosUltimosSeteDias("medicamentos");
            await scrap.Carregar();

            var corpoDoEmailTask = CriarCorpoDoEmail(emailSettings, scrap);
            MailMessage mailMessage = await CriarMensagemDeEmail(emailSettings, corpoDoEmailTask);

            using (var client = new SmtpClient(emailSettings.SmtpServer))
            {
                PopularClienteSmtp(emailSettings, client);
                client.Send(mailMessage);
            }

            Console.Write("Done");
        }

        private static void PopularClienteSmtp(EmailSettings emailSettings, SmtpClient client)
        {
            client.EnableSsl = emailSettings.EnableSsl;
            client.Port = emailSettings.Port;
            client.DeliveryMethod = Enum.Parse<SmtpDeliveryMethod>(emailSettings.DeliveryMethod);
            client.UseDefaultCredentials = emailSettings.UseDefaultCredentials;
            client.Credentials = new NetworkCredential(emailSettings.MailSender.email, emailSettings.MailSender.passwd);
        }

        private static async Task<MailMessage> CriarMensagemDeEmail(EmailSettings emailSettings, Task<string> mailBodyTask)
        {
            MailAddress from = new MailAddress(emailSettings.MailSender.email);
            MailAddress to = new MailAddress(emailSettings.MailReceiver.email);
            MailMessage mailMessage = new MailMessage(from, to);
            mailMessage.Body = await mailBodyTask;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = emailSettings.Subject;
            mailMessage.SubjectEncoding = Encoding.UTF8;
            return mailMessage;
        }

        private static async Task<string> CriarCorpoDoEmail(EmailSettings emailSettings, ScrapAnvisa scrapAnvisa)
        {
            var carregarArquivoTask = File.ReadAllTextAsync(emailSettings.BodyTemplate);

            StringBuilder strTemplateLinhas = new StringBuilder();
            string rowTemplate(string data, string titulo, string link) => $@"<tr><td>{data}</td><td><a href=""{link}"" target=""_blank"">{titulo}</a></td></tr>";
            foreach (var node in scrapAnvisa.ObterNodes())
            {
                var hyperLink = scrapAnvisa.ObterHyperlink(node);
                var spanDtPublicacao = scrapAnvisa.ObterDataDePublicacao(node);
                strTemplateLinhas.Append(rowTemplate(spanDtPublicacao, hyperLink.titulo, hyperLink.link));
            }

            var (filtro, dtInicio, dtFim) = scrapAnvisa.Filtros();
            StringBuilder msgBody = new StringBuilder(await carregarArquivoTask);
            msgBody.Replace("##TEXTO_FILTRO##", filtro);
            msgBody.Replace("##DATA_INICIO##", dtInicio);
            msgBody.Replace("##DATA_FIM##", dtFim);
            msgBody.Replace("##LINHAS##", strTemplateLinhas.ToString());

            return msgBody.ToString();
        }
    }
}
