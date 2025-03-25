using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using IKEA.DAL.Models.Identity;

namespace IKEA.BLL.Services.EmailSettings
{
    public class EmailSettings : IEmailSettings
    {
        public void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            //Sender - Receiver
            //receiver = ahmedelkhatieb94@gmail.com => user trying to reset

            //Generate Password
            client.Credentials = new NetworkCredential("ahmedelkhatieb94@gmail.com", "xogkatfrlnisirwi");
            client.Send("ahmedelkhatieb94@gmail.com", email.To, email.Subject, email.Body);

        }
    }
}
