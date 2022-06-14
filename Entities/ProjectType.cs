using System;
using System.Collections.Generic;
using System.Numerics;

namespace Entities
{
    public class ProjectType
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}