using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using IronSoccerDDD.Core.Exceptions;
using IronSoccerDDD.Core.Shared;
using System;

namespace IronSoccerDDD.Core.Entities
{
    public class Match: BaseEntity
    {
        public virtual Team TeamA { get; private set; }
        public virtual Team TeamB { get; private set; }
        public DateTime MatchDate { get; private set; }
        public virtual Team Winner { get; private set; }
        public virtual Player BestPlayer { get; private set; }

        //TODO: Constructor can not be PRIVATE & Navidation property Virtual because of we are using E.F Lazy Mode
        protected Match() { }

        public Match(Team teamA, Team teamB, DateTime matchDate)
        {
            Guard.Against.Null(teamA, nameof(teamA));
            Guard.Against.Null(teamB, nameof(teamB));
            Guard.Against.DateBeforeNow(matchDate, nameof(matchDate));

            if (teamA == teamB)
                throw new BusinessException($"Teams must be different");

            var canPlayTeamAResult = teamA.CanPlayOn(matchDate);
            if (canPlayTeamAResult.IsFailure)
                throw new BusinessException(canPlayTeamAResult.Error);

            var canPlayTeamBResult = teamB.CanPlayOn(matchDate);
            if (canPlayTeamBResult.IsFailure)
                throw new BusinessException(canPlayTeamBResult.Error);

            TeamA = teamA;
            TeamB = teamB;
            MatchDate = matchDate;
        }

        public void SetEndResult(Team winner, Player bestPlayer)
        {
            Guard.Against.Null(winner, nameof(winner));
            Guard.Against.Null(bestPlayer, nameof(bestPlayer));

            Winner = winner;
            BestPlayer = bestPlayer;
        }

        public Result ReschuduleMachDate(DateTime newMatchDate)
        {
            Guard.Against.Null(newMatchDate, nameof(newMatchDate));
            Guard.Against.Default(newMatchDate, nameof(newMatchDate));
            Guard.Against.DateBeforeNow(newMatchDate, nameof(newMatchDate));

            if (MatchDate == newMatchDate)
                return Result.Success();

            var canReschuduleResult =  TeamA.CanReschuduleMatchDate(this, newMatchDate);
            if(canReschuduleResult.IsFailure)
                Result.Failure(canReschuduleResult.Error);

            canReschuduleResult = TeamB.CanReschuduleMatchDate(this, newMatchDate);
            if (canReschuduleResult.IsFailure)
                Result.Failure(canReschuduleResult.Error);

            MatchDate = newMatchDate;

            return Result.Success();
        }
    }
}
