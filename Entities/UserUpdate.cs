using System;
using System.Collections.Generic;
using System.Numerics;

namespace Entities
{
    public class UserUpdate    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string DocumentId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int DocumentTypeId { get; set; }
        public string Email { get; set; }
        public string ProfileImage { get; set; }
        public bool Enabled { get; set; }
        public int InstitutionId { get; set; }
        public int SpecialtyId { get; set; }
        public int ProfileId { get; set; }
    }
}