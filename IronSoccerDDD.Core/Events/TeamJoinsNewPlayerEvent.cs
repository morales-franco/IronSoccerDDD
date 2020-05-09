using IronSoccerDDD.Core.Entities;
using IronSoccerDDD.Core.Interfaces;

namespace IronSoccerDDD.Core.Events
{
    public sealed class TeamJoinsNewPlayerEvent: IDomainEvent
    {
        public Team TeamWithNewPlayer { get; }
        public Player NewPlayer { get; }

        public TeamJoinsNewPlayerEvent(Team teamWithNewPlayer, Player newPlayer)
        {
            TeamWithNewPlayer = teamWithNewPlayer;
            NewPlayer = newPlayer;
        }
    }
}
