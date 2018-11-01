using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*  
 *  these classes are used by the application to handle validations
 *  both of database access 
 *      so that the data access objects don't have to deal with validations themselves
 */
namespace checkPlus
{
    /*  ===================================================================================================================
     *  CLASS - DatabaseHandler
     *  ===================================================================================================================
     *  singleton class that is used any time a handler object needs to access the database
     *  
     */
    sealed class DatabaseHandler
    {
        private static DatabaseHandler DBInstance = null;
        private static readonly object DBLock = new object();

        CheckPlusDB CPDB;

        AccountSQLer AccSQL;
        Acct_checkSQLer Acct_chkSQL;
        BankSQLer BankSQL;

        private DatabaseHandler()
        {
            CPDB = new CheckPlusDB();
            CPDB.Database.Connection.ConnectionString = "" +
                "Data Source=localhost;" +
                "Initial Catalog=CheckPlus;" +
                "Integrated Security=True;" +
                "MultipleActiveResultSets=True"
            ;

            AccSQL = new AccountSQLer(CPDB);
            Acct_chkSQL = new Acct_checkSQLer(CPDB);
            BankSQL = new BankSQLer(CPDB);
        }

        public static DatabaseHandler Instance
        {
            get
            {
                lock (DBLock)
                {
                    if (DBInstance == null) { DBInstance = new DatabaseHandler(); }
                    return DBInstance;
                }
            }
        }

        public AccountSQLer GetAccountSQLer() { return AccSQL; }
        public Acct_checkSQLer GetAcct_checkSQLer() { return Acct_chkSQL; }
        public BankSQLer GetBankSQLer() { return BankSQL; }
    }


    class InputHandler
    {

    }


    class AccountHandler
    {
        AccountSQLer AccountSQL = DatabaseHandler.Instance.GetAccountSQLer();
        BankSQLer BankSQL = DatabaseHandler.Instance.GetBankSQLer();

        /*  ---------------------------------------------------------------
         *  FUNCTION - BuildAccount
         *  ---------------------------------------------------------------
         *  build a new Account object (not database record)
         *      with the provided information
         */
        private Account BuildNewAccount
        (
            string routNum, string acctNum,
            string firstName, string lastName,
            string address, string city, string state, string zip,
            string phone
        )
        {
            return new Account()
            {
                First_name = firstName,
                Last_name = lastName,
                Bank_id = BankSQL.SelectBank(routNum).Bank_id,
                Account_number = acctNum,
                Address = address,
                City = city,
                State = state,
                Zip_code = zip,
                Country = "United States",
                Phone_number = phone
            };
        }

        /*  ---------------------------------------------------------------
         *  FUNCTION - SelectAccount
         *  ---------------------------------------------------------------
         *  used for retrieving an Account record with
         *      routing number of <routNum> and
         *      account number of <acctNum>
         */
        public Account SelectAccount(string routNum, string acctNum)
        {
            return AccountSQL.SelectAccount(routNum, acctNum);
        }


        /*  ---------------------------------------------------------------
         *  FUNCTION - InsertAccount
         *  ---------------------------------------------------------------
         *  insert a new account record
         *      after validating that a record with the <routNum> and <acctNum>
         *      does not already exist
         */
        public Account InsertAccount(string routNum, string acctNum, string firstName, string lastName, string address, string city, string state, string zip, string phone)
        {   //see if an account already exists with <routNum> and <acctNum>
            Account tstAccount = AccountSQL.SelectAccount(routNum, acctNum);

            //if no account exists
            if (tstAccount == null)
            {   
                //insert a new account record with that new information
                AccountSQL.InsertAccount(BuildNewAccount(routNum, acctNum, firstName, lastName, address, city, state, zip, phone));

                //return the newly created account
                return null;
            }
            //otherwise, return null
            else { return tstAccount; }
        }


        /*  ---------------------------------------------------------------
         *  FUNCTION - UpdateAccount
         *  ---------------------------------------------------------------
         *  update an existing account record
         *      after validating that a record with the <newRoutNum> and <newAcctNum>
         *      does not already exist
         */
        public void UpdateAccount(
            string origRoutNum, string origAcctNum,
            string newRoutNum, string newAcctNum, string newFirstName, string newLastName, string newAddress, string newCity, string newState, string newZip, string newPhone)
        {
            Account origAccount = AccountSQL.SelectAccount(origRoutNum, origAcctNum);
            Account dupTestAccount = AccountSQL.SelectAccount(newRoutNum, newAcctNum);

            if (dupTestAccount == null)
            {
                Account newInfoAccount = BuildNewAccount(
                    newRoutNum, newAcctNum,
                    newFirstName, newLastName, newAddress, newCity, newState, newZip, newPhone);

                AccountSQL.UpdateAccount(origAccount, newInfoAccount);
            }
        }
    }
}
