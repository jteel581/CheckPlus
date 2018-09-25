using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkAdd
{
    class psudoCheck
    {
        string accountNum;
        string routingNum;
        double ammount;

        public psudoCheck(string acct, string rout, double ammnt)
        {
            accountNum = acct;
            routingNum = rout;
            ammount = ammnt;

        }
        public double getAmmount() { return ammount; }

    }
}
