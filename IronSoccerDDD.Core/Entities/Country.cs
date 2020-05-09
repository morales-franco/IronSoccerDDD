using CSharpFunctionalExtensions;
using IronSoccerDDD.Core.Shared;
using System.Collections.Generic;

namespace IronSoccerDDD.Core.Entities
{
    public class Country: BaseEntity
    {
        //Countries are fixed
        public static readonly Country Argentina = new Country(1, "Argentina");
        public static readonly Country NewZealand = new Country(2, "New Zealand");
        public static readonly Country Germany = new Country(3, "Germany");
        public static readonly Country Chile = new Country(4, "Chile");
        public static readonly Country Spain = new Country(5, "Spain");

        public static readonly IReadOnlyList<Country> AllCountries = 
            new List<Country>{ Argentina, NewZealand, Germany, Chile, Spain };

        private const int MaxTeamsPerCountry = 2;
        private readonly List<Team> _teams = new List<Team>();

        public string Name { get; private set; }
        public virtual IReadOnlyList<Team> Teams => _teams.AsReadOnly();

        protected Country()
        {

        }

        private Country(int id, string name)
            :base(id)
        {
            Name = name;
        }

        public Result CanRegisterNewTeam()
        {
            if (_teams.Count >= MaxTeamsPerCountry)
                return Result.Failure($"{ Name } has already got the max teams allowed");

            return Result.Success();
        }
    }
}
