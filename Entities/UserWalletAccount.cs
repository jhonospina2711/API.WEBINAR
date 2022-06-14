using System;

namespace Entities
{
    public class UserWalletAccount
    {
        public string Id { get; set; }
        public decimal Coins { get; set; }
        public bool Enabled { get; set; }
        public int  UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual User User { get; set; }
    }
}