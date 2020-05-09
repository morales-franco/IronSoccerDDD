namespace IronSoccerDDD.Core.Interfaces
{
    public interface IEmailSender
    {
        void SendEmail(string toAddress, string messageBody);
    }
}
