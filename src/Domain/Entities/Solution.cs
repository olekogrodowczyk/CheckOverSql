﻿using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Solution : AuditableEntity
    {
        public string Dialect { get; set; }
        public string Query { get; set; }
        public int ExerciseId { get; set; }
        public virtual Exercise Exercise { get; set; }
        public int CreatorId { get; set; }
        public virtual User Creator { get; set; }
        public virtual Comparison Comparison { get; set; }
        public bool? IsValid { get; set; }
        public bool Outcome { get; set; } = false;
        public virtual ICollection<Solving> Solvings { get; set; }
    }
}
