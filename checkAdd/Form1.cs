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
using checkAdd;

namespace checkPlus
{
    public partial class ammountLabel : Form
    {
        //used for message boxes so that we don't have to type the strings every time....
        private const string error = "Error";
        private const string success = "Success!";
        private const string warning = "Warning";

        //db variables
        CheckPlusDB cpdb;
        AccountSQLer accSQL;
        Acct_checkSQLer acct_chkSQL;

        pseudoDatabase database = new pseudoDatabase();
        UsersCollection uc = new UsersCollection();
        User activeUser = null;
        public ammountLabel()
        {
            InitializeComponent();
            accountsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            tabControl1.Selected += new TabControlEventHandler(TabControl1_Selected);

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

            updateAccountListView();
            updateCheckListView();
            updateUserListView();
            userListView.FullRowSelect = true;
            accountsListView.FullRowSelect = true;
            checkListView.FullRowSelect = true;
        }

        /*  --------------------------------------------------------
         *  FUNCTION - ClearAccountTabTextBoxes
         *  --------------------------------------------------------
         */
        public void ClearAccountTabTextBoxes()
        {
            firstNameBox.Clear();
            lastNameBox.Clear();
            routingBox1.Clear();
            accountBox1.Clear();
            addressBox.Clear();
            cityBox.Clear();
            stateBox.Clear();
            zipBox.Clear();
            phoneNumBox.Clear();
        }

        /*  --------------------------------------------------------
         *  FUNCTION - ClearCheckTabTextBoxes
         *  --------------------------------------------------------
         */
        public void ClearCheckTabTextBoxes()
        {
            accountBox2.Clear();
            routingBox2.Clear();
            checkNumBox.Clear();
            ammountBox.Clear();
        }


        /*  --------------------------------------------------------
         *  FUNCTION - VerifyAccountTextBoxes
         *  --------------------------------------------------------
         */
        public bool VerifyAccountTextBoxes()
        {
            string badResponse = "Please enter a";
            if (firstNameBox.Text == "") { badResponse += " First Name."; }
            else if (lastNameBox.Text == "") { badResponse += " Last Name."; }
            else if (routingBox1.Text == "") { badResponse += " Routing Number."; }
            else if (accountBox1.Text == "") { badResponse += "n Account Number."; }
            else if (addressBox.Text == "") { badResponse += "n Address."; }
            else if (cityBox.Text == "") { badResponse += " City."; }
            else if (stateBox.Text == "") { badResponse += " State."; }
            else if (zipBox.Text == "") { badResponse += " ZIP code"; }
            else if (phoneNumBox.Text == "") { badResponse += " Phone Number."; }
            else { return true; }

            DisplayMessageNoResponse(error, badResponse);
            return false;
        }


        /*  --------------------------------------------------------
         *  FUNCTION - VerifyCheckTextBoxes
         *  --------------------------------------------------------
         */
        public bool VerifyCheckTextBoxes()
        {
            string badResponse = "Please enter a";
            if (routingBox2.Text == "") { badResponse += " Routing Number."; }
            else if (accountBox2.Text == "") { badResponse += "n Account Number."; }
            else if (ammountBox.Text == "") { badResponse += "n Amount."; }
            else if (checkNumBox.Text == "") { badResponse += " Check Number."; }
            else { return true; }

            DisplayMessageNoResponse(error, badResponse);
            return false;
        }

        /*  --------------------------------------------------------
         *  FUNCTION - DisplayMessageNoResponse
         *  --------------------------------------------------------
         *  helpful for displaying success/error messages
         *      throughout the application
         */
        public void DisplayMessageNoResponse(string prmHeading, string prmMessage)
        {
            MessageBox.Show(prmMessage, prmHeading, MessageBoxButtons.OK);
        }


