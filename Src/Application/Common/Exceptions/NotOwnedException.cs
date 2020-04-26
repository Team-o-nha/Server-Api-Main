using System;

namespace ColabSpace.Application.Common.Exceptions
{
    public class NotOwnedException : Exception
    {
        public NotOwnedException(string name, object key)
            : base($"Login user does not own this \"{name}\" ({key})")
        {
        }
    }
}
