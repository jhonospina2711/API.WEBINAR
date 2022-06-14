using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class SetPasswordModel
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }
}