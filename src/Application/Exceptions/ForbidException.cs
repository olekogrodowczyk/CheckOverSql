using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class ForbidException : Exception
    {
        public bool IsPublic { get; }
        public ForbidException(string message, bool isPublic = false) : base(message)
        {
            IsPublic = isPublic;
        }      
    }
}
