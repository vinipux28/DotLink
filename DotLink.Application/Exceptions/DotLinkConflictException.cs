using System;

namespace DotLink.Application.Exceptions
{
    public class DotLinkConflictException : Exception
    {
        public DotLinkConflictException(string message)
            : base(message)
        {
        }

        public DotLinkConflictException(string entityName, string keyName, string keyValue)
            : base($"Conflict: Entity \"{entityName}\" with {keyName} \"{keyValue}\" already exists.")
        {
        }
    }
}