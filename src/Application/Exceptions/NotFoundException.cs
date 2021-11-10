﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public bool IsPublic { get; }
        public NotFoundException(string message, bool isPublic = false) : base(message)
        {
            IsPublic = IsPublic;
        }      
    }
}