        private void TabControl1_Selected(Object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedIndex != 0)
            {
                if (activeUser == null)
                {
                    string message = "You must be logged in to access this page! Log in now?";
                    string caption = "User not logged in";
                    switch (MessageBox.Show(message, caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Yes:
                            tabControl1.SelectedIndex = 0;
                            break;

                        case DialogResult.No:
                            tabControl1.SelectedIndex = 0;
                            break;

                        case DialogResult.Cancel:
                            tabControl1.SelectedIndex = 0;
                            break;

                    }
                }
            }
        }



        /*  ====================================================================================================================
         *  ====================================================================================================================
         *  FUNCTIONs for INSERTing records into the database
         *  ====================================================================================================================
         *  ====================================================================================================================
         */



        /*  --------------------------------------------------------
         *  FUNCTION - addActButton_Click
         *  --------------------------------------------------------
         *  called when user clicks "Add Account" button
         *  
         *  verifies that information has been entered into the text boxes
         *  
         *  grabs all the information provided in the text boxes,
         *      verifies it,
         *      creates a new record
         */
        //on user click of button, attempt to add an account record
        private void addActButton_Click(object sender, EventArgs e)
        {   
            if (VerifyAccountTextBoxes())
            {   //stashing text input from the form into variables
                string firstName = firstNameBox.Text;
                string lastName = lastNameBox.Text;
                string routingNumber = routingBox1.Text;
                string accountNumber = accountBox1.Text;
                string address = addressBox.Text;
                string city = cityBox.Text;
                string state = stateBox.Text;
                string zip = zipBox.Text;
                string phnNum = phoneNumBox.Text;

                Account dupCheckAcct = accSQL.SelectAccount(accSQL.BuildAccount(routingNumber, accountNumber));

                if (dupCheckAcct != null)
                {   //if account with that combo of rout num and acct num exists already, report dup error
                    DisplayMessageNoResponse(error, "Account already exists.");
                }
                else
                {   //otherwise, insert a new account object and update the listing
                    accSQL.TurnOnInsert();

                    Account insAcct = accSQL.InsertAccount
                    (accSQL.BuildAccount
                        (
                            routingNumber, accountNumber,
                            firstName, lastName,
                            address, city, state, zip,
                            phnNum
                        )
                    );

                    ListViewItem lvi = new ListViewItem
                    (new string[]
                        {   insAcct.Account_number,
                        insAcct.First_name, insAcct.Last_name,
                        accSQL.GetChecksInAccount(insAcct).Count().ToString(),
                        accSQL.GetAccountBalance(insAcct).ToString(),
                        accSQL.GetBankRoutingNumber(insAcct)
                        }
                    );
                    accountsListView.Items.Add(lvi);
                    DisplayMessageNoResponse(success, "New account added.");

                    accSQL.TurnOffInsert();

                    ClearAccountTabTextBoxes();
                }
            }
        }


        /*  --------------------------------------------------------
         *  FUNCTION - addChkButton_Click
         *  --------------------------------------------------------
         *  called when user clicks "Add Check" button
         *  
         *  grabs all the information provided in the text boxes,
         *      verifies it,
         *      creates a new acct_check record
         *  reports an error if the information would've
         *      resulted in a duplicate
         */
        private void addChkButton_Click(object sender, EventArgs e)
        {
            if (VerifyCheckTextBoxes())
            {
                string acctNum = accountBox2.Text;
                string routNum = routingBox2.Text;
                string chkNum = checkNumBox.Text;
                Decimal ammount = Convert.ToDecimal(ammountBox.Text);
                DateTime dateWritten = Convert.ToDateTime(dateWrittenSelector.Text);

                Acct_check dupChkChk = acct_chkSQL.SelectAcct_check(acct_chkSQL.BuildAcct_check(acctNum, routNum, chkNum));

                if (dupChkChk != null)
                {   //check with that acct num, rout num, and check num already exists, so display an error message
                    DisplayMessageNoResponse(error, "Check already exists.");
                }
                else
                {   //otherwise, insert a new check record and update the listing
                    acct_chkSQL.TurnOnInsert();

                    Acct_check insAcct_check = acct_chkSQL
                    .InsertAcct_check
                    (
                        acct_chkSQL.BuildAcct_check
                        (
                            acctNum, routNum,
                            chkNum, ammount, dateWritten
                        )
                    );

                    ListViewItem lvi = new ListViewItem
                    (   new string[]
                        {   acct_chkSQL.GetAccountNumber(insAcct_check),
                            acct_chkSQL.GetFirstName(insAcct_check),
                            acct_chkSQL.GetLastName(insAcct_check),
                            insAcct_check.Check_number,
                            insAcct_check.Amount.ToString()
                        }
                    )
                    ;
                    checkListView.Items.Add(lvi);
                    updateAccountListView();
                    DisplayMessageNoResponse(success, "New check added.");

                    acct_chkSQL.TurnOffInsert();

                    ClearCheckTabTextBoxes();
                }
            }
        }



