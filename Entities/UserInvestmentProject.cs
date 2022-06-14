using System;
using System.Collections.Generic;
using System.Numerics;

namespace Entities
{
    public class UserInvestmentProject
    {
        public int Id { get; set; }
        public string UserWalletAccountTransactionId { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual UserWalletAccountTransaction UserWalletAccountTransaction { get; set; }
        public virtual Project Project { get; set; }
    }
}