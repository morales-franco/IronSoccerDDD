using IronSoccerDDD.Core.Interfaces;
using System;

namespace IronSoccerDDD.Infraestructure.Services
{
    public class EmailSenderService : IEmailSender
    {
        public void SendEmail(string toAddress, string messageBody)
        {
            Console.WriteLine($"Sending an email: { messageBody } to { toAddress }");
        }
    }
}