        /*  ====================================================================================================================
         *  ====================================================================================================================
         *  FUNCTIONs for UPDATEing records in the database
         *  ====================================================================================================================
         *  ====================================================================================================================
         */



        public void updateUserListView()
        {
            if (userListView.Items.Count == 0)
            {
                ListViewItem lvi;
                foreach (User usr in uc.getUsers())
                {
                    string sup = usr.supervisorPrivaleges ? "Granted" : "Not Granted";
                    string admin = usr.adminPrivaleges ? "Granted" : "Not Granted";
                    lvi = new ListViewItem(new string[] { usr.getFirstName(), usr.getLastName(), sup, admin });
                    userListView.Items.Add(lvi);
                    
                }
            }
            else
            {
                foreach (ListViewItem lvi in userListView.Items)
                {
                    string fName = lvi.SubItems[0].ToString();
                    string lName = lvi.SubItems[1].ToString();
                    User usr = uc.getUserByName(fName, lName);
                   
                    
                    if (usr != null)
                    {

                        lvi.SubItems[2].Text = usr.supervisorPrivaleges ? "Granted" : "Not Granted";
                        lvi.SubItems[3].Text = usr.adminPrivaleges ? "Granted" : "Not Granted";

                    }
                    else
                    {
                        lvi.Remove();
                    }
                }
            }
            checkListView.Sort();
        }


        /*  ---------------------------------------------------------
         *  FUNCTION - updateAccountListView
         *  ---------------------------------------------------------
         *  called any time a user clicks a button that makes changes to accounts
         *      check addition
         *      changing account information
         *      deleting an account, etc....
         *  
         *  if      {}  when first opening the application, populate with the existing records
         *  else    {}  throughout use of the application, update the values displayed
         */
        public void updateAccountListView()
        {
            if (accountsListView.Items.Count == 0)
            {
                foreach(Account act in accSQL.GetAllAccounts())
                {
                    string acctNum = act.Account_number;
                    string fName = act.First_name;
                    string lName = act.Last_name;
                    string checkCount = accSQL.GetChecksInAccount(act).Count().ToString();
                    string acctBal = accSQL.GetAccountBalance(act).ToString();
                    string routNum = accSQL.GetBankRoutingNumber(act);
                    
                    ListViewItem lvi = new ListViewItem
                    (
                        new string[]
                        {
                            acctNum, 
                            fName,
                            lName,
                            checkCount,
                            acctBal,
                            routNum
                        }
                    );
                    accountsListView.Items.Add(lvi);
                }
            }
            else
            {   
                foreach (ListViewItem lvi in accountsListView.Items)
                {
                    string routNum = lvi.SubItems[5].Text;
                    string acctNum = lvi.SubItems[0].Text;

                    Account tstAcct = accSQL.SelectAccount(accSQL.BuildAccount(routNum, acctNum));

                    if (tstAcct != null)
                    {   //update all the information to reflect whatever changes were made
                        lvi.SubItems[0].Text = tstAcct.Account_number;
                        lvi.SubItems[1].Text = tstAcct.First_name;
                        lvi.SubItems[2].Text = tstAcct.Last_name;
                        lvi.SubItems[3].Text = accSQL.GetChecksInAccount(tstAcct).Count().ToString();
                        lvi.SubItems[4].Text = accSQL.GetAccountBalance(tstAcct).ToString();
                        lvi.SubItems[5].Text = accSQL.GetBankRoutingNumber(tstAcct);
                    }
                    else { accountsListView.Items.Remove(lvi); }
                }
            }
        }


