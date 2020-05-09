using Ardalis.GuardClauses;
using System;

namespace IronSoccerDDD.Core.Exceptions
{
    public static class GuardExtesions
    {
        public static void DateBeforeNow(this IGuardClause guardClause, DateTime date, string parameterName)
        {
            if (date < DateTime.UtcNow)
                throw new ArgumentException("Date is before to current date time", parameterName);
        }

        public static void InvalidEmail(this IGuardClause guardClause, string email, string parameterName)
        {
            Guard.Against.NullOrWhiteSpace(email, nameof(email));

            try
            {
                var result = new System.Net.Mail.MailAddress(email);
                return;
            }
            catch (FormatException)
            {
                throw new ArgumentException($"Input {parameterName} was not a valid email.", parameterName);
            }
        }
    }
}
