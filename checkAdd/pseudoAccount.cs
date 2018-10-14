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
        string streetNum;
        string streetName;
        string city;
        string state;
        string zip;
        double curBal;
        List<pseudoCheck> checks;
        int numOfChecks;

        public pseudoAccount(string first, string last, string rout, string account, string stNum, string stName, string cityName, string stateName, string zipCode)
        {
            firstName = first;
            lastName = last;
            routingNumber = rout;
            accountNumber = account;
            curBal = 0;
            numOfChecks = 0;
            streetNum = stNum;
            streetName = stName;
            city = cityName;
            state = stateName;
            zip = zipCode;
            checks = new List<pseudoCheck>();
        }

        public List<pseudoCheck> getChecks() { return checks; }

        public pseudoCheck getCheckByNum(string checkNum)
        {
            pseudoCheck check = null;
            foreach(pseudoCheck chk in checks)
            {
                string checkNumStr = chk.getCheckNum().ToString();
                if (checkNumStr == checkNum)
                {
                    return chk;
                }
            }
            return check;
        }

        public string getAccountNum()
        {
            return accountNumber;
        }
        public void setAccountNum(string newNum)
        {
            accountNumber = newNum;
        }
        public string getFirstName() { return firstName; }
        public void setFirstName(string fName)
        {
            firstName = fName;
        }
        public string getLastName() { return lastName; }
        public void setLastName(string LName)
        {
            lastName = LName;
        }
        public string getRoutingNum() { return routingNumber; }
        public void setRoutingNum(string rNum)
        {
            routingNumber = rNum;
        }
        public string getStNum() { return streetNum; }
        public void setStNum(string stNum)
        {
            streetNum = stNum;
        }
        public string getStName() { return streetName; }
        public void setStName(string stName)
        {
            streetName = stName;
        }
        public string getCity() { return city; }
        public void setCity(string cityName)
        {
            city = cityName;
        }
        public string getZip() { return zip; }
        public void setZip(string newZip)
        {
            zip = newZip;
        }
        public string getState() { return state; }
        public void setState(string newState)
        {
            state = newState;
        }
        public double getCurBal() { return curBal; }
        public void setCureBal(double newBal)
        {
            curBal = newBal;
        }
        public void addCheck(pseudoCheck check)
        {
            checks.Add(check);
            numOfChecks++;
            curBal += check.getAmmount();
        }
        public int getNumOfChecks() { return numOfChecks; }
    }
}