        public void updateCheckListView()
        {
            if (checkListView.Items.Count == 0)
            {
                foreach (Acct_check ac in cpdb.Acct_checks)
                {
                    ListViewItem lvi = new ListViewItem
                    (
                        new string[]
                        {
                            acct_chkSQL.GetAccountNumber(ac),
                            acct_chkSQL.GetFirstName(ac),
                            acct_chkSQL.GetLastName(ac),
                            ac.Check_number,
                            ac.Amount.ToString(),
                            acct_chkSQL.GetRoutingNumber(ac)
                        }
                    );
                    checkListView.Items.Add(lvi);
                }
            }
            else
            {
                foreach (ListViewItem lvi in checkListView.Items)
                {
                    string acctNum = lvi.SubItems[0].Text;
                    string routNum = lvi.SubItems[5].Text;
                    string checkNum = lvi.SubItems[3].Text;

                    Acct_check tstAcct_check =
                        acct_chkSQL.SelectAcct_check(acct_chkSQL.BuildAcct_check(
                            acctNum, routNum, checkNum));

                    if (tstAcct_check != null)
                    {
                        lvi.SubItems[0].Text = acct_chkSQL.GetAccountNumber(tstAcct_check);
                        lvi.SubItems[1].Text = acct_chkSQL.GetFirstName(tstAcct_check);
                        lvi.SubItems[2].Text = acct_chkSQL.GetLastName(tstAcct_check);
                        lvi.SubItems[3].Text = tstAcct_check.Check_number;
                        lvi.SubItems[4].Text = tstAcct_check.Amount.ToString();
                        lvi.SubItems[5].Text = acct_chkSQL.GetRoutingNumber(tstAcct_check);
                    }
                    else { checkListView.Items.Remove(lvi); }
                }
            }
            checkListView.Sort();
        }


        private void loginButton_Click(object sender, EventArgs e)
        {
            string uName = usernameBox.Text;
            string pWord = passwordBox.Text;

            foreach(User usr in uc.getUsers())
            {
                if (usr.getUserName() == uName && usr.getPassword() == pWord)
                {
                    activeUser = usr;
                    userLabel.Text = activeUser.getFirstName() + " " + activeUser.getLastName();
                    userLabel.ForeColor = Color.Black;
                    if (activeUser.adminPrivaleges)
                    {
                        privilegesLabel2.Text = "Admininstrator Privileges";
                        deleteAccountButton.Enabled = true;
                        deleteCheckButton.Enabled = true;
                        deleteUserButton.Enabled = true;

                    }
                    else if (activeUser.supervisorPrivaleges)
                    {
                        privilegesLabel2.Text = "Supevisor Privileges";
                        deleteAccountButton.Enabled = true;
                        deleteCheckButton.Enabled = true;
                    }
                    else
                    {
                        privilegesLabel2.Text = "User Privileges";
                        deleteCheckButton.Enabled = true;
                    }
                    privilegesLabel2.ForeColor = Color.Black;
                    return;
                }
            }
            // complain that username or password is incorrect
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            activeUser = null;
            userLabel.Text = "Not signed in yet";
            userLabel.ForeColor = Color.DarkRed;
            privilegesLabel2.Text = "Not signed in yet";
            privilegesLabel2.ForeColor = Color.DarkRed;
            deleteUserButton.Enabled = false;
            deleteAccountButton.Enabled = false;
            deleteCheckButton.Enabled = false;
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            accountsListView.SelectedItems.Clear();
            accountsListView.SelectedIndices.Clear();
            if (accountNumSearchBox.Text != null && accountNameSearchBox.Text != null)
            {
                // message box choose one
                // then change rest to else if
            }
            if (accountNumSearchBox != null)
            {
                string sNum = accountNumSearchBox.Text;
                int i = 0;
                foreach (ListViewItem lvi in accountsListView.Items)
                {
                    string actNum = lvi.SubItems[0].Text;
                    if (actNum == sNum)
                    {
                        accountsListView.Items[i].Selected = true;
                        accountsListView.TopItem = accountsListView.Items[i];
                        accountsListView.Select();
                        return;
                    }
                    i++;
                }
            }
            else if (accountNameSearchBox.Text != null)
            {
                string sName = accountNameSearchBox.Text;
                int i = 0;
                foreach (ListViewItem lvi in accountsListView.Items)
                {
                    string fName = lvi.SubItems[1].ToString();
                    string lName = lvi.SubItems[2].ToString();
                    if (fName == sName || lName == sName)
                    {
                        accountsListView.Items[i].Selected = true;
                        accountsListView.TopItem = accountsListView.Items[i];
                        accountsListView.Select();

                        return;
                    }
                    i++;
                }
            }
        }

