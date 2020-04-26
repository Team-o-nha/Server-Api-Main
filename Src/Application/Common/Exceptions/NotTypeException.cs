using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.Common.Exceptions
{
    public class NotTypeException : Exception
    {
        public NotTypeException(string typeName, List<string> listType)
            : base($"Type \"{typeName}\" is not in ({string.Join(',', listType)}) was not found.")
        {
        }
    }
}
