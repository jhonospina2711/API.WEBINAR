using System;

namespace Entities
{
    public class UserWalletAccountTransactionResponse
    {
        public string Id { get; set; }
        public string Coins { get; set; }
        public decimal CoinsEarned { get; set; }
        public decimal ProfitabilityPercentaje { get; set; }
        public string UserWalletAccountId { get; set; }
        public int TransactionTypeId { get; set; }
        public string CreatedDate { get; set; }
        public string ProjectName { get; set; }
    }
}