        /*  ---------------------------------------------------------
         *  FUNCTION - saveChangesButton_Click
         *  ---------------------------------------------------------
         *  called when the user clicks the "Save Changes" button 
         *      on the accounts tab
         *      
         *  verifies the information provided in the text boxes 
         *      
         *  works only when the user has selected only 1 account
         *      otherwise, an error message will occur
         *      
         *  uses the information provided in the listing to populate
         *      the text boxes
         *      
         *  the user can then change the text in the text boxes
         */
        private void saveChangesButton_Click(object sender, EventArgs e)
        {
            if (VerifyAccountTextBoxes())
            {
                //did the user select only one account from the listing?
                if (accountsListView.SelectedItems.Count == 1)
                {   //get the index of the item being highlighted
                    //this highlighting disappears once the user clicks "Save Changes"
                    //  so we need to preserve the index
                    int acctListItemInd = accountsListView.FocusedItem.Index;

                    //get the original data
                    string origAccountNum = accountsListView.Items[acctListItemInd].SubItems[0].Text;
                    string origRoutNum = accountsListView.Items[acctListItemInd].SubItems[5].Text;

                    //get all the new information (not all of it has to have been changed,
                    //  but it may have been, so we want to grab ALL the things
                    string newFirstName = firstNameBox.Text;
                    string newLastName = lastNameBox.Text;
                    string newRoutNum = routingBox1.Text;
                    string newAddress = addressBox.Text;
                    string newCity = cityBox.Text;
                    string newState = stateBox.Text;
                    string newZip = zipBox.Text;
                    string newAcctNum = accountBox1.Text;
                    string newPhoneNum = phoneNumBox.Text;

                    //get the Account object that houses the unique pieces of the original
                    Account acctToUpd = accSQL.SelectAccount(accSQL.BuildAccount(origRoutNum, origAccountNum));
                    Account acctNewInfo = accSQL.BuildAccount(
                        newRoutNum, newAcctNum,
                        newFirstName, newLastName,
                        newAddress,
                        newCity, newState, newZip,
                        newPhoneNum
                    );

                    //see if an account with the updated information already exists
                    Account tstDupAcct = accSQL.SelectAccount(acctNewInfo);
                    if (tstDupAcct != null)
                    {   //if it does, display an error message and do nothing with the changes
                        DisplayMessageNoResponse(error, "Account with that information already exists.");
                    }
                    else
                    {   //otherwise, update database account record and update display listing
                        accSQL.UpdateAccount(acctToUpd, acctNewInfo);
                        updateAccountListView();
                        updateCheckListView();
                    }
                }
                else { DisplayMessageNoResponse(error, "Please select 1 account."); }
            }
        }


