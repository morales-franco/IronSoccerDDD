using IronSoccerDDD.Core.Events;
using IronSoccerDDD.Core.Interfaces;

namespace IronSoccerDDD.Core.Handlers
{
    public class TeamJoinsNewPlayerHandler : IHandler<TeamJoinsNewPlayerEvent>
    {
        private readonly IEmailSender _emailSender;

        public TeamJoinsNewPlayerHandler(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public void Handle(TeamJoinsNewPlayerEvent domainEvent)
        {
            _emailSender.SendEmail(domainEvent.NewPlayer.Email,
                $"The player { domainEvent.NewPlayer.CompleteName.ToString() } is a new player of { domainEvent.TeamWithNewPlayer.Name }");
        }
    }
}
