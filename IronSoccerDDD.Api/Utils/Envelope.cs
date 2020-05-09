using System;

namespace IronSoccerDDD.Api.Utils
{
    public class Envelope<T>
    {
        public T Result { get; }
        public bool Success { get; }
        public string ErrorMessage { get; }
        public DateTime TimeGenerated { get; }

        protected internal Envelope(T result, bool success, string errorMessage)
        {
            Result = result;
            ErrorMessage = errorMessage;
            TimeGenerated = DateTime.UtcNow;
            Success = success;
        }
    }

    public class Envelope : Envelope<string>
    {
        protected Envelope(string errorMessage, bool success)
            : base(null, success, errorMessage)
        {
        }

        public static Envelope<T> Ok<T>(T result)
        {
            return new Envelope<T>(result, true, null);
        }

        public static Envelope Ok()
        {
            return new Envelope(null, true);
        }

        public static Envelope Error(string errorMessage)
        {
            return new Envelope(errorMessage, false);
        }
    }
}
