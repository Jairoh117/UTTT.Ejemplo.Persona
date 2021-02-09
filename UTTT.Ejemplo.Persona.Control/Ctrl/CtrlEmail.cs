using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace UTTT.Ejemplo.Persona.Control.Ctrl
{
    public class CtrlEmail
    {
        public void sendEmail(String message, String file, String scope, String type)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();
                mailMessage.From = new MailAddress("18301381@uttt.edu.mx");
                mailMessage.To.Add(new MailAddress("gtavsantos39@gmail.com"));
                mailMessage.Subject = "Error / Exception Handling";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = this.createMessage(message, type, file, scope);
                smtpClient.Port = 587;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("gtavsantos39@gmail.com", "MariaSilverioSandovalh117");
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(mailMessage);
            }
            catch (Exception _e)
            {
                Console.WriteLine(_e.Message);
            }
        }


        private String createMessage(String message, String type, String file, String scope)
        {
            String now = DateTime.Now.ToString();
            String body = "<h1>Madre mia no hackiaste los momos</h1><br>" +
                "<p><strong>excepción: </strong>" + message + "<br><strong>Tipo de la excepción: </strong>" + type + "<br>" +
                "<strong>File: </strong>" + file + "<br><strong>Contexto: </strong>" + scope + "<br>" +
                "<strong>Fecha y Hora: </strong>" + now + "<br></p>";
            // String htmlBodyMail = String.Format(body, message, type, file, scope, now);
            return body;
        }
    }
}
