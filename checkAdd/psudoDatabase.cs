using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkAdd
{
    class psudoDatabase
    {
        List<psudoAccount> psudoAccounts;
        public psudoDatabase()
        {
            psudoAccounts = new List<psudoAccount>();
        }
        public void addAccount(psudoAccount act)
        {
            psudoAccounts.Add(act);
        }
        public psudoAccount getAccountByNum(int acctNum)
        {
            foreach (psudoAccount acct in psudoAccounts)
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
