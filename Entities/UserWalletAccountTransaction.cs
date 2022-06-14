using System;

namespace Entities
{
    public class UserWalletAccountTransaction
    {
        public string Id { get; set; }
        public decimal Coins { get; set; }
        public decimal CoinsEarned { get; set; }
        public decimal ProfitabilityPercentaje { get; set; }
        public string UserWalletAccountId { get; set; }
        public int TransactionTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual UserWalletAccount UserWalletAccount { get; set; }
        public virtual TransactionType TransactionType { get; set; }
    }
}