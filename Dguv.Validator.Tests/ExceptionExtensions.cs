using System;

namespace Dguv.Validator.Tests
{
    public static class ExceptionExtensions
    {
        public static string GetFirstLine(this Exception exception)
        {
            return exception.Message.Split('\r', '\n')[0];
        }
    }
}
