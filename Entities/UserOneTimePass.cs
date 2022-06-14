using System;
using System.Collections.Generic;
using System.Numerics;

namespace Entities
{
    public class UserOneTimePass
    {
        public int Id { get; set; }
        public int OTPCode { get; set; }
        public bool Used { get; set; }
        public bool ReceivedByUser { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual User User { get; set; }
    }
}