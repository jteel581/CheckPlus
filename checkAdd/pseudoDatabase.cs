using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkPlus
{
    class pseudoDatabase
    {
        List<pseudoAccount> psudoAccounts;
        public pseudoDatabase()
        {
            psudoAccounts = new List<pseudoAccount>();
        }
        public void addAccount(pseudoAccount act)
        {
            psudoAccounts.Add(act);
        }
        public pseudoAccount getAccountByNum(string acctNum)
        {
            foreach (pseudoAccount acct in psudoAccounts)
            {
                if (acct.getAccountNum() == acctNum)
                {
                    return acct;
                }
            }
            return null;
                 
        }
    }
}
