using System;
using System.Collections.Generic;
using System.Numerics;

namespace Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Activated { get; set; }
        public bool Enabled { get; set; }
        public bool IsFirstLogin { get; set; }
        public int DependencyId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual Dependency Dependency { get; set; }
    }
}