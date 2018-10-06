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
    class AccountSQLer
    {
        private CheckPlusDB cpdb;
        public AccountSQLer(CheckPlusDB in_cpdb) { cpdb = in_cpdb; }
        public void TurnOnInsert()
        {
            cpdb.Database.ExecuteSqlCommand("set identity insert dbo.account on");
            cpdb.Database.ExecuteSqlCommand("set identity insert dbo.acct_holder on");
            cpdb.Database.ExecuteSqlCommand("set identity insert dbo.address on");
        }
        public void TurnOffInsert()
        {
            cpdb.Database.ExecuteSqlCommand("set identity insert dbo.account off");
            cpdb.Database.ExecuteSqlCommand("set identity insert dbo.acct_holder off");
            cpdb.Database.ExecuteSqlCommand("set identity insert dbo.address off");
        }


        /*  FUNCTION
         *  build an account out of the unique characteristics of an account record:
         *      Bank's routing number
         *      Account's account number
         *  if a record does not exist with the provided information,
         *      return null
         *      
         *  use it to build an Account instance 
         *      any time you need to call one of the "SQL-esque" Account functions below
         */
        public Account BuildAccount
        (
            string prmFirstNm, string prmLastNm, //account holder info
            string prmRoutNum, //bank info
            string prmAddress, string prmCity, string prmState, string prmZip, //address info
            string prmAcctNum, string prmPhnNum //account info
        )
        {
            Account tst_Acct = (
                from a in cpdb.Accounts
                join b in cpdb.Banks on a.Bank_id equals b.Bank_id
                where b.Routing_number == prmRoutNum
                    && a.Account_number == prmAcctNum
                select a
            ).FirstOrDefault();

            //there already was an account with that information
            if (tst_Acct != null) { return tst_Acct; }
            else
            {   //build a new account record because one does not already exist
                return new Account()
                {   
                    Acct_holder_id = (
                        from ah in cpdb.Acct_holders
                        where ah.First_name == prmFirstNm
                            && ah.Last_name == prmLastNm
                        select ah.Acct_holder_id
                    ).FirstOrDefault(),
                    Acct_holder_id_2 = null, //implement this later
                    Bank_id = (
                        from b in cpdb.Banks
                        where b.Routing_number == prmRoutNum
                        select b.Bank_id
                    ).FirstOrDefault(),
                    Address_id = (
                        from a in cpdb.Addresses
                        where a.Address_nm == prmAddress
                            && a.City == prmCity
                            && a.State == prmState
                            && a.Zip_code == prmZip
                        select a.Address_id
                    ).FirstOrDefault(),
                    Account_number = prmAcctNum,
                    Date_start = DateTime.Now,
                    Phone_number = null
                };
            }
        }


        public List<Account> GetAllAccounts() { return cpdb.Accounts.ToList(); }

        public void UpdateAccount(Account acctToUpdate)
        {

        }

        public Account InsertAccount(Account acctToInsert)
        {
            var tstAccount = (
                from a in cpdb.Accounts
                where a.Account_id == acctToInsert.Account_id
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
        public Acct_check BuildAcct_check
        (
            string prmAcctNum, //account info
            string prmRoutNum, //bank info
            string prmCheckNum, double prmAmt, DateTime prmDateWrit //check info
        )
        {
            Acct_check tstAcct_check = (
                from ac in cpdb.Acct_checks
                join a in cpdb.Accounts on ac.Account_id equals a.Account_id
                join b in cpdb.Banks on a.Bank_id equals b.Bank_id
                where a.Account_number == prmAcctNum
                    && b.Routing_number == prmRoutNum
                    && ac.Check_number == prmCheckNum
                select ac
            ).FirstOrDefault();

            if(tstAcct_check != null) { return tstAcct_check; }
            else
            {
                return new Acct_check()
                {
                    Account_id = (
                        from a in cpdb.Accounts
                        join b in cpdb.Banks on a.Bank_id equals b.Bank_id
                        where a.Account_number == prmAcctNum
                            && b.Routing_number == prmRoutNum
                        select a.Account_id
                    ).FirstOrDefault(),
                    Amount = prmAmt,
                    Date_written = prmDateWrit,
                    Date_received = DateTime.Now
                };
            }
        }
        public List<Acct_check> GetAllAcct_checks() { return cpdb.Acct_checks.ToList(); }
        public Acct_check GetAcct_check(string prmrout_num, string prmacct_num, string prmcheck_num)
        {
            return (
                from ac in cpdb.Acct_checks
                join a in cpdb.Accounts on ac.Account_id equals a.Account_id
                join b in cpdb.Banks on a.Bank_id equals b.Bank_id
                where b.Routing_number == prmrout_num
                    && a.Account_number == prmacct_num
                    && ac.Check_number == prmcheck_num
                select ac
            ).FirstOrDefault();
        }
        public void UpdateAcct_check(Acct_check acctChkToUpdate)
        {

        }
        public Acct_check InsertAcct_check(Acct_check acctChkToInsert)
        {
            return null;
        }
        public void DeleteAcct_check(Acct_check acctChkToDelete)
        {

        }
    }
    

    class Acct_holderSQLer
    {
        private CheckPlusDB cpdb;
        public Acct_holderSQLer(CheckPlusDB in_cpdb) { cpdb = in_cpdb; }
        public List<Acct_holder> GetAllAcct_holders() { return cpdb.Acct_holders.ToList(); }
        
        /*  FUNCTION
         *  this has the possibility of returning more than one person
         *      since first + last likely will not be a unique combination,
         *      so it is to be used for searching
        */
        public Acct_holder GetAcct_holder(string prmFirstNm, string prmLastNm)
        {   
            return (
                from ah in cpdb.Acct_holders
                where ah.First_name == prmFirstNm
                    && ah.Last_name == prmLastNm
                select ah
            ).FirstOrDefault();
        }
    }


    class BankSQLer
    {
        private CheckPlusDB cpdb;
        public BankSQLer(CheckPlusDB in_cpdb) { cpdb = in_cpdb; }
        public List<Bank> GetAllBanks() { return cpdb.Banks.ToList(); }
        public Bank GetBank(string prmRoutNum)
        {
            return (
                from b in cpdb.Banks
                where b.Routing_number == prmRoutNum
                select b
            ).FirstOrDefault();
        }
    }
}
