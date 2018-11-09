using checkPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace checkAdd
{
    class TestEntityAccess
    {
        const string nl = "\r\n";

        string TestString;

        ApplicationHandler AppHand = new ApplicationHandler();

        CheckPlusDB cpdb;
        AccountSQLer accSQL;
        Acct_checkSQLer acct_chkSQL;

        public TestEntityAccess()
        {
            TestString = "";
            //configuring db variables
            cpdb = new CheckPlusDB();
            cpdb.Database.Connection.ConnectionString = "" +
                "Data Source=localhost;" +
                "Initial Catalog=CheckPlus;" +
                "Integrated Security=True;" +
                "MultipleActiveResultSets=True"
            ;

            accSQL = new AccountSQLer(cpdb);
            acct_chkSQL = new Acct_checkSQLer(cpdb);
        }



        /*      ===========================================================================================================================================================================
         *   ===============================================================================================================================================================================
         *   ===============================================================================================================================================================================
         *   FUNCTION - RunSQLTests
         *   ===============================================================================================================================================================================
         *   ===============================================================================================================================================================================
         *       ===========================================================================================================================================================================
         *   testing the data access objects in EntityAccess.cs
         */



        public bool RunSQLTests()
        {
            // Tests for manage accounts
            // Create account test and Retrieve account test

            accSQL.TurnOnInsert();

            Account insAcct = accSQL.InsertAccount
                    (accSQL.BuildAccount
                        (
                            "000123678", "963852741",
                            "Jack", "Bauer",
                            "24 International Dr.", "LA", "CA", "90210",
                            "8649999999"
                        )
                    );

            accSQL.TurnOffInsert();
            TestString += "Inserting test account..." + nl;
            Account tstAcct = accSQL.SelectAccount("000123678", "963852741");
            TestString += "Retrieving account information..." + nl;
            Debug.Assert("Jack" == tstAcct.First_name);
            TestString += "First name test passed!" + nl;

            Debug.Assert("Bauer" == tstAcct.Last_name);
            TestString += "Last name test passed!" + nl;

            Debug.Assert("24 International Dr." == tstAcct.Address);
            TestString += "Address test passed!" + nl;

            Debug.Assert("LA" == tstAcct.City);
            TestString += "City test passed!" + nl;

            Debug.Assert("CA" == tstAcct.State);
            TestString += "State test passed!" + nl;

            Debug.Assert("90210" == tstAcct.Zip_code);
            TestString += "Zip test passed!" + nl;



            // Edit account test
            Account acctToUpd = accSQL.SelectAccount(accSQL.BuildAccount("000123678", "963852741"));
            TestString += "Getting account to update..." + nl;

            Debug.Assert(tstAcct.First_name_2 == null);

            Account acctNewInfo = accSQL.BuildAccount(
                "000123678", "963852741",
                acctToUpd.First_name, acctToUpd.Last_name,
                "SafeHouse01",
                acctToUpd.City, acctToUpd.State, acctToUpd.Zip_code,
                "8649999999"
            );
            TestString += "Getting new information to update account with..." + nl;

            accSQL.UpdateAccount(acctToUpd, acctNewInfo);
            TestString += "Updating account..." + nl;

            tstAcct = accSQL.BuildAccount("000123678", "963852741");
            TestString += "Retrieving updated account..." + nl;

            Debug.Assert(tstAcct.Address == "SafeHouse01");
            TestString += "Edit account test passed!" + nl;

            // Remove account test
            accSQL.DeleteAccount(tstAcct);
            Debug.Assert(accSQL.SelectAccount(tstAcct) == null);
            TestString += "Deleted account." + nl;

            // Tests for manage checks
            // Create check test

            // Retrieve check test

            // Edit check test

            // Remove check test
            return true;
        }
        public string getTestStr() { return TestString; }

        public bool RunApplicationHandlerTests()
        {
            TestString += "\r\n---AccountHandler Tests---\r\n" + nl;


            //input verification testing
            Debug.Assert(AppHand.VerifyDecimalStringInput("00354.56"));
            TestString += "Decimal string verified." + nl;

            Debug.Assert(AppHand.VerifyIntegerStringInput("00023455"));
            TestString += "Integer string verified." + nl;

            return true;
        }


        /*      ===========================================================================================================================================================================
         *   ===============================================================================================================================================================================
         *   ===============================================================================================================================================================================
         *   FUNCTION - RunAccountHandlerTests
         *   ===============================================================================================================================================================================
         *   ===============================================================================================================================================================================
         *       ===========================================================================================================================================================================
         *   run the unit tests for class AccountHandler
         */
        public bool RunAccountHandlerTests()
        {
            TestString += "" +
                "" + nl + 
                "======================================" + nl +
                "---AccountHandler Tests---" + nl +
                "======================================" + nl +
                "" + nl;


            //testing both SelectAccount and VerifyAccountInfoInputWithStringFeedback
            Account successSelectAccount = AppHand.GetAccountHandler().SelectAccount("938747658", "6543216456354");
            Debug.Assert(successSelectAccount != null);
            Debug.Assert(AppHand.GetAccountHandler()
                .VerifyAccountInfoInputWithStringFeedback
                (
                    successSelectAccount.First_name, successSelectAccount.Last_name,
                    AppHand.GetBankHandler().GetRoutingNumber(successSelectAccount.Bank_id),
                    successSelectAccount.Account_number,
                    successSelectAccount.Address, "",
                    successSelectAccount.State, successSelectAccount.Zip_code, successSelectAccount.Phone_number
                ) == "Please enter a City."
            );
            TestString += "Success retrieving existing account." + nl;


            //testing:
            //  successful insert
            //  failed insert
            //  insert of second account for same person
            int countOfAccounts = AppHand.GetAccountHandler().SelectAllAccounts().Count();
            Account successInsertAccount = AppHand.GetAccountHandler().InsertAccount
            (
                "000432178", "18593621839",
                "Gertrude", "Burbanks",
                "1401 Pennsylvania Ave.", "Washington, D.C.", "DC", "20012",
                "800-555-1654"
            );
            Debug.Assert(AppHand.GetAccountHandler().SelectAllAccounts().Count() == countOfAccounts + 1);
            TestString += "Successful insertion test." + nl;
            Debug.Assert(successInsertAccount != null);
            TestString += "New account added through handler!" + nl;

            Account failInsertAccount = AppHand.GetAccountHandler().InsertAccount
            (
                "000432178", "18593621839",
                "MaryJoe", "Fields",
                "215 Marrisville Bridge Rd.", "Albany", "New York", "10001",
                "800-555-9875"
            );
            Debug.Assert(AppHand.GetAccountHandler().SelectAllAccounts().Count() == countOfAccounts + 1);
            TestString += "Successful insertion failure." + nl;
            Debug.Assert(failInsertAccount == null);
            TestString += "Caught a potential duplicate account." + nl;

            Account account2ForSamePerson = AppHand.GetAccountHandler().InsertAccount
            (
                "000432178", "2637846783263",
                "Gertrude", "Burbanks",
                "1401 Pennsylvania Ave.", "Washington, D.C.", "DC", "20012",
                "800-555-1654"
            );
            Debug.Assert(AppHand.GetAccountHandler().SelectAllAccounts().Count() == countOfAccounts + 2);
            TestString += "Successful insertion of account of existing person." + nl;
            Debug.Assert(account2ForSamePerson != null);
            TestString += "Added 2nd account for Gertrude." + nl;


            //testing udpates:
            //  valid changes
            //  invalid changes
            //  updating the unique pieces of the account
            Account updatedAccount = AppHand.GetAccountHandler().UpdateAccount
            (
                "000432178", "2637846783263",
                "000432178", "2637846783263",
                "MaryJoe", "Fields",
                "215 Marrisville Bridge Rd.", "Albany", "New York", "10001",
                "800-555-9875"
            );
            Debug.Assert(updatedAccount.First_name == "MaryJoe" && updatedAccount.Last_name == "Fields");
            TestString += "Updated the second account info." + nl;

            Account updatedAccountInvalidBank = AppHand.GetAccountHandler().UpdateAccount
            (
                "000432178", "2637846783263",
                "2834237682", "2637846783263",
                "MaryJoe", "Fields",
                "215 Marrisville Bridge Rd.", "Albany", "New York", "10001",
                "800-555-9875"
            );
            Debug.Assert(updatedAccountInvalidBank == null);
            TestString += "Caught bad bank info." + nl;

            Account updatedAccountValidBank = AppHand.GetAccountHandler().UpdateAccount
            (
                "000432178", "2637846783263",
                "023666278", "2637846783263",
                "MaryJoe", "Fields",
                "215 Marrisville Bridge Rd.", "Albany", "New York", "10001",
                "800-555-9875"
            );
            Debug.Assert(updatedAccountValidBank != null);
            TestString += "Good bank info update." + nl;

            Account updatedAccountChangeAccountNum = AppHand.GetAccountHandler().UpdateAccount
            (
                "000432178", "18593621839",
                "000432178", "82364872363",
                "Gertrude", "Burbanks",
                "1401 Pennsylvania Ave.", "Washington, D.C.", "DC", "20012",
                "800-555-1654"
            );
            Debug.Assert(updatedAccountChangeAccountNum != null && updatedAccountChangeAccountNum.Account_number == "82364872363");
            TestString += "Updated account number." + nl;


            //testing account deletion
            Account failedAccountDeletion = AppHand.GetAccountHandler().DeleteAccount("2039884234", "98569837465389478");
            Debug.Assert(failedAccountDeletion == null);
            TestString += "Successfully caught an attempt to delete a non-existing account." + nl;

            Account deletedAccount = AppHand.GetAccountHandler().DeleteAccount("000432178", "82364872363");
            Debug.Assert(deletedAccount != null);
            TestString += "Successfully deleted account." + nl;

            Account deletedAccount2 = AppHand.GetAccountHandler().DeleteAccount("023666278", "2637846783263");
            Debug.Assert(deletedAccount2 != null);
            TestString += "Successfully deleted first account." + nl;

            return true;
        }

        /*      ===========================================================================================================================================================================
         *   ===============================================================================================================================================================================
         *   ===============================================================================================================================================================================
         *   FUNCTION - RunCheckHandlerTests
         *   ===============================================================================================================================================================================
         *   ===============================================================================================================================================================================
         *       ===========================================================================================================================================================================
         *   run the unit tests for class CheckHandler
         */
        public bool RunCheckHandlerTests()
        {
            TestString += "" + 
                nl +
                "======================================" + nl +
                "---CheckHandler Tests---" + nl +
                "======================================" + nl +
                "" + nl;


            //selecting a check tests
            Acct_check successSelectCheck = AppHand.GetCheckHandler().SelectCheck("342323678", "585879874653132", "000024");
            Debug.Assert(successSelectCheck != null && successSelectCheck.Amount == 1235.67M);
            TestString += "Success selecting a Check." + nl;

            Acct_check failSelectCheck = AppHand.GetCheckHandler().SelectCheck("238746298374", "84756934756", "2387446938746");
            Debug.Assert(failSelectCheck == null);
            TestString += "Success at failing to select a Check." + nl;


            //inserting a check tests
            Acct_check successInsertCheck = AppHand.GetCheckHandler().InsertCheck
            (
                "239898788", "68786765343655", 154.67M, 
                new DateTime(2018, 3, 5), "00045"
            );
            Debug.Assert(successInsertCheck != null);
            TestString += "Success at inserting a check." + nl;

            Acct_check failInsertBecauseExistingCheck = AppHand.GetCheckHandler().InsertCheck
            (
                "239898788", "68786765343655", 154.67M,
                new DateTime(2018, 3, 5), "00045"
            );
            Debug.Assert(failInsertBecauseExistingCheck == null);
            TestString += "Success at failing to insert a check." + nl;


            //updating a check tests
            Acct_check successUpdateCheck = AppHand.GetCheckHandler().UpdateCheck
            (
                "239898788", "68786765343655", "00045",
                "239898788", "68786765343655", "00045",
                155.67M,
                new DateTime(2018, 3, 5)
            );
            Debug.Assert(successUpdateCheck.Amount == 155.67M);
            TestString += "Success at updating a check." + nl;

            Acct_check failUpdateCheck = AppHand.GetCheckHandler().UpdateCheck
            (
                "239898788", "68786765343655", "00045",
                "342323678", "585879874653132", "000024",
                155.67M,
                new DateTime(2018, 3, 5)
            );
            Debug.Assert(failUpdateCheck == null);
            TestString += "Caught an attempt to update one check to an existing check." + nl;
            
            Acct_check successUpdateOfAccountCheck = AppHand.GetCheckHandler().UpdateCheck
            (
                "239898788", "68786765343655", "00045",
                "342323678", "585879874653132", "00026",
                1236.67M,
                new DateTime(2018, 3, 5)
            );
            Debug.Assert(successUpdateOfAccountCheck != null && successUpdateOfAccountCheck.Amount == 1236.67M);
            TestString += "Success at updating a check to a different exising account." + nl;


            //deleting a check tests
            Acct_check successDeleteCheck = AppHand.GetCheckHandler().DeleteCheck("342323678", "585879874653132", "00026");
            Debug.Assert(successDeleteCheck != null);
            TestString += "Success at deleting a check." + nl;

            Acct_check failDeleteCheck = AppHand.GetCheckHandler().DeleteCheck("238742638746", "02387462398746", "3847263947");
            Debug.Assert(failDeleteCheck == null);
            TestString += "Success at failing to delete a check." + nl;
            
            return true;
        }
    }
}