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
                    First_name = prmFirstNm,
                    Last_name = prmLastNm,
                    Bank_id = (
                        from b in cpdb.Banks
                        where b.Routing_number == prmRoutNum
                        select b.Bank_id
                    ).FirstOrDefault(),
                    Address = prmAddress,
                    City = prmCity,
                    State = prmState,
                    Zip_code = prmZip,
                    Account_number = prmAcctNum,
                    Phone_number = null
                };
            }
        }

        /*  FUNCTION
         *  returns a list of all accounts in the database
         */
        public List<Account> GetAllAccounts() { return cpdb.Accounts.ToList(); }

        /*  FUNCTION
         *  pass in the account to update and an "Account record" with the new updated info
         *  set all the current account record's info to the provided new info
         */
        public void UpdateAccount(Account prmAcctToUpdate, Account prmNewAcctInfo)
        {
            prmAcctToUpdate.First_name = prmNewAcctInfo.First_name;
            prmAcctToUpdate.Last_name = prmNewAcctInfo.Last_name;
            prmAcctToUpdate.First_name_2 = prmNewAcctInfo.First_name_2;
            prmAcctToUpdate.Last_name_2 = prmNewAcctInfo.Last_name_2;
            prmAcctToUpdate.Bank_id = prmNewAcctInfo.Bank_id;
            prmAcctToUpdate.Address = prmNewAcctInfo.Address;
            prmAcctToUpdate.City = prmNewAcctInfo.City;
            prmAcctToUpdate.State = prmNewAcctInfo.State;
            prmAcctToUpdate.Country = prmNewAcctInfo.Country;
            prmAcctToUpdate.Zip_code = prmNewAcctInfo.Zip_code;
            prmAcctToUpdate.Account_number = prmNewAcctInfo.Account_number;

            cpdb.SaveChanges();
        }

        public Account InsertAccount(Account prmAccount)
        {
            var tstAccount = (
                from a in cpdb.Accounts
                where a.Account_id == prmAccount.Account_id
                select a
                ).FirstOrDefault();

            if (tstAccount == null)
            {
                cpdb.Accounts.Add(prmAccount);
                cpdb.SaveChanges();
            }

            return tstAccount;
        }

        public void DeleteAccount(Account prmAccount)
        {
            cpdb.Accounts.Remove((
                from a in cpdb.Accounts
                where a.Account_id == prmAccount.Account_id
                select a
            ).FirstOrDefault());

            cpdb.SaveChanges();
        }
    }

    class Acct_checkSQLer
    {
        private CheckPlusDB cpdb;
        public Acct_checkSQLer(CheckPlusDB in_cpdb) { cpdb = in_cpdb; }
        public void TurnOnInsert()
        {
            cpdb.Database.ExecuteSqlCommand("set identity insert dbo.acct_check on");
            cpdb.Database.ExecuteSqlCommand("set identity insert dbo.account on");
            cpdb.Database.ExecuteSqlCommand("set identity insert dbo.acct_holder on");
            cpdb.Database.ExecuteSqlCommand("set identity insert dbo.address on");
        }
        public void TurnOffInsert()
        {
            cpdb.Database.ExecuteSqlCommand("set identity insert dbo.acct_check off");
            cpdb.Database.ExecuteSqlCommand("set identity insert dbo.account off");
            cpdb.Database.ExecuteSqlCommand("set identity insert dbo.acct_holder off");
            cpdb.Database.ExecuteSqlCommand("set identity insert dbo.address off");
        }
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

            if (tstAcct_check != null) { return tstAcct_check; }
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
        public Acct_check GetAcct_check(Acct_check prmAcctCheck)
        {
            return (
                from ac in cpdb.Acct_checks
                where ac.Acct_check_id == prmAcctCheck.Acct_check_id
                select ac
            ).FirstOrDefault();
        }
        public void UpdateAcct_check(Acct_check prmChkToUpdate, Acct_check prmChkNewInfo)
        {
            prmChkToUpdate.Acct_check_id = prmChkNewInfo.Acct_check_id;
            prmChkToUpdate.Amount = prmChkNewInfo.Amount;
            prmChkToUpdate.Check_number = prmChkNewInfo.Check_number;
            prmChkToUpdate.Date_written = prmChkNewInfo.Date_written;
            prmChkToUpdate.Date_received = prmChkNewInfo.Date_received;
            prmChkToUpdate.Date_paid = prmChkNewInfo.Date_paid;
            prmChkToUpdate.Amount_paid = prmChkNewInfo.Amount_paid;

            cpdb.SaveChanges();
        }
        public Acct_check InsertAcct_check(Acct_check prmAcctCheck)
        {
            var tstAcct_check = (
                from ac in cpdb.Acct_checks
                where ac.Acct_check_id == prmAcctCheck.Acct_check_id
                select ac
            ).FirstOrDefault();

            if (tstAcct_check == null)
            {
                cpdb.Acct_checks.Add(prmAcctCheck);
                cpdb.SaveChanges();
            }

            return tstAcct_check;
        }
        public void DeleteAcct_check(Acct_check prmAcctCheck)
        {
            cpdb.Acct_checks.Remove((
                from ac in cpdb.Acct_checks
                where ac.Acct_check_id == prmAcctCheck.Acct_check_id
                select ac
            ).FirstOrDefault());

            cpdb.SaveChanges();
        }
    }


    class BankSQLer
    {
        private CheckPlusDB cpdb;
        public BankSQLer(CheckPlusDB in_cpdb) { cpdb = in_cpdb; }
        public Bank BuildBank(string prmRoutNum)
        {
            return (
                from b in cpdb.Banks
                where b.Routing_number == prmRoutNum
                select b
            ).FirstOrDefault();
        }
        public List<Bank> GetAllBanks() { return cpdb.Banks.ToList(); }
        public Bank GetBank(Bank prmBank)
        {
            return (
                from b in cpdb.Banks
                where b.Bank_id == prmBank.Bank_id
                select b
            ).FirstOrDefault();
        }
        public void DeleteBank(Bank prmBank)
        {
            cpdb.Banks.Remove((
                from b in cpdb.Banks
                where b.Bank_id == prmBank.Bank_id
                select b
            ).FirstOrDefault());

            cpdb.SaveChanges();
        }
        public Bank InsertBank(Bank prmBank)
        {
            var tstBank = (
                from b in cpdb.Banks
                where b.Bank_id == prmBank.Bank_id
                select b
            ).FirstOrDefault();

            if (tstBank == null)
            {
                cpdb.Banks.Add(prmBank);
                cpdb.SaveChanges();
            }

            return tstBank;
        }
    }
}
