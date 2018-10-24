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

        //
        public void TurnOnInsert()
        {
            cpdb.Database.ExecuteSqlCommand("set identity_insert dbo.account on");
            cpdb.Database.ExecuteSqlCommand("set identity_insert dbo.bank on");
        }
        //
        public void TurnOffInsert()
        {
            cpdb.Database.ExecuteSqlCommand("set identity_insert dbo.account off");
            cpdb.Database.ExecuteSqlCommand("set identity_insert dbo.bank off");
        }

        /*  -----------------------------------------------------
         *  FUNCTION - BuildAccount
         *  -----------------------------------------------------
         *  build an account out of the unique characteristics of an account record:
         *      Bank's routing number
         *      Account's account number
         *  if a record does not exist with the provided information,
         *      return null
         *      
         *  use it to build an Account instance 
         *      any time you need to call one of the "SQL-esque" Account functions below
         *  -----------------------------------------------------      
         */
        public Account BuildAccount
        (
            string prmRoutNum,
            string prmAcctNum, 
            string prmFirstNm = "", string prmLastNm = "",
            string prmAddress = "", string prmCity = "", string prmState = "", string prmZip = "",
            string prmPhnNum = ""
        )
        {
            Account tstAcct = (
                from a in cpdb.Accounts
                join b in cpdb.Banks on a.Bank_id equals b.Bank_id
                where b.Routing_number == prmRoutNum
                    && a.Account_number == prmAcctNum
                select a
            ).FirstOrDefault();

            //there already was an account with that information
            if (tstAcct != null) { return tstAcct; }
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
                    Country = "United States",
                    Account_number = prmAcctNum,
                    Phone_number = null
                };
            }
        }

        /*  -----------------------------------------------------
         *  FUNCTION -- GetAllAccounts
         *  -----------------------------------------------------
         *  returns a list of all accounts in the database
         *  -----------------------------------------------------
         */
        public List<Account> GetAllAccounts()
        {
            return (
                from a in cpdb.Accounts
                select a
            ).ToList();
        }


        /*  -----------------------------------------------------
         *  FUNCTION -- SelectAccount
         *  -----------------------------------------------------
         *  returns Account object corresponding to <prmAcct>
         *  -----------------------------------------------------
         */
        public Account SelectAccount(Account prmAcct)
        {
            return (
                from a in cpdb.Accounts
                where a.Account_id == prmAcct.Account_id
                select a
            ).FirstOrDefault();
        }


        /*  -----------------------------------------------------
         *  FUNCTION -- GetChecksInAccount
         *  -----------------------------------------------------
         *  returns a list of all unpaid checks 
         *  connected to an <prmAcct>
         *  -----------------------------------------------------
         */
        public List<Acct_check> GetChecksInAccount(Account prmAcct)
        {
            return (
                from a in cpdb.Accounts
                join ac in cpdb.Acct_checks on a.Account_id equals ac.Account_id
                where ac.Date_paid == null
                    && a.Account_id == prmAcct.Account_id
                select ac
            ).ToList();
        }


        /*  -----------------------------------------------------
         *  FUNCTION -- GetAccountBalance
         *  -----------------------------------------------------
         *  returns the sum of all unpaid check amounts 
         *  connected to <prmAcct>
         *  -----------------------------------------------------
         */
        public Decimal GetAccountBalance(Account prmAcct)
        {
             var tstBalance = (
                from a in cpdb.Accounts
                join ac in cpdb.Acct_checks on a.Account_id equals ac.Account_id
                where ac.Date_paid == null
                    && a.Account_id == prmAcct.Account_id
                select ac.Amount
            ).ToList();

            if(tstBalance != null) { return tstBalance.Sum(); }
            else { return 0.0M; }
        }


        /*  -----------------------------------------------------
         *  FUNCTION -- GetBankRountingNumber
         *  -----------------------------------------------------
         *  returns the routing number of the bank 
         *  connected to <prmAcct>
         *  -----------------------------------------------------
         */
        public string GetBankRoutingNumber(Account prmAcct)
        {
            return (
                from a in cpdb.Accounts
                join b in cpdb.Banks on a.Bank_id equals b.Bank_id
                where b.Bank_id == prmAcct.Bank_id
                select b.Routing_number
            ).FirstOrDefault();
        }


        /*  -----------------------------------------------------
         *  FUNCTION -- UpdateAccount
         *  -----------------------------------------------------
         *  pass in an Account object <prmAcctToUpdate> to update 
         *      and an Account object <prmNewAcctInfo> with the new updated info
         *  set all <prmAcctToUpdate>'s info to the <prmNewAcctInfo>'s info
         *  -----------------------------------------------------
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


        /*  -----------------------------------------------------
         *  FUNCTION -- InsertAccount
         *  -----------------------------------------------------
         *  attempt to insert a new record <prmAccount> into the database
         *  if the account already exists, return <prmAccount>
         *  else return null
         *  -----------------------------------------------------
         */
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


        /*  -----------------------------------------------------
         *  FUNCTION - DeleteAccount
         *  -----------------------------------------------------
         *  attempts to DELETE from the database
         *      an account record that corresponds with the
         *      <prmAccount> Account object
         *  if an account is found, it will delete the record
         *      and return the object
         *  otherwise, it will return the null object
         * ------------------------------------------------------
         */
        public Account DeleteAccount(Account prmAccount)
        {
            var tstAccount = (
                from a in cpdb.Accounts
                where a.Account_id == prmAccount.Account_id
                select a
            ).FirstOrDefault();

            if (tstAccount != null)
            {
                cpdb.Accounts.Remove(tstAccount);
                cpdb.SaveChanges();
            }
            return tstAccount;
        }
    }


    //================================================================================
    //  CLASS Acct_checkSQLer
    //================================================================================
    /*  houses functions to access, update, insert, and delete dbo.acct_check records
     *   
     */
    class Acct_checkSQLer
    {
        private CheckPlusDB cpdb;
        public Acct_checkSQLer(CheckPlusDB in_cpdb) { cpdb = in_cpdb; }


        public void TurnOnInsert()
        {
            cpdb.Database.ExecuteSqlCommand("set identity_insert dbo.acct_check on");
            cpdb.Database.ExecuteSqlCommand("set identity_insert dbo.account on");
        }
        public void TurnOffInsert()
        {
            cpdb.Database.ExecuteSqlCommand("set identity_insert dbo.acct_check off");
            cpdb.Database.ExecuteSqlCommand("set identity_insert dbo.account off");
        }


        public Acct_check BuildAcct_check
        (
            string prmAcctNum, //account info
            string prmRoutNum, //bank info
            string prmCheckNum, Decimal prmAmt = 0.0M, DateTime prmDateWrit = new DateTime()  //check info
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
                    Check_number = prmCheckNum,
                    Amount = prmAmt,
                    Date_written = prmDateWrit,
                    Date_received = DateTime.Now,
                    //just tacking Client_id here for now until it needs to be implemented correctly
                    Client_id = 100000
                };
            }
        }


        public List<Acct_check> GetAllAcct_checks()
        {
            return (
                from ac in cpdb.Acct_checks
                select ac
            ).ToList();
        }

        /*  -----------------------------------------------------
         *  FUNCTION - SelectAcct_check
         *  -----------------------------------------------------
         *  attempts to SELECT from the database an acct_check
         *      record that corresponds to <prmAcctCheck>
         *  returns null if nothing is found
         * ------------------------------------------------------
         */
        public Acct_check SelectAcct_check(Acct_check prmAcctCheck)
        {
            return (
                from ac in cpdb.Acct_checks
                where ac.Acct_check_id == prmAcctCheck.Acct_check_id
                select ac
            ).FirstOrDefault();
        }


        /*  -----------------------------------------------------
         *  FUNCTION - GetAccountNumber
         *  -----------------------------------------------------
         *  
         * ------------------------------------------------------
         */
        public string GetAccountNumber(Acct_check prmAcctCheck)
        {
            return (
                from ac in cpdb.Acct_checks
                join a in cpdb.Accounts on ac.Account_id equals a.Account_id
                where ac.Acct_check_id == prmAcctCheck.Acct_check_id
                select a.Account_number
            ).FirstOrDefault();
        }


        /*  -----------------------------------------------------
         *  FUNCTION - GetRoutingNumber
         *  -----------------------------------------------------
         *
         * ------------------------------------------------------
         */
        public string GetRoutingNumber(Acct_check prmAcctCheck)
        {
            return (
                from ac in cpdb.Acct_checks
                join a in cpdb.Accounts on ac.Account_id equals a.Account_id
                join b in cpdb.Banks on a.Bank_id equals b.Bank_id
                where ac.Acct_check_id == prmAcctCheck.Acct_check_id
                select b.Routing_number
            ).FirstOrDefault();
        }



        /*  -----------------------------------------------------
         *  FUNCTION - GetFirstName
         *  -----------------------------------------------------
         *  
         * ------------------------------------------------------
         */
        public string GetFirstName(Acct_check prmAcctCheck)
        {
            return (
                from ac in cpdb.Acct_checks
                join a in cpdb.Accounts on ac.Account_id equals a.Account_id
                where ac.Acct_check_id == prmAcctCheck.Acct_check_id
                select a.First_name
            ).FirstOrDefault();
        }



        /*  -----------------------------------------------------
         *  FUNCTION - GetLastName
         *  -----------------------------------------------------
         *  
         * ------------------------------------------------------
         */
        public string GetLastName(Acct_check prmAcctCheck)
        {
            return (
                from ac in cpdb.Acct_checks
                join a in cpdb.Accounts on ac.Account_id equals a.Account_id
                where ac.Acct_check_id == prmAcctCheck.Acct_check_id
                select a.Last_name
            ).FirstOrDefault();
        }

        /*  -----------------------------------------------------
         *  FUNCTION - UpdateAcct_check
         *  -----------------------------------------------------
         *  
         * ------------------------------------------------------
         */
        public void UpdateAcct_check(Acct_check prmChkToUpdate, Acct_check prmChkNewInfo)
        {
            prmChkToUpdate.Acct_check_id = prmChkNewInfo.Acct_check_id;
            prmChkToUpdate.Account_id = prmChkNewInfo.Account_id;
            prmChkToUpdate.Amount = prmChkNewInfo.Amount;
            prmChkToUpdate.Check_number = prmChkNewInfo.Check_number;
            prmChkToUpdate.Date_written = prmChkNewInfo.Date_written;
            prmChkToUpdate.Date_received = prmChkNewInfo.Date_received;
            prmChkToUpdate.Date_paid = prmChkNewInfo.Date_paid;
            prmChkToUpdate.Amount_paid = prmChkNewInfo.Amount_paid;

            cpdb.SaveChanges();
        }

        /*  -----------------------------------------------------
         *  FUNCTION - InsertAcct_check
         *  -----------------------------------------------------
         *  
         * ------------------------------------------------------
         */
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


        /*  -----------------------------------------------------
         *  FUNCTION - DeleteAcct_check
         *  -----------------------------------------------------
         *  attempts to delete a database acct_check record that
         *      corresponds to <prmAcctCheck> Acct_check object
         *  if there was no corresponding record,
         *      return null 
         *      and utilize that knowledge accordingly
         * ------------------------------------------------------
         */
        public Acct_check DeleteAcct_check(Acct_check prmAcctCheck)
        {
            Acct_check tstAcct_check = (
                from ac in cpdb.Acct_checks
                where ac.Acct_check_id == prmAcctCheck.Acct_check_id
                select ac
            ).FirstOrDefault();

            if (tstAcct_check != null)
            {
                cpdb.Acct_checks.Remove(tstAcct_check);
                cpdb.SaveChanges();
            }

            return tstAcct_check;
        }
    }


    //==========================================================================
    //CLASS BankSQLer
    //==========================================================================
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
