using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SearchFight.Domain.Exceptions
{
    public class NotEnoughProvidersResultsException : Exception
    {
        private const string MessageTemplate = "You need to add at least 2 providers search results before running a fight.";
        public NotEnoughProvidersResultsException():this(MessageTemplate)
        {
        }

        public NotEnoughProvidersResultsException(string message) : base(message)
        {
        }

        public NotEnoughProvidersResultsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotEnoughProvidersResultsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
