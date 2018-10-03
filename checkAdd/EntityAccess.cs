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

namespace checkPlus
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
        public int GetNewAccountID()
        {
            return (from a in cpdb.Accounts
                    select a.Account_id).Max() + 1;
        }

        /*  FUNCTION
            *  updates the desired record
            */
        public void UpdateAccount(Account acctToUpdate)
        {
                /*
            int updAcct_holder_id = acctToUpdate.Acct_holder_id;
            int? updAcct_holder_id_2 = acctToUpdate.Acct_holder_id_2;
            int updBank_id = acctToUpdate.Bank_id;
            int updAddress_id = acctToUpdate.Address_id;
            string updAccount_number = acctToUpdate.Account_number;
            DateTime updDate_start = acctToUpdate.Date_start;
            DateTime? updDate_end = acctToUpdate.Date_end;
            string updPhone_number = acctToUpdate.Phone_number;

            var upAccount = cpdb.Accounts
                .Where(
                    x =>
                    x.Account_number == acctToUpdate.Account_number
                        && x.Routing_number == acctToUpdate.Routing_number)
                .ToList();

            //the actual update
            foreach (var a in upAccount)
            {
                a.Entity_id_1 = updAcct_holder_id;
                a.Entity_id_2 = updAcct_holder_id_2;
                a.Account_number = updAccount_number;
                a.Routing_number = updRouting_number;
                a.Date_start = updDate_start;
                a.Date_end = updDate_end;
            }

            cpdb.SaveChanges();
            */
        }


        public Account InsertAccount(Account acctToInsert)
        {
            var tstAccount = (
                from a in cpdb.Accounts
                join ah in cpdb.Acct_holders on a.Acct_holder_id equals ah.Acct_holder_id
                select a
                ).FirstOrDefault();

            if (tstAccount == null)
            {
                cpdb.Accounts.Add(acctToInsert);
                cpdb.SaveChanges();
            }

            return tstAccount;
        }

            /*
        public void DeleteAccount(Account acctToDel)
        {
            GetAllAccounts()
                .RemoveAll(
                    x =>
                    x.Routing_number == acctToDel.Routing_number
                        && x.Account_number == acctToDel.Account_number
                );
        }
        */
    }

    class Acct_checkSQLer
    {
        private CheckPlusDB cpdb;
        public Acct_checkSQLer(CheckPlusDB in_cpdb) { cpdb = in_cpdb; }
        public int GetNewAcct_check_id()
        {
            return (from ac in cpdb.Acct_checks
                    select ac.Acct_check_id).Max() + 1;
        }
        public List<Acct_check> GetAllAcct_checks() { return cpdb.Acct_checks.ToList(); }
        public Acct_check GetAcct_check(string param_rout_num, string param_acct_num, string param_check_num)
        {
            return (from ac in cpdb.Acct_checks
                    join a in cpdb.Accounts on ac.Account_id equals a.Account_id
                    join b in cpdb.Banks on a.Bank_id equals b.Bank_id
                    where b.Routing_number == param_rout_num
                        && a.Account_number == param_acct_num
                        && ac.Check_number == param_check_num
                    select ac
                ).FirstOrDefault();
        }
        public void UpdateAcct_check(Acct_check acctChkToUpdate)
        {

        }
        public void InsertAcct_check(Acct_check acctChkToInsert)
        {

        }
        public void DeleteAcct_check(Acct_check acctChkToDelete)
        {

        }
    }
    

    class Acct_holderSQLer
    {
        private CheckPlusDB cpdb;
        public Acct_holderSQLer(CheckPlusDB in_cpdb) { cpdb = in_cpdb; }
        public int GetNewAcct_holder_id()
        {
            return (from ah in cpdb.Acct_holders
                    select ah.Acct_holder_id).Max() + 1;
        }
        public List<Acct_holder> GetAllAcct_holders() { return cpdb.Acct_holders.ToList(); }
        public Acct_holder GetAcct_holder(string param_first_nm, string param_last_nm)
        {
            return (from ah in cpdb.Acct_holders
                    where ah.First_name == param_first_nm
                        && ah.Last_name == param_last_nm
                    select ah
                ).FirstOrDefault();
        }
    }
}
