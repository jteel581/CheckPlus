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


    
    /*  ===================================================================================================================
     *  CLASS - ApplicationHandler
     *  ===================================================================================================================
     *  class to hold all of the other handlers
     *  
     *  this way, we need instantiate only an instance of this class in the application,
     *      and we will have access to all the other handler classes below 
     *      
     *  this class has more general verifications functions as well
     */
    class ApplicationHandler
    {
        AccountHandler AccountHand { get; set; }
        CheckHandler CheckHand { get; set; }
        BankHandler BankHand { get; set; }

        public ApplicationHandler()
        {
            AccountHand = new AccountHandler();
            CheckHand = new CheckHandler();
            BankHand = new BankHandler();
        }


        /*  --------------------------------------------------------
         *  FUNCTION - VerifyIntegerStringInput
         *  --------------------------------------------------------
         *  attempt to convert the string to an integer
         *  
         *  if it fails, return false
         *  else, return true
         */
        public bool VerifyIntegerStringInput(string prmInt)
        {
            try { Convert.ToInt64(prmInt); }
            catch { return false; }
            return true;
        }


        /*  --------------------------------------------------------
         *  FUNCTION - VerifyDecimalTypeStringInput
         *  --------------------------------------------------------
         *  attempt to convert the string to a decimal
         *  
         *  if it fails, return false
         *  else, return true
         */
        public bool VerifyDecimalStringInput(string prmDecimal)
        {
            try { Convert.ToDecimal(prmDecimal); }
            catch { return false; }
            return true;
        }

        public AccountHandler GetAccountHandler() { return AccountHand; }
        public CheckHandler GetCheckHandler() { return CheckHand; }
        public BankHandler GetBankHandler() { return BankHand; }
    }

    
    
    /*  ===================================================================================================================
     *  CLASS - AccountHandler
     *  ===================================================================================================================
     *  class to do all the work of talking to the data access objects 
     *      and all verification regarding account objects:
     *          - data input
     *          - database INSERTs, UPDATEs, and DELETEs
     *          - etc.
     */
    class AccountHandler
    {
        AccountSQLer AccountSQL = DatabaseHandler.Instance.GetAccountSQLer();
        BankSQLer BankSQL = DatabaseHandler.Instance.GetBankSQLer();

        /*  ---------------------------------------------------------------
         *  FUNCTION - BuildAccount
         *  ---------------------------------------------------------------
         *  build a new Account object (not database record)
         *      with the provided information
         *      
         *  if <routNum> does not match an existing bank record,
         *      return null
         *      
         *  not accessible to the outside world
         *  I want to keep the idea of "building an account"
         *      local to this AccountHandler object
         *      so that no one can build one willy nilly
         */
        private Account BuildNewAccount
        (
            string routNum, string acctNum,
            string firstName, string lastName,
            string address, string city, string state, string zip,
            string phone
        )
        {   //first, check to see if there is an existing bank record with the <routNum>
            Bank tstBank = BankSQL.SelectBank(routNum);
            if (tstBank == null)
            {   //if not, return null
                return null;
            }
            else
            {   //otherwise, build a new account object and return it
                return new Account()
                {
                    First_name = firstName,
                    Last_name = lastName,
                    Bank_id = tstBank.Bank_id,
                    Account_number = acctNum,
                    Address = address,
                    City = city,
                    State = state,
                    Zip_code = zip,
                    Country = "United States",
                    Phone_number = phone
                };
            }
        }


        /*  ---------------------------------------------------------------
         *  FUNCTION - VerifyAccountInfoInputWithStringFeedback
         *  ---------------------------------------------------------------
         *  pass in all the text input from the gui when working with accounts
         *      and verify that all of the information is legal information
         */
        public string VerifyAccountInfoInputWithStringFeedback
        (
            string firstName, string lastName, 
            string routNum, string acctNum, 
            string addr, string city, string state, string zip, 
            string phoneNum
        )
        {   //start the string then finish it off in one of these if/else if statements
            string badResponse = "Please enter a";
            if (firstName == "") { badResponse += " First Name."; }
            else if (lastName == "") { badResponse += " Last Name."; }
            else if (routNum == "") { badResponse += " Routing Number."; }
            else if (acctNum == "") { badResponse += "n Account Number."; }
            else if (addr == "") { badResponse += "n Address."; }
            else if (city == "") { badResponse += " City."; }
            else if (state == "") { badResponse += " State."; }
            else if (zip == "") { badResponse += " ZIP code"; }
            else if (phoneNum == "") { badResponse += " Phone Number."; }
            else
            {
                Bank tstBank = BankSQL.SelectBank(routNum);
                if (tstBank == null) { badResponse += " information of an existing Bank."; }
                else
                {   //return an empty string signifying that everything has been verified
                    return "";
                }
            }
            return badResponse;
        }


        /*  ---------------------------------------------------------------
         *  FUNCTION - SelectAllAccounts
         *  ---------------------------------------------------------------
         *  used for retrieving a List of Account objects corresponding to
         *      all account records in the database
         */
        public List<Account> SelectAllAccounts()
        {
            return AccountSQL.GetAllAccounts();
        }


        /*  ---------------------------------------------------------------
         *  FUNCTION - SelectAccount
         *  ---------------------------------------------------------------
         *  used for retrieving an Account record with
         *      <routNum> and <acctNum>
         *      <accountID>
         */
        public Account SelectAccount(string routNum, string acctNum)
        {
            return AccountSQL.SelectAccount(routNum, acctNum);
        }
        public Account SelectAccount(int accountID)
        {
            return AccountSQL.SelectAccount(accountID);
        }


        /*  ---------------------------------------------------------------
         *  FUNCTION - InsertAccount
         *  ---------------------------------------------------------------
         *  insert a new account record
         *      after validating that a record with the <routNum> and <acctNum>
         *      does not already exist
         *      
         *  if there already was a record with those unique identifiers,
         *      return null to show that no account was added
         */
        public Account InsertAccount(string routNum, string acctNum, string firstName, string lastName, string address, string city, string state, string zip, string phone)
        {   //see if an account already exists with <routNum> and <acctNum>
            Account tstAccount = AccountSQL.SelectAccount(routNum, acctNum);

            //if no account exists
            if (tstAccount == null)
            {   //attempt to build account; if the routing number does not have a bank to connect to, it will return null
                Account newAccount = BuildNewAccount(routNum, acctNum, firstName, lastName, address, city, state, zip, phone);

                if (newAccount == null) { return null; }
                else
                {   //insert a new account record with that new information and return the newly created account
                    return AccountSQL.InsertAccount(newAccount);
                }
            }
            //otherwise, return null to signify that no account was inserted
            else { return null; }
        }


        /*  ---------------------------------------------------------------
         *  FUNCTION - UpdateAccount
         *  ---------------------------------------------------------------
         *  update an existing account record
         *      after validating that a record with the <newRoutNum> and <newAcctNum>
         *      does not already exist
         *      
         *  if the new information cannot produce a valid account, return null
         *  otherwise, return the account with its new info
         */
        public Account UpdateAccount(
            string origRoutNum, string origAcctNum,
            string newRoutNum, string newAcctNum, string newFirstName, string newLastName, string newAddress, string newCity, string newState, string newZip, string newPhone)
        {
            Account origAccount = AccountSQL.SelectAccount(origRoutNum, origAcctNum);
            Account dupTestAccount = null;

            //if there is an attempt to change the routing number or account number
            if (origRoutNum != newRoutNum && origAcctNum != newAcctNum)
            {   //then check to see if the new values would correspond to an existing account
                //if they do, dupTestAccount will be set to that account, otherwise, it will remain null
                dupTestAccount = AccountSQL.SelectAccount(newRoutNum, newAcctNum);
            }

            //if there was no account with that routing number and account number
            if (dupTestAccount == null)
            {   //then build an Account object that will have the new information
                Account newInfoAccount = BuildNewAccount(
                    newRoutNum, newAcctNum,
                    newFirstName, newLastName, newAddress, newCity, newState, newZip, newPhone);

                //if we cannot build that object due to bad bank info, return null
                if (newInfoAccount == null) { return null; }
                //otherwise, update the account with the new information and return an Account object with the new info
                else { return AccountSQL.UpdateAccount(origAccount, newInfoAccount); };
            }
            //otherwise, return null to signify failer to update the account
            else { return null; }
        }


        /*  ---------------------------------------------------------------
         *  FUNCTION - DeleteAccount
         *  ---------------------------------------------------------------
         *  delete an existing account record with <routNum> and <acctNum>
         *  if no existing account exists, return a null record
         */
        public Account DeleteAccount(string routNum, string acctNum)
        {
            Account tstAccount = AccountSQL.SelectAccount(routNum, acctNum);

            if (tstAccount != null) { return AccountSQL.DeleteAccount(tstAccount); }
            return null;
        }


        /*  ---------------------------------------------------------------
         *  FUNCTION - GetUnpaidChecksInAccount
         *  ---------------------------------------------------------------
         *  retrieve the checks in <account>
         */
        public List<Acct_check> GetUnpaidChecksInAccount(Account account)
        {
            return AccountSQL.GetChecksInAccount(account);
        }


        /*  ---------------------------------------------------------------
         *  FUNCTION - GetUnpaidChecksInAccount
         *  ---------------------------------------------------------------
         *  retrieve the balance of the checks in the account
         */
        public Decimal GetCurrentBalance(Account account)
        {
            Decimal balance = 0.0M;
            foreach(Acct_check c in GetUnpaidChecksInAccount(account))
            {
                balance += c.Amount;
            }
            return balance;
        }
    }


    /*  ===================================================================================================================
     *  CLASS - CheckHandler
     *  ===================================================================================================================
     *  class to do all the work of talking to the acct_check data access objects 
     *      and all verification regarding account objects:
     *          - data input
     *          - database INSERTs, UPDATEs, and DELETEs
     *          - etc.
     */
    class CheckHandler
    {
        Acct_checkSQLer CheckSQL = DatabaseHandler.Instance.GetAcct_checkSQLer();
        AccountSQLer AccountSQL = DatabaseHandler.Instance.GetAccountSQLer();
        BankSQLer BankSQL = DatabaseHandler.Instance.GetBankSQLer();

        /*  ---------------------------------------------------------------
         *  FUNCTION - BuildAccount
         *  ---------------------------------------------------------------
         *  build a new Account object (not database record)
         *      with the provided information
         *      
         *  if <routNum> does not match an existing bank record,
         *      return null
         *      
         *  not accessible to the outside world
         *  I want to keep the idea of "building an account"
         *      local to this AccountHandler object
         *      so that no one can build one willy nilly
         */
        private Account BuildNewAccount
        (
            string routNum, string acctNum,
            string firstName, string lastName,
            string address, string city, string state, string zip,
            string phone
        )
        {   //first, check to see if there is an existing bank record with the <routNum>
            Bank tstBank = BankSQL.SelectBank(routNum);
            if (tstBank == null)
            {   //if not, return null
                return null;
            }
            else
            {   //otherwise, build a new account object and return it
                return new Account()
                {
                    First_name = firstName,
                    Last_name = lastName,
                    Bank_id = tstBank.Bank_id,
                    Account_number = acctNum,
                    Address = address,
                    City = city,
                    State = state,
                    Zip_code = zip,
                    Country = "United States",
                    Phone_number = phone
                };
            }
        }


        /*  ---------------------------------------------------------------
         *  FUNCTION - VerifyAccountInfoInputWithStringFeedback
         *  ---------------------------------------------------------------
         *  pass in all the text input from the gui when working with accounts
         *      and verify that all of the information is legal information
         */
        public string VerifyAccountInfoInputWithStringFeedback
        (
            string firstName, string lastName,
            string routNum, string acctNum,
            string addr, string city, string state, string zip,
            string phoneNum
        )
        {   //start the string then finish it off in one of these if/else if statements
            string badResponse = "Please enter a";
            if (firstName == "") { badResponse += " First Name."; }
            else if (lastName == "") { badResponse += " Last Name."; }
            else if (routNum == "") { badResponse += " Routing Number."; }
            else if (acctNum == "") { badResponse += "n Account Number."; }
            else if (addr == "") { badResponse += "n Address."; }
            else if (city == "") { badResponse += " City."; }
            else if (state == "") { badResponse += " State."; }
            else if (zip == "") { badResponse += " ZIP code"; }
            else if (phoneNum == "") { badResponse += " Phone Number."; }
            else
            {
                Bank tstBank = BankSQL.SelectBank(routNum);
                if (tstBank == null) { badResponse += " information of an existing Bank."; }
                else
                {   //return an empty string signifying that everything has been verified
                    return "";
                }
            }
            return badResponse;
        }


        /*  ---------------------------------------------------------------
         *  FUNCTION - SelectAllAccounts
         *  ---------------------------------------------------------------
         *  used for retrieving a List of Account objects corresponding to
         *      all account records in the database
         */
        public List<Account> SelectAllAccounts()
        {
            return AccountSQL.GetAllAccounts();
        }


        /*  ---------------------------------------------------------------
         *  FUNCTION - SelectAccount
         *  ---------------------------------------------------------------
         *  used for retrieving an Account record with
         *      <routNum> and <acctNum>
         *      <accountID>
         */
        public Account SelectAccount(string routNum, string acctNum)
        {
            return AccountSQL.SelectAccount(routNum, acctNum);
        }
        public Account SelectAccount(int accountID)
        {
            return AccountSQL.SelectAccount(accountID);
        }


        /*  ---------------------------------------------------------------
         *  FUNCTION - InsertAccount
         *  ---------------------------------------------------------------
         *  insert a new account record
         *      after validating that a record with the <routNum> and <acctNum>
         *      does not already exist
         *      
         *  if there already was a record with those unique identifiers,
         *      return null to show that no account was added
         */
        public Account InsertAccount(string routNum, string acctNum, string firstName, string lastName, string address, string city, string state, string zip, string phone)
        {   //see if an account already exists with <routNum> and <acctNum>
            Account tstAccount = AccountSQL.SelectAccount(routNum, acctNum);

            //if no account exists
            if (tstAccount == null)
            {   //attempt to build account; if the routing number does not have a bank to connect to, it will return null
                Account newAccount = BuildNewAccount(routNum, acctNum, firstName, lastName, address, city, state, zip, phone);

                if (newAccount == null) { return null; }
                else
                {   //insert a new account record with that new information and return the newly created account
                    return AccountSQL.InsertAccount(newAccount);
                }
            }
            //otherwise, return null to signify that no account was inserted
            else { return null; }
        }


        /*  ---------------------------------------------------------------
         *  FUNCTION - UpdateAccount
         *  ---------------------------------------------------------------
         *  update an existing account record
         *      after validating that a record with the <newRoutNum> and <newAcctNum>
         *      does not already exist
         *      
         *  if the new information cannot produce a valid account, return null
         *  otherwise, return the account with its new info
         */
        public Account UpdateAccount(
            string origRoutNum, string origAcctNum,
            string newRoutNum, string newAcctNum, string newFirstName, string newLastName, string newAddress, string newCity, string newState, string newZip, string newPhone)
        {
            Account origAccount = AccountSQL.SelectAccount(origRoutNum, origAcctNum);
            Account dupTestAccount = null;

            //if there is an attempt to change the routing number or account number
            if (origRoutNum != newRoutNum && origAcctNum != newAcctNum)
            {   //then check to see if the new values would correspond to an existing account
                //if they do, dupTestAccount will be set to that account, otherwise, it will remain null
                dupTestAccount = AccountSQL.SelectAccount(newRoutNum, newAcctNum);
            }

            //if there was no account with that routing number and account number
            if (dupTestAccount == null)
            {   //then build an Account object that will have the new information
                Account newInfoAccount = BuildNewAccount(
                    newRoutNum, newAcctNum,
                    newFirstName, newLastName, newAddress, newCity, newState, newZip, newPhone);

                //if we cannot build that object due to bad bank info, return null
                if (newInfoAccount == null) { return null; }
                //otherwise, update the account with the new information and return an Account object with the new info
                else { return AccountSQL.UpdateAccount(origAccount, newInfoAccount); };
            }
            //otherwise, return null to signify failer to update the account
            else { return null; }
        }


        /*  ---------------------------------------------------------------
         *  FUNCTION - DeleteAccount
         *  ---------------------------------------------------------------
         *  delete an existing acct_check record with <routNum> and <acctNum> and <checkNum>
         *  
         *  if no existing acct_check exists, return a null record
         */
        public Acct_check DeleteAccount(string routNum, string acctNum, string checkNum)
        {
            Acct_check tstCheck = CheckSQL.SelectAcct_check(routNum, acctNum, checkNum);

            if (tstCheck != null) { return CheckSQL.DeleteAcct_check(tstCheck); }
            return null;
        }
    }



    /*  ===================================================================================================================
     *  CLASS - BankHandler
     *  ===================================================================================================================
     *  class to do all the work of talking to the bank data access objects 
     *      and all verification regarding account objects:
     *          - data input
     *          - database INSERTs, UPDATEs, and DELETEs
     *          - etc.
     */
    class BankHandler
    {
        BankSQLer BankSQL = DatabaseHandler.Instance.GetBankSQLer();

        /*  ---------------------------------------------------------------
         *  FUNCTION - SelectBank
         *  ---------------------------------------------------------------
         *  used for retrieving a bank record with
         *      <routNum>
         *      <bankID>
         */
        public Bank SelectBank(string routNum)
        {
            return BankSQL.SelectBank(routNum);
        }
        public Bank SelectBank(int bankID)
        {
            return BankSQL.SelectBank(bankID);
        }
        
        public string GetRoutingNumber(int bankID)
        {
            Bank tstBank = BankSQL.SelectBank(bankID);

            if (tstBank == null) { return null; }
            else { return tstBank.Routing_number; }
        }
    }
}
