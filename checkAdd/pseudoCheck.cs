using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkPlus
{
    class pseudoCheck
    {
        string accountNum;
        string routingNum;
        double ammount;
        int checkNum;

        public pseudoCheck(string acct, string rout, double ammnt)
        {
            accountNum = acct;
            routingNum = rout;
            ammount = ammnt;

        }
        public pseudoCheck(string acct, string rout, double ammnt, int num)
        {
            accountNum = acct;
            routingNum = rout;
            ammount = ammnt;
            checkNum = num;
        }
        public double getAmmount() { return ammount; }

    }
}
