using Ardalis.GuardClauses;
using IronSoccerDDD.Core.Exceptions;
using IronSoccerDDD.Core.Shared;
using System;

namespace IronSoccerDDD.Core.Entities
{
    public class Player: BaseEntity
    {
        private const int MinAgeToPlay = 16;

        public virtual CompleteName CompleteName { get; private set; }
        public DateTime BirthDate { get; private set; }
        public Email Email { get; private set; }
        public string Phone { get; private set; }
        public virtual Team Team { get; private set; }

        protected Player(){ }

        public Player(CompleteName completeName, DateTime birthDate, Email email, string phone, Team team)
        {
            Guard.Against.Default(birthDate, nameof(birthDate));
            Guard.Against.NullOrWhiteSpace(phone, nameof(phone));
            Guard.Against.OutOfRange(phone.Length, nameof(phone), 1, 50);
            Guard.Against.Null(team, nameof(team));

            if (GetAge() < MinAgeToPlay)
                throw new BusinessException($"Min age required to play: { MinAgeToPlay } years old");

            var joinPlayerResult = team.JoinPlayer(this);
            if (joinPlayerResult.IsFailure)
                throw new BusinessException(joinPlayerResult.Error);

            CompleteName = completeName;
            BirthDate = birthDate;
            Email = email;
            Phone = phone;
            Team = team; 
        }

        //simple age calculation
        private int GetAge() => DateTime.Now.Year - BirthDate.Year;
    }
}
