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
        string testString;

        ApplicationHandler AppHand = new ApplicationHandler();

        CheckPlusDB cpdb;
        AccountSQLer accSQL;
        Acct_checkSQLer acct_chkSQL;

        public TestEntityAccess()
        {
            testString = "";
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
            testString += "Inserting test account...\r\n";
            Account tstAcct = accSQL.SelectAccount("000123678", "963852741");
            testString += "Retrieving account information...\r\n";
            Debug.Assert("Jack" == tstAcct.First_name);
            testString += "First name test passed!\r\n";

            Debug.Assert("Bauer" == tstAcct.Last_name);
            testString += "Last name test passed!\r\n";

            Debug.Assert("24 International Dr." == tstAcct.Address);
            testString += "Address test passed!\r\n";

            Debug.Assert("LA" == tstAcct.City);
            testString += "City test passed!\r\n";

            Debug.Assert("CA" == tstAcct.State);
            testString += "State test passed!\r\n";

            Debug.Assert("90210" == tstAcct.Zip_code);
            testString += "Zip test passed!\r\n";



            // Edit account test
            Account acctToUpd = accSQL.SelectAccount(accSQL.BuildAccount("000123678", "963852741"));
            testString += "Getting account to update...\r\n";

            Debug.Assert(tstAcct.First_name_2 == null);

            Account acctNewInfo = accSQL.BuildAccount(
                "000123678", "963852741",
                acctToUpd.First_name, acctToUpd.Last_name,
                "SafeHouse01",
                acctToUpd.City, acctToUpd.State, acctToUpd.Zip_code,
                "8649999999"
            );
            testString += "Getting new information to update account with...\r\n";

            accSQL.UpdateAccount(acctToUpd, acctNewInfo);
            testString += "Updating account...\r\n";

            tstAcct = accSQL.BuildAccount("000123678", "963852741");
            testString += "Retrieving updated account...\r\n";

            Debug.Assert(tstAcct.Address == "SafeHouse01");
            testString += "Edit account test passed!\r\n";

            // Remove account test
            accSQL.DeleteAccount(tstAcct);
            Debug.Assert(accSQL.SelectAccount(tstAcct) == null);
            testString += "Deleted account.\r\n";

            // Tests for manage checks
            // Create check test

            // Retrieve check test

            // Edit check test

            // Remove check test
            return true;
        }
        public string getTestStr() { return testString; }

        public bool RunApplicationHandlerTests()
        {
            testString += "\r\n---AccountHandler Tests---\r\n\r\n";


            //input verification testing
            Debug.Assert(AppHand.VerifyDecimalStringInput("00354.56"));
            testString += "Decimal string verified.\r\n";

            Debug.Assert(AppHand.VerifyIntegerStringInput("00023455"));
            testString += "Integer string verified.\r\n";

            return true;
        }

        public bool RunAccountHandlerTests()
        {
            testString += "\r\n---AccountHandler Tests---\r\n\r\n";


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
            testString += "Success retrieving existing account.\r\n";


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
            testString += "Successful insertion test.\r\n";
            Debug.Assert(successInsertAccount != null);
            testString += "New account added through handler!\r\n";

            Account failInsertAccount = AppHand.GetAccountHandler().InsertAccount
            (
                "000432178", "18593621839",
                "MaryJoe", "Fields",
                "215 Marrisville Bridge Rd.", "Albany", "New York", "10001",
                "800-555-9875"
            );
            Debug.Assert(AppHand.GetAccountHandler().SelectAllAccounts().Count() == countOfAccounts + 1);
            testString += "Successful insertion failure.\r\n";
            Debug.Assert(failInsertAccount == null);
            testString += "Caught a potential duplicate account.\r\n";

            Account account2ForSamePerson = AppHand.GetAccountHandler().InsertAccount
            (
                "000432178", "2637846783263",
                "Gertrude", "Burbanks",
                "1401 Pennsylvania Ave.", "Washington, D.C.", "DC", "20012",
                "800-555-1654"
            );
            Debug.Assert(AppHand.GetAccountHandler().SelectAllAccounts().Count() == countOfAccounts + 2);
            testString += "Successful insertion of account of existing person.\r\n";
            Debug.Assert(account2ForSamePerson != null);
            testString += "Added 2nd account for Gertrude.\r\n";


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
            testString += "Updated the second account info.\r\n";

            Account updatedAccountInvalidBank = AppHand.GetAccountHandler().UpdateAccount
            (
                "000432178", "2637846783263",
                "2834237682", "2637846783263",
                "MaryJoe", "Fields",
                "215 Marrisville Bridge Rd.", "Albany", "New York", "10001",
                "800-555-9875"
            );
            Debug.Assert(updatedAccountInvalidBank == null);
            testString += "Caught bad bank info.\r\n";

            Account updatedAccountValidBank = AppHand.GetAccountHandler().UpdateAccount
            (
                "000432178", "2637846783263",
                "023666278", "2637846783263",
                "MaryJoe", "Fields",
                "215 Marrisville Bridge Rd.", "Albany", "New York", "10001",
                "800-555-9875"
            );
            Debug.Assert(updatedAccountValidBank != null);
            testString += "Good bank info update.\r\n";

            Account updatedAccountChangeAccountNum = AppHand.GetAccountHandler().UpdateAccount
            (
                "000432178", "18593621839",
                "000432178", "82364872363",
                "Gertrude", "Burbanks",
                "1401 Pennsylvania Ave.", "Washington, D.C.", "DC", "20012",
                "800-555-1654"
            );
            Debug.Assert(updatedAccountChangeAccountNum != null && updatedAccountChangeAccountNum.Account_number == "82364872363");
            testString += "Updated account number.\r\n";


            //testing account deletion
            Account failedAccountDeletion = AppHand.GetAccountHandler().DeleteAccount("2039884234", "98569837465389478");
            Debug.Assert(failedAccountDeletion == null);
            testString += "Successfully caught an attempt to delete a non-existing account.\r\n";

            Account deletedAccount = AppHand.GetAccountHandler().DeleteAccount("000432178", "82364872363");
            Debug.Assert(deletedAccount != null);
            testString += "Successfully deleted account.\r\n";

            Account deletedAccount2 = AppHand.GetAccountHandler().DeleteAccount("023666278", "2637846783263");
            Debug.Assert(deletedAccount2 != null);
            testString += "Successfully deleted first account.\r\n";

            return true;
        }
    }
}
