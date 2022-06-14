using System;

namespace Entities
{
    public class WalletAccountWord
    {
        public int Id { get; set; }
        public string Words { get; set; }
        public string UserWalletAccountId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual UserWalletAccount UserWalletAccount { get; set; }
    }
}