        /*  ---------------------------------------------------------
         *  FUNCTION - updateCheckButton_Click
         *  ---------------------------------------------------------
         *  called when the user clicks the "Save Changes" button
         *  works only when the user has selected only 1 check
         *      otherwise, an error message will occur
         *      
         *  uses the information provided in the listing to populate
         *      the text boxes
         *      
         *  the user can then change the text in the text boxes
         */
        private void updateCheckButton_Click(object sender, EventArgs e)
        {
            if (VerifyCheckTextBoxes())
            {
                //did the user select only one check from the listing?
                if (checkListView.SelectedItems.Count == 1)
                {   //get the index of the item being highlighted
                    //this highlighting disappears once the user clicks "Save Changes"
                    //  so we need to preserve the index
                    int chckListItemInd = checkListView.FocusedItem.Index;

                    //get the original data
                    string origAccountNum = checkListView.Items[chckListItemInd].SubItems[0].Text;
                    string origRoutNum = checkListView.Items[chckListItemInd].SubItems[5].Text;
                    string origCheckNum = checkListView.Items[chckListItemInd].SubItems[3].Text;

                    //get all the new information (not all of it has to have been changed,
                    //  but it may have been, so we want to grab ALL the things)
                    string newRoutNum = routingBox2.Text;
                    string newAcctNum = accountBox2.Text;
                    Decimal newCheckAmt = Convert.ToDecimal(ammountBox.Text);
                    string newCheckNum = checkNumBox.Text;
                    DateTime newDateWrit = Convert.ToDateTime(dateWrittenSelector.Text);

                    //get the Acct_check object that houses the unique pieces of the original
                    Acct_check acct_chkToUpd = acct_chkSQL.BuildAcct_check(origAccountNum, origRoutNum, origCheckNum);
                    Acct_check acct_chkNewInfo = acct_chkSQL.BuildAcct_check
                    (newAcctNum, newRoutNum, newCheckNum,
                        newCheckAmt, newDateWrit
                    );

                    //see if an acct_check with the updated information already exists
                    Acct_check tstDupAcct_chk = acct_chkSQL.SelectAcct_check(acct_chkNewInfo);

                    if (tstDupAcct_chk != null)
                    {   //if it does, display an error message and do nothing with the changes
                        DisplayMessageNoResponse(error, "Check with that information already exists.");
                    }
                    else
                    {   //update database acct_check record and update display listing
                        acct_chkSQL.UpdateAcct_check(acct_chkToUpd, acct_chkNewInfo);
                        updateAccountListView();
                        updateCheckListView();
                    }
                }
                else { DisplayMessageNoResponse(error, "Please select 1 check."); }
            }
        }



        /*  ==========================================================================================
         *  ==========================================================================================
         *  FUNCTIONs for DELETEing items in the db
         *  ==========================================================================================
         *  ==========================================================================================
         */



        /*  --------------------------------------------------------
         *  FUNCTION - deleteAccountButton_Click
         *  --------------------------------------------------------
         *  called when user clicks "Delete Account" button
         *  if any items
         */
        private void deleteAccountButton_Click(object sender, EventArgs e)
        {
            if (accountsListView.SelectedItems.Count > 0)
            {   //loop through each item selected and do work
                foreach (ListViewItem lvi in accountsListView.SelectedItems)
                {   //grab the account number and routing number
                    string acctNumSelected = lvi.SubItems[0].Text;
                    string routNumSelected = lvi.SubItems[5].Text;

                    //get the account based on the account number and the routing number
                    Account tstAccount = accSQL.SelectAccount(accSQL.BuildAccount(routNumSelected, acctNumSelected));

                    //grab all the checks that are connected to that account so that we can delete them
                    //we have to delete all of the checks before we delete the account
                        //because referencial integrity
                    List<Acct_check> acctChecks =
                            accSQL.GetChecksInAccount(tstAccount);
                    foreach(Acct_check ac in acctChecks) { acct_chkSQL.DeleteAcct_check(ac); }

                    //now delete the actual account
                    Account delAccount =
                        accSQL.DeleteAccount(tstAccount);
                }

                //update the views
                updateAccountListView();
                updateCheckListView();
            }
            else { DisplayMessageNoResponse(error, "Please select account(s)."); }

            ClearAccountTabTextBoxes();
        }


