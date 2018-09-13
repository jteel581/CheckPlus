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
    }
}
