using System;

namespace Studio.Core.Services
{
    public sealed class ServiceAlreadyRegisteredException : Exception
    {
        public ServiceAlreadyRegisteredException(Type serviceType)
            : base($"Service of type '{serviceType.Name}' is already registered.")
        {
        }
    }
}
