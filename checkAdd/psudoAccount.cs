﻿using System;
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
        int routingNumber;
        int accountNumber;
        List<psudoCheck> checks;
        public psudoAccount(string first, string last, int rout, int account)
        {
            firstName = first;
            lastName = last;
            routingNumber = rout;
            accountNumber = account;
            checks = new List<psudoCheck>();
        }
        public void addCheck(psudoCheck check)
        {
            checks.Add(check);
        }
        
    }
}
