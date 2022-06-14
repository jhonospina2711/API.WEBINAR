using System;
using System.Collections.Generic;
using System.Numerics;

namespace Entities
{
    public class UserInvestmentProjectModel
    {
        public string UserWalletAccountAccountId { get; set; }
        public int ProjectId { get; set; }
        public decimal Coins { get; set; }
    }
}