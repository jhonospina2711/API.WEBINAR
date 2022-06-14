using System;

namespace Entities
{
    public class CreateWalletModel
    {
        public virtual UserWalletAccount UserWalletAccount { get; set; }
        public virtual WalletAccountWord WalletAccountWord { get; set; }
        public virtual UserWalletAccountTransaction UserWalletAccountTransaction { get; set; }
    }
}