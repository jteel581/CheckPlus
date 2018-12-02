using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using checkAdd.Properties;

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

        private SettingsPropertyCollection Props = checkAdd.Properties.Settings.Default.Properties;

        private string DBServer, DBTest, DBLogin, DBPW;

        private string CPDBConn;

        CheckPlusDB CPDB;

        AccountSQLer AccSQL;
        Acct_checkSQLer Acct_chkSQL;
        BankSQLer BankSQL;
        UserSQLer UserSQL;
        ClientSQLer ClientSQL;

        private DatabaseHandler()
        {
            DBServer = Props["db_server"].DefaultValue.ToString();
            DBTest = Props["db_test"].DefaultValue.ToString();
            DBLogin = Props["db_login"].DefaultValue.ToString();
            DBPW = Props["db_pw"].DefaultValue.ToString();

            CPDB = new CheckPlusDB();

            /*
            CPDBConn = "" +
                "Data Source=" + DBServer + ";" +
                "Initial Catalog=" + DBTest + ";" +
                "User Id=" + DBLogin + ";" +
                "Password=" + DBPW + ";" +
                "MultipleActiveResultSets=True"
            ;
            */

            CPDBConn = "Data Source=.; Initial Catalog=CheckPlus; Integrated Security=True; MultipleActiveResultSets=True";

            CPDB.Database.Connection.ConnectionString = CPDBConn;

            AccSQL = new AccountSQLer(CPDB);
            Acct_chkSQL = new Acct_checkSQLer(CPDB);
            BankSQL = new BankSQLer(CPDB);
            UserSQL = new UserSQLer(CPDB);
            ClientSQL = new ClientSQLer(CPDB);
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
        public UserSQLer GetUserSQLer() { return UserSQL; }
        public ClientSQLer GetClientSQLer() { return ClientSQL; }
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
        UserHandler UserHand { get; set; }

        public ApplicationHandler()
        {
            AccountHand = new AccountHandler();
            CheckHand = new CheckHandler();
            BankHand = new BankHandler();
            UserHand = new UserHandler();
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


        /*  --------------------------------------------------------
         *  FUNCTIONs for verifying data access object instances
         *      VerifyExistingBank - verifying Banks
         *      VerfiyValidAccount - verifying Accounts
         *      VerifyExistingCheck - verifying Acct_checks
         *  --------------------------------------------------------
         */
        public bool VerifyExistingBank(string routNum)
        {
            Bank tstBank = BankHand.SelectBank(routNum);
            if (tstBank == null) { return false; }
            else { return true; }
        }
        public bool VerifyExistingAccount(string routNum, string acctNum)
        {
            Account tstAccount = AccountHand.SelectAccount(routNum, acctNum);
            if (tstAccount == null) { return false; }
            else { return true; }
        }
        public bool VerifyExistingCheck(string routNum, string acctNum, string checkNum)
        {
            Acct_check tstCheck = CheckHand.SelectCheck(routNum, acctNum, checkNum);
            if (tstCheck == null) { return false; }
            else { return true; }
        }
        public bool VerifyExistingUser(string username)
        {
            Cp_user tstUser = UserHand.SelectUser(username);
            if (tstUser == null) { return false; }
            else { return true; }
        }

        public AccountHandler GetAccountHandler() { return AccountHand; }
        public CheckHandler GetCheckHandler() { return CheckHand; }
        public BankHandler GetBankHandler() { return BankHand; }
        public UserHandler GetUserHandler() { return UserHand; }
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
         *  FUNCTION - BuildNewAccount
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
         *  FUNCTIONs for sundry verifications
         *  ---------------------------------------------------------------
         *  all stolen from ApplicationHandler
         *  probably not the safest way to go about it, 
         *      but this way, there's no code duplication
         */
        public bool VerifyIntegerStringInput(string intString)
        {
            ApplicationHandler appHand = new ApplicationHandler();
            return appHand.VerifyIntegerStringInput(intString);
        }
        public bool VerifyDecimalStringInput(string decString)
        {
            ApplicationHandler appHand = new ApplicationHandler();
            return appHand.VerifyDecimalStringInput(decString);
        }
        public bool VerifyExistingBank(string routNum)
        {
            ApplicationHandler appHand = new ApplicationHandler();
            return appHand.VerifyExistingBank(routNum);
        }
        public bool VerifyExistingAccount(string routNum, string acctNum)
        {
            ApplicationHandler appHand = new ApplicationHandler();
            return appHand.VerifyExistingAccount(routNum, acctNum);
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
            else if (!VerifyIntegerStringInput(phoneNum) && phoneNum.Length >= 9) { badResponse += " a valid Phone Number. At least 9 digits."; }
            else if (!VerifyIntegerStringInput(routNum)) { badResponse += " valid Routing Number."; }
            else if (!VerifyIntegerStringInput(acctNum)) { badResponse += " valid Account Number."; }
            else
            {
                if (!VerifyExistingBank(routNum)) { badResponse += " information of an existing Bank."; }
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
         *  update an existing account record after some validation
         *      
         *  if the new information cannot produce a valid account, return null
         *  otherwise, return the account with its new info
         */
        public Account UpdateAccount(
            string origRoutNum, string origAcctNum,
            string newRoutNum, string newAcctNum, string newFirstName, string newLastName, string newAddress, string newCity, string newState, string newZip, string newPhone)
        {   //if there is an attempt to change the routing number or account number
            if (origRoutNum != newRoutNum || origAcctNum != newAcctNum)
            {   //if there was already an account with that routing number and account number,
                //  return null to signify failure 
                //  since we don't want to update an account's unique info 
                //  to another account's unique info
                if (VerifyExistingAccount(newRoutNum, newAcctNum)) { return null; }
            }
            //otherwise, pull the orignial account and build an Account object that will have the new information
            Account origAccount = AccountSQL.SelectAccount(origRoutNum, origAcctNum);
            Account newInfoAccount = BuildNewAccount(
                newRoutNum, newAcctNum,
                newFirstName, newLastName, newAddress, newCity, newState, newZip, newPhone);

            //if we cannot build that object due to bad bank info, return null
            if (newInfoAccount == null) { return null; }
            //otherwise, update the account with the new information and return an Account object with the new info
            else { return AccountSQL.UpdateAccount(origAccount, newInfoAccount); };
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

            if (VerifyExistingAccount(routNum, acctNum)) { return AccountSQL.DeleteAccount(tstAccount); }
            return null;
        }
        public Account DeleteAccount(Account account)
        {
            Account tstAccount = AccountSQL.SelectAccount(account);

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
         *  FUNCTION - GetCurrentBalance
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


        /*  ---------------------------------------------------------------
         *  FUNCTION - GetAccountNumber
         *  ---------------------------------------------------------------
         *  retrieve the account number of the account
         */
        public string GetAccountNumber(int accountID)
        {
            return AccountSQL.SelectAccount(accountID).Account_number;
        }
        /*  ---------------------------------------------------------------
         *  FUNCTION - GetFirstName
         *  ---------------------------------------------------------------
         *  retrieve the first name of the account
         */
        public string GetFirstName(int accountID)
        {
            return AccountSQL.SelectAccount(accountID).First_name;
        }
        /*  ---------------------------------------------------------------
         *  FUNCTION - GetLastName
         *  ---------------------------------------------------------------
         *  retrieve the last name of the account
         */
        public string GetLastName(int accountID)
        {
            return AccountSQL.SelectAccount(accountID).Last_name;
        }
    }


    /*  ==================================================================================================================================================
     *  ==================================================================================================================================================
     *  CLASS - CheckHandler
     *  ==================================================================================================================================================
     *  ==================================================================================================================================================
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
         *  FUNCTION - BuildNewCheck
         *  ---------------------------------------------------------------
         *  build a new Acct_check object (not database record)
         *      with the provided information
         *      
         *  if <routNum> does not match an existing bank record,
         *      return null
         *  if <routNum> and <acctNum> do not match an existing account record,
         *      return null
         *      
         *  not accessible to the outside world
         *  I want to keep the idea of "building an acct_check"
         *      local to this CheckHandler object
         *      so that no one can build one willy nilly
         */
        private Acct_check BuildNewCheck
        (
            string routNum, string acctNum,
            Decimal amount, DateTime dateWritten,
            string checkNum, DateTime dateReceived
        )
        {   //first, check to see if there is an existing bank record with the <routNum>
            Bank tstBank = BankSQL.SelectBank(routNum);
            //then, check to see if there is an existing account <routNum> and <acctNum>
            Account tstAccount = AccountSQL.SelectAccount(routNum, acctNum);
            if (tstBank == null || tstAccount == null)
            {   //if not, return null
                return null;
            }
            else
            {   //otherwise, build a new acct_check object and return it
                return new Acct_check()
                {
                    Account_id = tstAccount.Account_id,
                    Amount = amount,
                    Date_written = dateWritten,
                    Check_number = checkNum,
                    Date_received = dateReceived,
                    Client_id = 100000
                };
            }
        }

        /*  ---------------------------------------------------------------
         *  FUNCTIONs for sundry verifications
         *  ---------------------------------------------------------------
         *  all stolen from ApplicationHandler
         *  probably not the safest way to go about it, 
         *      but this way, there's no code duplication
         */
        public bool VerifyIntegerStringInput(string intString)
        {
            ApplicationHandler appHand = new ApplicationHandler();
            return appHand.VerifyIntegerStringInput(intString);
        }
        public bool VerifyDecimalStringInput(string decString)
        {
            ApplicationHandler appHand = new ApplicationHandler();
            return appHand.VerifyDecimalStringInput(decString);
        }
        public bool VerifyExistingBank(string routNum)
        {
            ApplicationHandler appHand = new ApplicationHandler();
            return appHand.VerifyExistingBank(routNum);
        }
        public bool VerifyExistingAccount(string routNum, string acctNum)
        {
            ApplicationHandler appHand = new ApplicationHandler();
            return appHand.VerifyExistingAccount(routNum, acctNum);
        }
        public bool VerifyExistingCheck(string routNum, string acctNum, string checkNum)
        {
            ApplicationHandler appHand = new ApplicationHandler();
            return appHand.VerifyExistingCheck(routNum, acctNum, checkNum);
        }

        /*  ---------------------------------------------------------------
         *  FUNCTION - VerifyCheckInfoInputWithStringFeedback
         *  ---------------------------------------------------------------
         *  pass in all the text input from the gui when working with checks
         *      and verify that all of the information is legal information
         */
        public string VerifyCheckInfoInputWithStringFeedback
        (
            string routNum, string acctNum,
            string amount, string checkNum
        )
        {   //start the string then finish it off in one of these if/else if statements
            string badResponse = "Please enter";

            if (routNum == "") { badResponse += " a Routing Number."; }
            else if (acctNum == "") { badResponse += " an Account Number."; }
            else if (amount == "") { badResponse += " an Amount."; }
            else if (checkNum == "") { badResponse += " a Check Number."; }
            else if (!VerifyIntegerStringInput(routNum)) { badResponse += " a valid Routing Number."; }
            else if (!VerifyIntegerStringInput(acctNum)) { badResponse += " a valid Account Number."; }
            else if (!VerifyDecimalStringInput(amount)) { badResponse += " a valid money Amount."; }
            else if (!VerifyIntegerStringInput(checkNum)) { badResponse += " a valid check Number."; }
            else
            {
                if (!VerifyExistingBank(routNum)) { badResponse += " information of an existing Bank."; }
                else if (!VerifyExistingAccount(routNum, acctNum)) { badResponse += " information of an existing Account."; }
                else
                {   //return an empty string signifying that everything has been verified
                    return "";
                }
            }
            return badResponse;
        }


        /*  ---------------------------------------------------------------
         *  FUNCTION - SelectAllChecks
         *  ---------------------------------------------------------------
         *  used for retrieving a List of Acct_check objects corresponding to
         *      all acct_check records in the database
         */
        public List<Acct_check> SelectAllChecks()
        {
            return CheckSQL.GetAllAcct_checks();
        }


        /*  ---------------------------------------------------------------
         *  FUNCTION - SelectCheck
         *  ---------------------------------------------------------------
         *  used for retrieving an Account record with
         *      <routNum> and <acctNum> and <checkNum>
         *      <checkID>
         */
        public Acct_check SelectCheck(string routNum, string acctNum, string checkNum)
        {
            return CheckSQL.SelectAcct_check(routNum, acctNum, checkNum);
        }
        public Acct_check SelectCheck(int checkID)
        {
            return CheckSQL.SelectAcct_check(checkID);
        }


        /*  ---------------------------------------------------------------
         *  FUNCTION - InsertCheck
         *  ---------------------------------------------------------------
         *  insert a new account record
         *      after validating that a record with the <routNum> and <acctNum>
         *      does not already exist
         *      
         *  if there already was a record with those unique identifiers,
         *      return null to show that no account was added
         */
        public Acct_check InsertCheck(string routNum, string acctNum, Decimal amount, DateTime dateWritten, string checkNum)
        {   //if no check already exists
            if (!VerifyExistingCheck(routNum, acctNum, checkNum))
            {   //attempt to build account; if the routing number does not have a bank to connect to, it will return null
                Acct_check newCheck = BuildNewCheck(routNum, acctNum, amount, dateWritten, checkNum, DateTime.Today);

                //if a new Acct_check object could not be built, return null
                if (newCheck == null) { return null; }
                else
                {   //otherwise, insert a new account record with that new information and return the newly created account
                    return CheckSQL.InsertAcct_check(newCheck);
                }
            }
            //otherwise, return null to signify that no check was inserted
            else { return null; }
        }


        /*  ---------------------------------------------------------------
         *  FUNCTION - UpdateCheck
         *  ---------------------------------------------------------------
         *  update an existing check record
         *      
         *  if the new information cannot produce a valid check, return null
         *  otherwise, return the account with its new info
         */
        public Acct_check UpdateCheck(
            string origRoutNum, string origAcctNum, string origCheckNum,
            string newRoutNum, string newAcctNum, string newCheckNum, Decimal newAmount, DateTime newDateWritten)
        {   //if there is an attempt to change the routing number or account number or check number
            if (origRoutNum != newRoutNum || origAcctNum != newAcctNum || origCheckNum != newCheckNum)
            {   //if there was already a check with that routing number and account number and check number
                //  return null to signify failure 
                //  since we don't want to update a check's unique info 
                //  to another check's unique info
                if (VerifyExistingCheck(newRoutNum, newAcctNum, newCheckNum)) { return null; }
            }
            //otherwise, pull the orignial check and build an Acct_check object that will have the new information
            Acct_check origCheck = CheckSQL.SelectAcct_check(origRoutNum, origAcctNum, origCheckNum);
            Acct_check newInfoCheck = BuildNewCheck(newRoutNum, newAcctNum, newAmount, newDateWritten, newCheckNum, origCheck.Date_received);

            //if we cannot build that object due to bad bank or account info, return null
            if (newInfoCheck == null) { return null; }
            //otherwise, update the account with the new information and return an Account object with the new info
            else { return CheckSQL.UpdateAcct_check(origCheck, newInfoCheck); };
        }


        /*  ---------------------------------------------------------------
         *  FUNCTION - DeleteCheck
         *  ---------------------------------------------------------------
         *  delete an existing acct_check record with <routNum> and <acctNum> and <checkNum>
         *  
         *  if no existing acct_check exists, return a null record
         */
        public Acct_check DeleteCheck(string routNum, string acctNum, string checkNum)
        {
            Acct_check tstCheck = CheckSQL.SelectAcct_check(routNum, acctNum, checkNum);

            if (tstCheck != null) { return CheckSQL.DeleteAcct_check(tstCheck); }
            return null;
        }
        public Acct_check DeleteCheck(Acct_check check)
        {
            Acct_check tstCheck = CheckSQL.SelectAcct_check(check);

            if(tstCheck != null) { return CheckSQL.DeleteAcct_check(tstCheck); }
            return null;
        }


        /*  ---------------------------------------------------------------
         *  FUNCTION - GetClient
         *  ---------------------------------------------------------------
         *  return the Client object corresponding to Client_id of <check>
         *  
         *  return null if no check record corresponds to <check>
         *  ---------------------------------------------------------------
         */
        public Client GetClient(Acct_check check)
        {
            Acct_check tstCheck = CheckSQL.SelectAcct_check(check);

            if(tstCheck != null) { return CheckSQL.GetClient(tstCheck); }
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


    /*  ===================================================================================================================
     *  CLASS - UserHandler
     *  ===================================================================================================================
     *  
     */
    class UserHandler
    {
        public bool VerifyExistingUser(string username)
        {
            ApplicationHandler appHand = new ApplicationHandler();
            return appHand.VerifyExistingUser(username);
        }


        UserSQLer UserSQL = DatabaseHandler.Instance.GetUserSQLer();
        ClientSQLer ClientSQL = DatabaseHandler.Instance.GetClientSQLer();


        private Cp_user BuildNewUser(string firstName, string lastName, string clientName, string username, string password, string userRole)
        {
            Cp_user newUser = UserSQL.SelectUser(username);

            if(newUser == null) { return null; }
            else
            {
                Client tstClient = ClientSQL.SelectClient(clientName); 
                return new Cp_user()
                {
                    Client_id = tstClient.Client_id,
                    First_name = firstName,
                    Last_name = lastName,
                    Username = username,
                    User_password = password,
                    User_role_cd = userRole
                };
            }
        }


        public List<Cp_user> SelectAllUsers() { return UserSQL.SelectAllUsers(); }


        /*  ---------------------------------------------------------------
         *  FUNCTION - SelectBank
         *  ---------------------------------------------------------------
         *  used for retrieving a bank record with
         *      <routNum>
         *      <bankID>
         */
        public Cp_user SelectUser(string username)
        {
            return UserSQL.SelectUser(username);
        }
        public Cp_user SelectUser(int userID)
        {
            return UserSQL.SelectUser(userID);
        }

        
        public Cp_user InsertUser(string firstName, string clientName, string lastname, string username, string password, string userRole)
        {
            if (!VerifyExistingUser(username))
            {
                Cp_user newUser = UserSQL.SelectUser(username);
                return UserSQL.InsertUser(newUser);
            }
            else { return null; }
        }


        public Cp_user UpdateUser(string origUsername, 
            string newClientName, string newFirstName, string newLastName, string newUsername, string newPassword, string newUserRole
        )
        {
            if (origUsername != newUsername)
            {   //check to make sure you aren't changing it to an existing username
                if(!VerifyExistingUser(newUsername)) { return null; }
            }
            else
            {
                Cp_user origUser = UserSQL.SelectUser(origUsername);
                Cp_user newUserInfo = BuildNewUser(newFirstName, newLastName, newClientName, newUsername, newPassword, newUserRole);

                Cp_user updatedUser = UserSQL.UpdateUser(origUser, newUserInfo);
                return updatedUser;
            }
            return null;
        }


        public Cp_user DeleteUser(string username)
        {
            if (!VerifyExistingUser(username))
            {
                Cp_user user = UserSQL.SelectUser(username);
                UserSQL.DeleteUser(user);

                return user;
            }
            else { return null; }
        }


        public Client GetClient(int clientID)
        {
            return ClientSQL.SelectClient(clientID);
        }
    }
}