using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    public class ImpossibleOprationException : Exception
    {
        public ImpossibleOprationException()
        {
        }

        public ImpossibleOprationException(string message) : base(message)
        {
        }

        public ImpossibleOprationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ImpossibleOprationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}