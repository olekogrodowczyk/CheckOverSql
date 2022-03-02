using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public bool IsPublic { get; }
        public NotFoundException(string message, bool isPublic = false) : base(message)
        {
            IsPublic = isPublic;
        }

        public NotFoundException(string name, object key, bool isPublic=false) 
            : base($"Entity \"{name}\" ({key}) was not found")
        {
            IsPublic = isPublic;
        }
    }
}
