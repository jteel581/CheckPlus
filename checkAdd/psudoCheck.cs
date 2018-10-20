using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkAdd
{
    class psudoCheck
    {
        int accountNum;
        int routingNum;
        double ammount;

        public psudoCheck(int acct, int rout, double ammnt)
        {
            accountNum = acct;
            routingNum = rout;
            ammount = ammnt;

        }
        public double getAmmount() { return ammount; }

    }
}
