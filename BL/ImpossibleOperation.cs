using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    public class ImpossibleOperation : Exception
    {
        public ImpossibleOperation()
        {
        }

        public ImpossibleOperation(string message) : base(message)
        {
        }

        public ImpossibleOperation(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ImpossibleOperation(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}