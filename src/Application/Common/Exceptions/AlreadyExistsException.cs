using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public bool IsPublic { get; set; }
        public AlreadyExistsException(string message, bool isPublic = false) : base(message)
        {
            IsPublic = isPublic;
        }
    }
}