        /*  ------------------------------------------------
         *  FUNCTION - deleteCheckButton_Click
         *  ------------------------------------------------  
         *  user clicks Delete Check button
         *  based on the values of the fields on the form, 
         * 
         * */
        private void deleteCheckButton_Click(object sender, EventArgs e)
        {
            if (checkListView.SelectedItems.Count > 0)
            {
                string acctNum = accountBox2.Text;
                string routNum = routingBox2.Text;
                string checkNum = checkNumBox.Text;

                Acct_check tstAcct_check =
                    acct_chkSQL.DeleteAcct_check(
                        acct_chkSQL.BuildAcct_check(acctNum, routNum, checkNum));

                if (tstAcct_check != null)
                {
                    updateAccountListView();
                    updateCheckListView();

                    ClearCheckTabTextBoxes();
                }
            }
            else { DisplayMessageNoResponse(error, "Please select check(s)."); }
        }

        private void viewChecksSearchButton_Click(object sender, EventArgs e)
        {
            checkListView.SelectedIndices.Clear();
            checkListView.SelectedItems.Clear();
            if (viewCheckActNumBox.Text != null && viewCheckNameBox.Text != null)
            {
                // message box choose one
                // then change rest to else if
            }
            if (viewCheckActNumBox != null)
            {
                string sNum = viewCheckActNumBox.Text;
                string sCNum = viewCheckNumBox.Text;
                int i = 0;
                foreach (ListViewItem lvi in checkListView.Items)
                {
                    string actNum = lvi.SubItems[0].Text;
                    string cNum = lvi.SubItems[3].Text;
                    if (actNum == sNum && cNum == sCNum)
                    {
                        checkListView.Items[i].Selected = true;
                        checkListView.TopItem = accountsListView.Items[i];
                        checkListView.Select();
                        return;
                    }
                    i++;
                }
            }
            else if (viewCheckNameBox.Text != null)
            {
                string sName = viewCheckActNumBox.Text;
                string sCNum = viewCheckNumBox.Text;

                int i = 0;
                foreach (ListViewItem lvi in accountsListView.Items)
                {
                    string fName = lvi.SubItems[1].ToString();
                    string lName = lvi.SubItems[2].ToString();
                    string cNum = lvi.SubItems[3].Text;

                    if ((fName == sName || lName == sName) && cNum == sCNum)
                    {
                        checkListView.Items[i].Selected = true;
                        checkListView.TopItem = checkListView.Items[i];
                        checkListView.Select();

                        return;
                    }
                    i++;
                }
            }
        }


        private void passwordBox_Enter(object sender, EventArgs e)
        {
            loginButton.PerformClick();
        }

