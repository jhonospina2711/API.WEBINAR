using System;
using System.Collections.Generic;
using System.Numerics;

namespace Entities
{
    public class UserInvestmentProjectResponse
    {
        public string UserWalletAccountTransactionId { get; set; }
        public decimal Coins { get; set; }
        public decimal ProjectCoins { get; set; }
        public decimal ProfitabilityPercentaje { get; set; }
    }
}