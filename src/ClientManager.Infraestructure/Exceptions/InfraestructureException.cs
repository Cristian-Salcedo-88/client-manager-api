using System;

namespace ClientManager.Infraestructure.Exceptions
{
    public class InfraestructureException : Exception
    {
        public InfraestructureException()
        { }

        public InfraestructureException(string message)
            : base(message)
        { }

        public InfraestructureException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