        private void passwordBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loginButton.PerformClick();
            }
        }

        private void accountsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (accountsListView.SelectedItems.Count == 1)
            {   
                string routNum = accountsListView.SelectedItems[0].SubItems[5].Text;
                string accountNum = accountsListView.SelectedItems[0].SubItems[0].Text;

                Account tstAcct = accSQL.BuildAccount(routNum, accountNum);

                firstNameBox.Text = tstAcct.First_name;
                lastNameBox.Text = tstAcct.Last_name;
                routingBox1.Text = accSQL.GetBankRoutingNumber(tstAcct);
                accountBox1.Text = tstAcct.Account_number;
                addressBox.Text = tstAcct.Address;
                cityBox.Text = tstAcct.City;
                stateBox.Text = tstAcct.State;
                zipBox.Text = tstAcct.Zip_code;
                phoneNumBox.Text = tstAcct.Phone_number;
            }
            else { ClearAccountTabTextBoxes(); }
        }

        private void checkListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkListView.SelectedItems.Count == 1)
            {
                string acctNum = checkListView.SelectedItems[0].SubItems[0].Text;
                string routNum = checkListView.SelectedItems[0].SubItems[5].Text;
                string checkNum = checkListView.SelectedItems[0].SubItems[3].Text;

                Acct_check tstAcct_check = acct_chkSQL.SelectAcct_check(acct_chkSQL.BuildAcct_check(acctNum, routNum, checkNum));
                
                routingBox2.Text = acct_chkSQL.GetRoutingNumber(tstAcct_check);
                accountBox2.Text = acct_chkSQL.GetAccountNumber(tstAcct_check);
                ammountBox.Text = tstAcct_check.Amount.ToString();
                checkNumBox.Text = tstAcct_check.Check_number;
                dateWrittenSelector.Text = tstAcct_check.Date_written.ToString();
            }
            else { ClearCheckTabTextBoxes(); }
        }

        private void searchUserButton_Click(object sender, EventArgs e)
        {
            userListView.SelectedIndices.Clear();
            userListView.SelectedItems.Clear();
            if (userFNameBox.Text == "" || userLNameBox.Text == "")
            {
                // complain that it needs more info
            }
            else
            {
                string fName = userFNameBox.Text;
                string lName = userLNameBox.Text;
                int i = 0;
                foreach (ListViewItem lvi in userListView.Items)
                {
                    string userFname = lvi.SubItems[0].Text;
                    string userLname = lvi.SubItems[1].Text;
                    if (userFname == fName && userLname == lName)
                    {
                        userListView.Items[i].Selected = true;
                        userListView.TopItem = userListView.Items[i];
                        userListView.Select();
                        return;
                    }
                    i++;
                }
            }
        }

        private void userListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userListView.SelectedItems.Count != 0)
            {
                string fName = userListView.SelectedItems[0].SubItems[0].Text;
                string lName = userListView.SelectedItems[0].SubItems[1].Text;
                User usr = uc.getUserByName(fName, lName);
                userFirstNameBox.Text = usr.getFirstName();
                userLastNameBox.Text = usr.getLastName();
                userUserNameBox.Text = usr.getUserName();
                if (usr.supervisorPrivaleges)
                {
                    supStatusBox.Checked = true;
                }
                if (usr.adminPrivaleges)
                {
                    adminStatusBox.Checked = true;
                }
            }
        }

        private void saveChangesButton_Click_1(object sender, EventArgs e)
        {
            string accountNum = accountsListView.SelectedItems[0].SubItems[0].Text;
            pseudoAccount act = database.getAccountByNum(accountNum);

        }

        private void manageAccountPage_Click(object sender, EventArgs e) {}
        private void firstNameLabel_Click(object sender, EventArgs e) { }
        private void stateLabel_Click(object sender, EventArgs e) { }
        private void cityLabel_Click(object sender, EventArgs e) { }
        private void stNameLabel_Click(object sender, EventArgs e) { }
        private void stNumLabel_Click(object sender, EventArgs e) { }
        private void accountLabel1_Click(object sender, EventArgs e) { }
        private void lastNameLabel_Click(object sender, EventArgs e) { }
        private void zipLabel_Click(object sender, EventArgs e) { }
        private void routingLabel1_Click(object sender, EventArgs e) { }

        private void unitTestsButton_Click(object sender, EventArgs e)
        {
            TestEntityAccess unitTest = new TestEntityAccess();
            unitTest.runTests();
            unitTestBox.Text = unitTest.getTestStr();
        }
    }
}
