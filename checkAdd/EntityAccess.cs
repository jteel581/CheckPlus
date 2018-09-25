using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace checkAdd
{
    /*  SELECT * functions
        */

    /*  UDPATE functions
        */

    /*  INSERT functions
        *  checks to see if the account to insert already exists
        *   if it does, return the account and add nothing to the db
        *   if not, add the account and return null
        *  
        *  be sure to check the return value
        *   to determine if INSERT was successful
        *      
        */

    /*  DELETE functions
        */

    class AccountSQLer
    {
        private CheckPlusDB cpdb;
        public AccountSQLer(CheckPlusDB in_cpdb) { cpdb = in_cpdb; }

        /*  FUNCTION
            *  retrieves a list of all the accounts present in dbo.account
            *  probably most commonly used for displaying lists of accounts in dropdowns
            *      and things of that nature
            */
        public List<Account> GetAllAccounts() { return cpdb.Accounts.ToList(); }

        /*  FUNCTION
            *  updates the desired record
            */
        public void UpdateAccount(Account acctToUpdate)
        {
            int updEntity_id_1 = acctToUpdate.Entity_id_1;
            int updEntity_id_2 = acctToUpdate.Entity_id_2;
            string updAccount_number = acctToUpdate.Account_number;
            string updRouting_number = acctToUpdate.Routing_number;
            DateTime updDate_start = acctToUpdate.Date_start;
            DateTime? updDate_end = acctToUpdate.Date_end;

            var upAccount = cpdb.Accounts
                .Where(
                    x =>
                    x.Account_number == acctToUpdate.Account_number
                        && x.Routing_number == acctToUpdate.Routing_number)
                .ToList();

            //the actual update
            foreach (var a in upAccount)
            {
                a.Entity_id_1 = updEntity_id_1;
                a.Entity_id_2 = updEntity_id_2;
                a.Account_number = updAccount_number;
                a.Routing_number = updRouting_number;
                a.Date_start = updDate_start;
                a.Date_end = updDate_end;
            }

            cpdb.SaveChanges();
        }


        public Account InsertAccount(Account acctToInsert)
        {
            var tstAccount = cpdb.Accounts
                .Where(
                    x =>
                    x.Routing_number == acctToInsert.Routing_number
                        && x.Account_number == acctToInsert.Account_number
                ).FirstOrDefault();

            if (tstAccount == null)
            {
                //cpdb.Accounts.Add(acctToInsert);
                //cpdb.SaveChanges();
            }

            return tstAccount;
        }

        public void DeleteAccount(Account acctToDel)
        {
            GetAllAccounts()
                .RemoveAll(
                    x =>
                    x.Routing_number == acctToDel.Routing_number
                        && x.Account_number == acctToDel.Account_number
                );
        }
    }

    class Account_checkSQLer
    {
        private CheckPlusDB cpdb;
        public Account_checkSQLer(CheckPlusDB in_cpdb) { cpdb = in_cpdb; }

        public List<Account_check> GetAllAccount_checks() { return cpdb.Account_Checks.ToList(); }
        public void UpdateAccount_check(Account_check acctChkToUpdate)
        {

        }
        public void InsertAccount_check(Account_check acctChkToInsert)
        {

        }
        public void DeleteAccount_check(Account_check acctChkToDelete)
        {

        }
    }

    class PersonSQLer
    {
        private CheckPlusDB cpdb;
        public PersonSQLer(CheckPlusDB in_cpdb) { cpdb = in_cpdb; }
        public List<Person> SelectAllPeople() { return cpdb.People.ToList(); }
        public Person SelectPerson(Person person)
        {
            var tstPerson = cpdb.People
                .Where(
                    x =>
                    x.First_name == person.First_name
                        && x.Last_name == person.Last_name).FirstOrDefault();
            return tstPerson;
        }
        public void UpdatePerson(Person person)
        {

        }
        public Person InsertPerson(Person perToInsert)
        {
            var tstPerson = cpdb.People
                .Where(
                    x =>
                    x.First_name == perToInsert.First_name
                        && x.Last_name == perToInsert.Last_name
                ).FirstOrDefault();

            if (tstPerson == null)
            {
                cpdb.People.Add(perToInsert);
                cpdb.SaveChanges();
            }

            return perToInsert;
        }
        public void DeletePerson(Person person)
        {

        }
    }
    
}
