using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using IronSoccerDDD.Core.Events;
using IronSoccerDDD.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronSoccerDDD.Core.Entities
{
    public class Team : BaseEntity
    {
        private const int MaxMatchesInPeriod = 2;
        private const int MaxMatchesPerDay = 2;
        private const int MaxPlayersInTeam = 5;
        private readonly List<Player> _players = new List<Player>();
        private readonly List<Match> _homeMatches = new List<Match>();
        private readonly List<Match> _visitorMatches = new List<Match>();

        public string Name { get; private set; }
        public virtual Country Country { get; private set; }
        public virtual IReadOnlyList<Player> Players => _players.AsReadOnly();
        public virtual IReadOnlyList<Match> HomeMatches => _homeMatches.AsReadOnly();
        public virtual IList<Match> VisitorMatches => _visitorMatches.AsReadOnly();

        protected Team() { }

        public Team(string name, Country country)
        {
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.OutOfRange(name.Length, nameof(name), 1, 200);
            Guard.Against.Null(country, nameof(country));

            var registerNewTeamResult = country.CanRegisterNewTeam();
            if (registerNewTeamResult.IsFailure)
                throw new InvalidOperationException(registerNewTeamResult.Error);

            Name = name;
            Country = country;
        }

        public Result JoinPlayer(Player player)
        {
            if (_players.Count >= MaxPlayersInTeam)
                return Result.Failure("Max 5 players per team");

            if (_players.Contains(player))
                return Result.Failure("Player is already in team");

            _players.Add(player);

            //TODO: Raise event when a team joins a new player
            RaiseDomainEvent(new TeamJoinsNewPlayerEvent(this, player));

            return Result.Ok();
        }

        internal Result CanPlayOn(DateTime matchDate)
        {
            var matchesInPeriod = _homeMatches.Where(x => x.MatchDate.Year == matchDate.Year && x.MatchDate.Month == matchDate.Month)
                .Union(_visitorMatches.Where(x => x.MatchDate.Year == matchDate.Year && x.MatchDate.Month == matchDate.Month));

            if (matchesInPeriod.Count() >= MaxMatchesInPeriod)
                return Result.Failure($"{ Name } has { matchesInPeriod.Count() } matche/s in the period (MM-YYYY)");

            var hasAMatchOnDay = matchesInPeriod.Any(x => x.MatchDate.Day == matchDate.Day);

            if (hasAMatchOnDay)
                return Result.Failure($"{ Name } has already a match on { matchDate.ToString("dd/MM/yyyy") }");

            return Result.Success();
        }

        internal Result CanReschuduleMatchDate(Match currentMatch, DateTime newMatchDate)
        {
            var matchesInDay = _homeMatches.Where(x => x.MatchDate.Date == newMatchDate.Date)
                .Union(_visitorMatches.Where(x => x.MatchDate.Date == newMatchDate.Date))
                .Where(x => x != currentMatch);

            if (matchesInDay.Count() >= MaxMatchesPerDay)
                return Result.Failure($"{ Name } has already a match on { newMatchDate.ToString("dd/MM/yyyy") }");

            return Result.Success();

        }

    }
}
