using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkAdd
{
    class psudoAccount
    {
        string firstName;
        string lastName;
        string routingNumber;
        string accountNumber;
        double curBal;
        List<psudoCheck> checks;
        int numOfChecks;

        public psudoAccount(string first, string last, string rout, string account)
        {
            firstName = first;
            lastName = last;
            routingNumber = rout;
            accountNumber = account;
            curBal = 0;
            numOfChecks = 0;
            checks = new List<psudoCheck>();
        }
        
        public string getAccountNum()
        {
            return accountNumber;
        }
        public string getFirstName() { return firstName; }
        public string getLastName() { return lastName; }
        public string getRoutingNum() { return routingNumber; }
        public double getCurBal() { return curBal; }
        public void addCheck(psudoCheck check)
        {
            checks.Add(check);
            numOfChecks++;
            curBal += check.getAmmount();
        }
        public int getNumOfChecks() { return numOfChecks; }
    }
}
