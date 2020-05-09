﻿using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace IronSoccerDDD.Core.Entities
{
    public class Email : Shared.ValueObject
    {
        public string Value { get; private set; }

        private Email(string value)
        {
            Value = value;
        }

        public static Result<Email> Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Result.Failure<Email>("Email should not be empty");

            email = email.Trim();

            if (email.Length > 200)
                return Result.Failure<Email>("Email is too long");

            if (!Regex.IsMatch(email, @"^(.+)@(.+)$"))
                return Result.Failure<Email>("Email is invalid");

            return Result.Success(new Email(email));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator string(Email email)
        {
            return email.Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
