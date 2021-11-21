using System;
using System.Runtime.Serialization;

namespace ConsoleUI_BL
{
    [Serializable]
    internal class WrongInputFormatException : Exception
    {
        public WrongInputFormatException()
        {
        }

        public WrongInputFormatException(string message) : base(message)
        {
        }

        public WrongInputFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongInputFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}