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
            pseudoAccount act = new pseudoAccount("John", "Smith", "111111111", "000000001", "100", "Greenville St", "Anderson", "SC", "29621");
            pseudoCheck chk = new pseudoCheck("000000001", "111111111", 229.53, 432);
            act.addCheck(chk);
            addAccount(act);
            act = new pseudoAccount("Jack", "Dickson", "222222222", "123123123", "50", "West I St.", "Anderson", "SC", "29621");
            chk = new pseudoCheck("123123123", "222222222", 315.87, 219);
            act.addCheck(chk);
            addAccount(act);
            act = new pseudoAccount("Bubby", "Felkel", "888888888", "200200200", "10", "Pasture Ln", "Orangeburg", "SC", "29030");
            chk = new pseudoCheck("200200200", "888888888", 12.87, 1003);
            act.addCheck(chk);
            addAccount(act);
            act = new pseudoAccount("Roger", "Codger", "777777777", "192568312", "831", "Mize Rd", "Belton", "SC", "29627");
            chk = new pseudoCheck("192568312", "777777777", 2599.99, 17);
            act.addCheck(chk);
            addAccount(act);
            act = new pseudoAccount("Jim", "Cramer", "333333333", "000000021", "1115", "Booya St", "Summit", "NJ", "07901");
            chk = new pseudoCheck("000000021", "333333333", 28759.53, 1857);
            act.addCheck(chk);
            addAccount(act);
        }

        public List<pseudoAccount> getAccountsList() { return psudoAccounts; }

        public void addAccount(pseudoAccount act)
        {
            psudoAccounts.Add(act);
        }
        public void deleteAccount(pseudoAccount act)
        {
            psudoAccounts.Remove(act);
            
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
