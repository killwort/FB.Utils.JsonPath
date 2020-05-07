using System;

namespace FB.Utils.JsonPath.Language
{
    public class InvalidSyntaxException : Exception
    {
        public InvalidSyntaxException(Error[] errors)
        {
            Errors = errors;
        }

        public Error[] Errors { get; }
    }
}
