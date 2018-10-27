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

        public bool runTests()
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
            Account tstAcct = accSQL.BuildAccount("000123678", "963852741");
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

            // Tests for manage checks
            // Create check test

            // Retrieve check test

            // Edit check test

            // Remove check test
            return true;
        }
        public string getTestStr() { return testString; }
    }
}
