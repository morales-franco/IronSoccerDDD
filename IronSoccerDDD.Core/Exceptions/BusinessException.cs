using System;

namespace IronSoccerDDD.Core.Exceptions
{
    public class BusinessException: Exception
    {
        public BusinessException()
        {

        }

        public BusinessException(string message, Exception ex = null)
            :base(message, ex)
        {

        }
    }
}
