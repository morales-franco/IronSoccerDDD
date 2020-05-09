using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace IronSoccerDDD.Core.Entities
{
    public class CompleteName : Shared.ValueObject
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        protected CompleteName() { }

        private CompleteName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public static Result<CompleteName> Create(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                return Result.Failure<CompleteName>("First name should not be empty");
            if (string.IsNullOrWhiteSpace(lastName))
                return Result.Failure<CompleteName>("Last name should not be empty");

            firstName = firstName.Trim();
            lastName = lastName.Trim();

            if (firstName.Length > 100)
                return Result.Failure<CompleteName>("First name is too long");
            if (lastName.Length > 150)
                return Result.Failure<CompleteName>("Last name is too long");

            return Result.Success(new CompleteName(firstName, lastName));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
        }

        public override string ToString() => $"{ FirstName } { LastName }";
    }
}
