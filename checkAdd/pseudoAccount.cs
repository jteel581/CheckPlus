using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkPlus
{
    class pseudoAccount
    {
        string firstName;
        string lastName;
        string routingNumber;
        string accountNumber;
        double curBal;
        List<pseudoCheck> checks;
        int numOfChecks;

        public pseudoAccount(string first, string last, string rout, string account)
        {
            firstName = first;
            lastName = last;
            routingNumber = rout;
            accountNumber = account;
            curBal = 0;
            numOfChecks = 0;
            checks = new List<pseudoCheck>();
        }
        
        public string getAccountNum()
        {
            return accountNumber;
        }
        public string getFirstName() { return firstName; }
        public string getLastName() { return lastName; }
        public string getRoutingNum() { return routingNumber; }
        public double getCurBal() { return curBal; }
        public void addCheck(pseudoCheck check)
        {
            checks.Add(check);
            numOfChecks++;
            curBal += check.getAmmount();
        }
        public int getNumOfChecks() { return numOfChecks; }
    }
}
