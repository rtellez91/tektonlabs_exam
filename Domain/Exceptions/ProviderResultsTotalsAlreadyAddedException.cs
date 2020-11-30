using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SearchFight.Domain.Exceptions
{
    public class ProviderResultsTotalsAlreadyAddedException : Exception
    {
        public ProviderResultsTotalsAlreadyAddedException()
        {
        }

        public ProviderResultsTotalsAlreadyAddedException(string message) : base(message)
        {
        }

        public ProviderResultsTotalsAlreadyAddedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProviderResultsTotalsAlreadyAddedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
