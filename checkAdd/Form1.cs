﻿using System;
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

        ApplicationHandler AppHand = new ApplicationHandler();

        TabPage AccountsPage;
        TabPage ChecksPage;
        TabPage UsersPage;

        Cp_user ActiveUser = null;

        //use this if we get "login as" functionality 
        //Cp_user SecondaryUser = null;

        /*  --------------------------------------------------------
         *  FUNCTION - RemoveAllTabs
         *  --------------------------------------------------------
         *  used to remove all tabs from the application
         *  
         *  this is the initial state of the application
         *      and the state after someone logs out
         */
        public void RemoveAllTabs()
        {
            ApplicationTabs.TabPages.Remove(ManageAccountsPage);
            ApplicationTabs.TabPages.Remove(ManageChecksPage);
            ApplicationTabs.TabPages.Remove(ManageUsersPage);
        }

        public ammountLabel()
        {
            InitializeComponent();
            AccountsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            ApplicationTabs.Selected += new TabControlEventHandler(HomeTabSelected);

            //preserving our tabs in class variables due to subsequent deletion
            AccountsPage = ManageAccountsPage;
            ChecksPage = ManageChecksPage;
            UsersPage = ManageUsersPage;

            RemoveAllTabs();

            usernameBox.Focus();
        }


        /*  --------------------------------------------------------
         *  FUNCTION - AllowFunctionalityBasedOnLoginRole
         *  --------------------------------------------------------
         *  depending on your <role>, you will be able to see only
         *      certain portions of the application
         *  
         *  "A" -- everything
         *  "S" -- everything
         *  "U" -- reports
         */
        public void AllowFunctionalityBasedOnLoginRole(Cp_user user)
        {
            switch (user.User_role_cd)
            {
                case "A":
                    ApplicationTabs.TabPages.Add(AccountsPage);
                    ApplicationTabs.TabPages.Add(ChecksPage);
                    ApplicationTabs.TabPages.Add(UsersPage);
                    break;
                case "S":
                    ApplicationTabs.TabPages.Add(AccountsPage);
                    ApplicationTabs.TabPages.Add(ChecksPage);
                    ApplicationTabs.TabPages.Add(UsersPage);
                    break;
                case "U":
                    if (user.Client_id == null)
                    {
                        ApplicationTabs.TabPages.Add(AccountsPage);
                        ApplicationTabs.TabPages.Add(ChecksPage);
                        DeleteAccountButton.Visible = false;
                        DeleteCheckButton.Visible = false;
                    }
                    break;
                default:
                    break;
            }
        }


        /*  --------------------------------------------------------
         *  FUNCTION - ClearHomeTabBoxes
         *  --------------------------------------------------------
         */
        public void ClearHomeTabTextBoxes()
        {
            usernameBox.Clear();
            passwordBox.Clear();
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
         *  FUNCTION - ClearUserTabTextBoxes
         *  --------------------------------------------------------
         */
        public void ClearUserTabTextBoxes()
        {
            userFirstNameBox.Clear();
            userLastNameBox.Clear();
            usernameBox.Clear();
            UserPasswordBox.Clear();
            supStatusBox.Checked = false;
            adminStatusBox.Checked = false;
            UserClientComboBx.ResetText();
        }


        public void ClearAllListViews()
        {
            AccountsListView.Items.Clear();
            CheckListView.Items.Clear();
            userListView.Items.Clear();
        }


        public bool VerifyIntegerStringInput(string prmInt)
        {
            try { Convert.ToInt64(prmInt); }
            catch { return false; }
            return true;
        }


        /*  --------------------------------------------------------
         *  FUNCTION - VerifyDecimalTypeStringInput
         *  --------------------------------------------------------
         */
        public bool VerifyDecimalStringInput(string prmDec)
        {
            try { Convert.ToDecimal(prmDec); }
            catch { return false; }
            return true;
        }


        public bool VerifyPhoneNum(string prmPhone)
        {
            return true;
        }


        /*  --------------------------------------------------------
         *  FUNCTION - VerifyAccountTextBoxes
         *  --------------------------------------------------------
         *  verify that all text boxes have input in them
         *  and that the input is valid for each box
         */
        public bool VerifyAccountTextBoxes()
        {
            string fName = firstNameBox.Text;
            string lName = lastNameBox.Text;
            string routNum = routingBox1.Text;
            string acctNum = accountBox1.Text;
            string addr = addressBox.Text;
            string city = cityBox.Text;
            string state = stateBox.Text;
            string zip = zipBox.Text;
            string phoneNum = phoneNumBox.Text;

            string response = AppHand.GetAccountHandler()
                .VerifyAccountInfoInputWithStringFeedback(fName, lName, routNum, acctNum, addr, city, state, zip, phoneNum);

            //if there is a response from VerifyAccountInfoInputWithStringFeedback
            if (response != "")
            {   //display an error message and return false
                DisplayMessageNoResponse(error, response);
                return false;
            }   //otherwise, return true
            else { return true; }
        }


        /*  --------------------------------------------------------
         *  FUNCTION - VerifyCheckTextBoxes
         *  --------------------------------------------------------
         *  verifies the input given in the text boxes
         */
        public bool VerifyCheckTextBoxes()
        {
            string routNum = routingBox2.Text;
            string acctNum = accountBox2.Text;
            string amount = ammountBox.Text;
            string checkNum = checkNumBox.Text;

            string response = AppHand.GetCheckHandler().VerifyCheckInfoInputWithStringFeedback(routNum, acctNum, amount, checkNum);

            if (response != "")
            {   //if a response is generated by the verification function, display an error message and return false
                DisplayMessageNoResponse(error, response);
                return false;
            }
            //otherwise, return true
            else { return true; }
        }


        public bool VerifyUserBoxes()
        {
            string firstName = userFirstNameBox.Text;
            string lastName = userLastNameBox.Text;
            string username = userUserNameBox.Text;
            return true;
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


        /*  --------------------------------------------------------
         *  FUNCTION - AddAccountToListView
         *  --------------------------------------------------------
         *  adds new account information to the list view in the "Manage Accounts" tab
         */
        public void AddAccountToListView(Account account)
        {
            Bank accountBank = AppHand.GetBankHandler().SelectBank(account.Bank_id);
            ListViewItem lvi = new ListViewItem
            (   
                new string[]
                {
                    account.Account_number,
                    account.First_name, account.Last_name,
                    AppHand.GetAccountHandler().GetUnpaidChecksInAccount(account).Count().ToString(),
                    AppHand.GetAccountHandler().GetCurrentBalance(account).ToString(),
                    accountBank.Routing_number
                }
            );
            AccountsListView.Items.Add(lvi);
        }


        /*  --------------------------------------------------------
         *  FUNCTION - AddCheckToListView
         *  --------------------------------------------------------
         *  adds new account information to the list view in the "Manage Accounts" tab
         */
        public void AddCheckToListView(Acct_check check)
        {
            Account checkAccount = AppHand.GetAccountHandler().SelectAccount(check.Account_id);
            Bank checkAccountBank = AppHand.GetBankHandler().SelectBank(checkAccount.Bank_id);
            ListViewItem lvi = new ListViewItem
            (   
                new string[]
                {
                    checkAccount.Account_number,
                    checkAccount.First_name,
                    checkAccount.Last_name,
                    check.Check_number,
                    Convert.ToString(check.Amount),
                    checkAccountBank.Routing_number
                }
            );
            CheckListView.Items.Add(lvi);
            UpdateAccountListView();
        }


        public void AddUserToListView(Cp_user user)
        {
            Cp_user newUser = AppHand.GetUserHandler().SelectUser(user.Cp_user_id);
            ListViewItem lvi = new ListViewItem
            (
                new string[]
                {
                    newUser.First_name,
                    newUser.Last_name,
                    newUser.User_role_cd == "A" || newUser.User_role_cd == "S" ? "Granted" : "Not Granted",
                    newUser.User_role_cd == "A" ? "Granted" : "Not Granted"
                }
            );
            userListView.Items.Add(lvi);
        }

        /*  ====================================================================================================================
         *  ====================================================================================================================
         *  FUNCTIONs related to Users
         *  ====================================================================================================================
         *  ====================================================================================================================
         */


        private void userListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userListView.SelectedItems.Count == 1)
            {
                string fName = userListView.SelectedItems[0].SubItems[0].Text;
                string lName = userListView.SelectedItems[0].SubItems[1].Text;
                Cp_user usr = AppHand.GetUserHandler().SelectUser(userListView.SelectedItems[0].SubItems[2].Text);

                userFirstNameBox.Text = usr.First_name;
                userLastNameBox.Text = usr.Last_name;
                userUserNameBox.Text = usr.Username;
                UserPasswordBox.Text = usr.User_password;
                if (usr.User_role_cd == "S")
                {
                    supStatusBox.Checked = true;
                }
                if (usr.User_role_cd == "A")
                {
                    adminStatusBox.Checked = true;
                }
            }
            else
            {
                ClearUserTabTextBoxes();
            }
        }


        private void HomeTabSelected(Object sender, TabControlEventArgs e)
        {
            if (ApplicationTabs.SelectedIndex != 0)
            {
                if (ActiveUser == null)
                {
                    string message = "You must be logged in to access this page! Log in now?";
                    string caption = "User not logged in";
                    switch (MessageBox.Show(message, caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Yes:
                            ApplicationTabs.SelectedIndex = 0;
                            break;

                        case DialogResult.No:
                            ApplicationTabs.SelectedIndex = 0;
                            break;

                        case DialogResult.Cancel:
                            ApplicationTabs.SelectedIndex = 0;
                            break;
                    }
                }
            }
        }


        /*  ---------------------------------------------------------
         *  FUNCTION - LoginButton_Click
         *  ---------------------------------------------------------
         *  called when the user clicks the Login button on the Home tab
         */
        private void LoginButton_Click(object sender, EventArgs e)
        {
            RemoveAllTabs();

            string uName = usernameBox.Text;
            string pWord = passwordBox.Text;

            foreach (Cp_user usr in AppHand.GetUserHandler().SelectAllUsers())
            {
                if (usr.Username == uName && usr.User_password == pWord)
                {
                    ActiveUser = usr;
                    userLabel.Text = ActiveUser.First_name + " " + ActiveUser.Last_name;
                    userLabel.ForeColor = Color.Black;
                    if (ActiveUser.User_role_cd == "A")
                    {
                        privilegesLabel2.Text = "Admininstrator Privileges";
                        DeleteAccountButton.Enabled = true;
                        DeleteCheckButton.Enabled = true;
                        deleteUserButton.Enabled = true;
                    }
                    else if (ActiveUser.User_role_cd == "S")
                    {
                        privilegesLabel2.Text = "Supevisor Privileges";
                        DeleteAccountButton.Enabled = true;
                        DeleteCheckButton.Enabled = true;
                    }
                    else
                    {
                        privilegesLabel2.Text = "User Privileges";
                        DeleteCheckButton.Enabled = true;
                    }
                    privilegesLabel2.ForeColor = Color.Black;

                    AllowFunctionalityBasedOnLoginRole(ActiveUser);

                    UpdateAccountListView();
                    UpdateCheckListView();
                    UpdateUserListView();

                    userListView.FullRowSelect = true;
                    AccountsListView.FullRowSelect = true;
                    CheckListView.FullRowSelect = true;

                    ClearHomeTabTextBoxes();

                    return;
                }
            }

            ClearHomeTabTextBoxes();
            DisplayMessageNoResponse("Error", "Incorrect username or password. \nPlease try again.");
        }


        /*  ---------------------------------------------------------
         *  FUNCTION - LogoutButton_Click
         *  ---------------------------------------------------------
         *  called when the user clicks the Logout button on the Home tab
         *  makes activeUser null and changes some properties
         */
        private void LogoutButton_Click(object sender, EventArgs e)
        {
            ActiveUser = null;
            userLabel.Text = "Not signed in yet";
            userLabel.ForeColor = Color.DarkRed;
            privilegesLabel2.Text = "Not signed in yet";
            privilegesLabel2.ForeColor = Color.DarkRed;
            deleteUserButton.Enabled = false;
            DeleteAccountButton.Enabled = false;
            DeleteCheckButton.Enabled = false;

            ClearAllListViews();
            RemoveAllTabs();
        }


        /*  ---------------------------------------------------------
         *  FUNCTION - UpdateUserListView
         *  ---------------------------------------------------------
         *  when the application is opened
         */
        public void UpdateUserListView()
        {
            if (userListView.Items.Count == 0)
            {
                ListViewItem lvi;
                foreach(Cp_user usr in AppHand.GetUserHandler().SelectAllUsers())
                {
                    string admin = usr.User_role_cd == "A" ? "Granted" : "Not Granted";
                    string sup = usr.User_role_cd == "S" ? "Granted" : "Not Granted";
                    lvi = new ListViewItem(new string[] { usr.First_name, usr.Last_name, usr.Username, sup, admin });
                    userListView.Items.Add(lvi);
                }
            }
            else
            {
                foreach (ListViewItem lvi in userListView.Items)
                {
                    string fName = lvi.SubItems[0].ToString();
                    string lName = lvi.SubItems[1].ToString();
                    string uname = lvi.SubItems[2].ToString();

                    Cp_user usr = AppHand.GetUserHandler().SelectUser(uname);

                    if (usr != null)
                    {
                        lvi.SubItems[3].Text = usr.User_role_cd == "S" ? "Granted" : "Not Granted";
                        lvi.SubItems[4].Text = usr.User_role_cd == "A" ? "Granted" : "Not Granted";
                    }
                    else
                    {
                        lvi.Remove();
                    }
                }
            }
            userListView.Sort();
        }

        
        private void passwordBox_Enter(object sender, EventArgs e)
        {
            LoginButton.PerformClick();
        }


        private void passwordBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoginButton.PerformClick();
            }
        }


        private void InsertUserButton_Click(object sender, EventArgs e)
        {
            if (VerifyUserBoxes())
            {   //stashing text input from the form into variables
                string firstName = userFirstNameBox.Text;
                string lastName = userLastNameBox.Text;
                string username = userUserNameBox.Text;
                string password = UserPasswordBox.Text;
                string userRole = supStatusBox.Checked ? (adminStatusBox.Checked ? "A" : "S") : "U";
                string clientName = UserClientComboBx.Text;

                //attempt to insert a user with the information provided
                //if it is attempting to insert a duplicate user, userToInsert will be null
                Cp_user userToInsert = AppHand.GetUserHandler().InsertUser(firstName, clientName, lastName, username, password, userRole);

                //if the insert failed due to an existing user, userToInsert will be null
                if (userToInsert == null)
                {   //so display an error message
                    DisplayMessageNoResponse(error, "User already exists.");
                }
                else
                {   //otherwise, update the listing and report a new user added, then clear the text boxes
                    AddUserToListView(userToInsert);
                    DisplayMessageNoResponse(success, "New user added.");
                    ClearUserTabTextBoxes();
                }
            }
        }


        private void UpdateUserButton_Click(object sender, EventArgs e)
        {

        }


        private void DeleteUserButton_Click(object sender, EventArgs e)
        {

        }


        /*  ---------------------------------------------------------
         *  FUNCTION - UpdateAccountListView
         *  ---------------------------------------------------------
         *  called any time a user clicks a button that makes changes to accounts
         *      check addition
         *      changing account information
         *      deleting an account, etc....
         *  
         *  if      {}  when first opening the application, populate with the existing records
         *  else    {}  throughout use of the application, update the values displayed
         */
        public void UpdateAccountListView()
        {
            if (AccountsListView.Items.Count == 0)
            {
                foreach (Account act in AppHand.GetAccountHandler().SelectAllAccounts())
                {
                    string acctNum = act.Account_number;
                    string fName = act.First_name;
                    string lName = act.Last_name;
                    string checkCount = AppHand.GetAccountHandler().GetUnpaidChecksInAccount(act).Count().ToString();
                    string acctBal = AppHand.GetAccountHandler().GetCurrentBalance(act).ToString();
                    string routNum = AppHand.GetBankHandler().GetRoutingNumber(act.Bank_id);

                    AddAccountToListView(act);
                }
            }
            else
            {
                foreach (ListViewItem lvi in AccountsListView.Items)
                {
                    string routNum = lvi.SubItems[5].Text;
                    string acctNum = lvi.SubItems[0].Text;

                    Account tstAcct = AppHand.GetAccountHandler().SelectAccount(routNum, acctNum);

                    if (tstAcct != null)
                    {   //update all the information to reflect whatever changes were made
                        lvi.SubItems[0].Text = tstAcct.Account_number;
                        lvi.SubItems[1].Text = tstAcct.First_name;
                        lvi.SubItems[2].Text = tstAcct.Last_name;
                        lvi.SubItems[3].Text = AppHand.GetAccountHandler().GetUnpaidChecksInAccount(tstAcct).Count().ToString();
                        lvi.SubItems[4].Text = AppHand.GetAccountHandler().GetCurrentBalance(tstAcct).ToString();
                        lvi.SubItems[5].Text = AppHand.GetBankHandler().GetRoutingNumber(tstAcct.Bank_id);
                    }
                    else { AccountsListView.Items.Remove(lvi); }
                }
            }
        }


        private void AccountsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AccountsListView.SelectedItems.Count == 1)
            {
                string routNum = AccountsListView.SelectedItems[0].SubItems[5].Text;
                string accountNum = AccountsListView.SelectedItems[0].SubItems[0].Text;

                Account tstAccount = AppHand.GetAccountHandler().SelectAccount(routNum, accountNum);

                firstNameBox.Text = tstAccount.First_name;
                lastNameBox.Text = tstAccount.Last_name;
                routingBox1.Text = AppHand.GetBankHandler().GetRoutingNumber(tstAccount.Bank_id);
                accountBox1.Text = tstAccount.Account_number;
                addressBox.Text = tstAccount.Address;
                cityBox.Text = tstAccount.City;
                stateBox.Text = tstAccount.State;
                zipBox.Text = tstAccount.Zip_code;
                phoneNumBox.Text = tstAccount.Phone_number;
            }
            else { ClearAccountTabTextBoxes(); }
        }


        private void CheckListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CheckListView.SelectedItems.Count == 1)
            {
                string acctNum = CheckListView.SelectedItems[0].SubItems[0].Text;
                string routNum = CheckListView.SelectedItems[0].SubItems[5].Text;
                string checkNum = CheckListView.SelectedItems[0].SubItems[3].Text;

                Acct_check check = AppHand.GetCheckHandler().SelectCheck(routNum, acctNum, checkNum);
                Account checkAccount = AppHand.GetAccountHandler().SelectAccount(check.Account_id);
                Bank checkAccountBank = AppHand.GetBankHandler().SelectBank(checkAccount.Bank_id);

                routingBox2.Text = checkAccountBank.Routing_number;
                accountBox2.Text = checkAccount.Account_number;
                ammountBox.Text = check.Amount.ToString();
                checkNumBox.Text = check.Check_number;
                dateWrittenSelector.Text = check.Date_written.ToString();
            }
            else { ClearCheckTabTextBoxes(); }
        }


        /*      ===========================================================================================================================================================================
        *   ===============================================================================================================================================================================
        *   ===============================================================================================================================================================================
        *   FUNCTIONs for Account buttons
        *       named for what they are doing in cahoots with the database
        *   ===============================================================================================================================================================================
        *   ===============================================================================================================================================================================
        *       ===========================================================================================================================================================================
        */


        /*  --------------------------------------------------------
         *  FUNCTION - InsertAccountButton_Click
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
        private void InsertAccountButton_Click(object sender, EventArgs e)
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

                //attempt to insert an account with the information provided
                //if it is attempting to insert a duplicate account, accountToInsert will be null
                Account accountToInsert = AppHand.GetAccountHandler()
                    .InsertAccount(routingNumber, accountNumber, firstName, lastName, address, city, state, zip, phnNum);

                //if the insert failed due to an existing account, accountToInsert will be null
                if (accountToInsert == null)
                {   //so display an error message
                    DisplayMessageNoResponse(error, "Account already exists.");
                }
                else
                {   //otherwise, update the listing and report a new account added, then clear the text boxes
                    AddAccountToListView(accountToInsert);
                    DisplayMessageNoResponse(success, "New account added.");
                    ClearAccountTabTextBoxes();
                }
            }
        }


        /*  ---------------------------------------------------------
         *  FUNCTION - UpdateAccountButton_Click
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
        private void UpdateAccountButton_Click(object sender, EventArgs e)
        {   //did the user select only one account from the listing?
            if (AccountsListView.SelectedItems.Count == 1)
            {
                if (VerifyAccountTextBoxes())
                {   //get the index of the item being highlighted
                    //this highlighting disappears once the user clicks "Save Changes"
                    //  so we need to preserve the index
                    int acctListItemInd = AccountsListView.FocusedItem.Index;

                    //get the original data
                    string origAccountNum = AccountsListView.Items[acctListItemInd].SubItems[0].Text;
                    string origRoutNum = AccountsListView.Items[acctListItemInd].SubItems[5].Text;

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

                    //attempt to update the account
                    //if the update is unsuccessful, updatedAccount will be null
                    Account updatedAccount = AppHand.GetAccountHandler()
                        .UpdateAccount(
                            origRoutNum, origAccountNum,
                            newRoutNum, newAcctNum, newFirstName, newLastName, newAddress, newCity, newState, newZip, newPhoneNum);

                    if (updatedAccount == null)
                    {   //if it does, display an error message and do nothing with the changes
                        DisplayMessageNoResponse(error, "Account with that information already exists.");
                    }
                    else
                    {   //otherwise, update database account record and update display listing
                        AccountsListView.Items[acctListItemInd].SubItems[0].Text = updatedAccount.Account_number;
                        AccountsListView.Items[acctListItemInd].SubItems[5].Text = AppHand.GetBankHandler().GetRoutingNumber(updatedAccount.Bank_id);

                        UpdateAccountListView();
                        UpdateCheckListView();
                    }
                }
            }
            else { DisplayMessageNoResponse(error, "Please select 1 account."); }
        }


        /*  --------------------------------------------------------
         *  FUNCTION - DeleteAccountButton_Click
         *  --------------------------------------------------------
         *  called when user clicks "Delete Account" button
         *  if any items
         */
        private void DeleteAccountButton_Click(object sender, EventArgs e)
        {
            if (AccountsListView.SelectedItems.Count > 0)
            {   //loop through each item selected and do work
                foreach (ListViewItem lvi in AccountsListView.SelectedItems)
                {   //grab the account number and routing number
                    string acctNumSelected = lvi.SubItems[0].Text;
                    string routNumSelected = lvi.SubItems[5].Text;

                    //get the account based on the account number and the routing number
                    Account tstAccount = AppHand.GetAccountHandler().SelectAccount(routNumSelected, acctNumSelected);

                    //grab all the checks that are connected to that account so that we can delete them
                    //we have to delete all of the checks before we delete the account
                    //because referencial integrity
                    List<Acct_check> acctChecks =
                            AppHand.GetAccountHandler().GetUnpaidChecksInAccount(tstAccount);
                    foreach (Acct_check ac in acctChecks) { AppHand.GetCheckHandler().DeleteCheck(ac); }

                    //now delete the actual account
                    Account delAccount =
                        AppHand.GetAccountHandler().DeleteAccount(tstAccount);
                }

                //update the views
                UpdateAccountListView();
                UpdateCheckListView();
            }
            else { DisplayMessageNoResponse(error, "Please select account(s)."); }

            ClearAccountTabTextBoxes();
        }


        /*      ===========================================================================================================================================================================
         *   ===============================================================================================================================================================================
         *   ===============================================================================================================================================================================
         *   FUNCTIONs for Check buttons
         *       named for what they are doing in cahoots with the database
         *   ===============================================================================================================================================================================
         *   ===============================================================================================================================================================================
         *       ===========================================================================================================================================================================
         */


        /*  --------------------------------------------------------
         *  FUNCTION - InsertCheckButton_Click
         *  --------------------------------------------------------
         *  called when user clicks "Add Check" button
         *  
         *  grabs all the information provided in the text boxes,
         *      verifies it,
         *      creates a new acct_check record
         *  reports an error if the information would've
         *      resulted in a duplicate
         */
        private void InsertCheckButton_Click(object sender, EventArgs e)
        {
            if (VerifyCheckTextBoxes())
            {
                string acctNum = accountBox2.Text;
                string routNum = routingBox2.Text;
                string checkNum = checkNumBox.Text;
                Decimal amount = Convert.ToDecimal(ammountBox.Text);
                DateTime dateWritten = Convert.ToDateTime(dateWrittenSelector.Text);

                //attempt to insert an account with the information provided
                //if it is attempting to insert a duplicate account, accountToInsert will be null
                Acct_check checkToInsert = AppHand.GetCheckHandler()
                    .InsertCheck(routNum, acctNum, amount, dateWritten, checkNum);

                if (checkToInsert == null)
                {   //check with that acct num, rout num, and check num already exists, so display an error message
                    DisplayMessageNoResponse(error, "Check already exists.");
                }
                else
                {   //otherwise, insert a new check record and update the listing
                    AddCheckToListView(checkToInsert);
                    DisplayMessageNoResponse(success, "New check added.");
                    ClearCheckTabTextBoxes();
                }
            }
        }


        /*  ---------------------------------------------------------
         *  FUNCTION - UpdateCheckButton_Click
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
        private void UpdateCheckButton_Click(object sender, EventArgs e)
        {
            if (CheckListView.SelectedItems.Count == 1)
            {   //did the user select only one check from the listing?
                if (VerifyCheckTextBoxes())
                {   //get the index of the item being highlighted
                    //this highlighting disappears once the user clicks "Save Changes"
                    //  so we need to preserve the index
                    int chckListItemInd = CheckListView.FocusedItem.Index;

                    //get the original data
                    string origAccountNum = CheckListView.Items[chckListItemInd].SubItems[0].Text;
                    string origRoutNum = CheckListView.Items[chckListItemInd].SubItems[5].Text;
                    string origCheckNum = CheckListView.Items[chckListItemInd].SubItems[3].Text;

                    //get all the new information (not all of it has to have been changed,
                    //  but it may have been, so we want to grab ALL the things)
                    string newRoutNum = routingBox2.Text;
                    string newAcctNum = accountBox2.Text;
                    Decimal newCheckAmt = Convert.ToDecimal(ammountBox.Text);
                    string newCheckNum = checkNumBox.Text;
                    DateTime newDateWrit = Convert.ToDateTime(dateWrittenSelector.Text);

                    //attempt to update the check
                    //if the update is unsuccessful, updatedCheck will be null
                    Acct_check updatedCheck = AppHand.GetCheckHandler().UpdateCheck
                    (
                        origRoutNum, origAccountNum, origCheckNum,
                        newRoutNum, newAcctNum, newCheckNum, newCheckAmt, newDateWrit
                    );

                    if (updatedCheck == null)
                    {   //display an error message and do nothing with the changes
                        DisplayMessageNoResponse(error, "Check with that information already exists.");
                    }
                    else
                    {   //update display listing
                        Account updatedCheckAccount = AppHand.GetAccountHandler().SelectAccount(updatedCheck.Account_id);
                        Bank updatedCheckAccountBank = AppHand.GetBankHandler().SelectBank(updatedCheckAccount.Bank_id);
                        CheckListView.Items[chckListItemInd].SubItems[0].Text = updatedCheckAccount.Account_number;
                        CheckListView.Items[chckListItemInd].SubItems[5].Text = updatedCheckAccountBank.Routing_number;
                        CheckListView.Items[chckListItemInd].SubItems[3].Text = updatedCheck.Check_number;

                        UpdateAccountListView();
                        UpdateCheckListView();
                    }
                }
            }
            else { DisplayMessageNoResponse(error, "Please select 1 check."); }
        }


        /*  ------------------------------------------------
         *  FUNCTION - DeleteCheckButton_Click
         *  ------------------------------------------------  
         *  user clicks Delete Check button
         *  based on the values of the fields on the form
         */
        private void DeleteCheckButton_Click(object sender, EventArgs e)
        {
            if (CheckListView.SelectedItems.Count > 0)
            {
                foreach (ListViewItem lvi in CheckListView.SelectedItems)
                {
                    string acctNum = lvi.SubItems[0].Text;
                    string routNum = lvi.SubItems[5].Text;
                    string checkNum = lvi.SubItems[3].Text;

                    Acct_check deletedCheck = AppHand.GetCheckHandler().DeleteCheck(routNum, acctNum, checkNum);
                }
                UpdateAccountListView();
                UpdateCheckListView();

                ClearCheckTabTextBoxes();
            }
            else { DisplayMessageNoResponse(error, "Please select check(s)."); }
        }


        /*  ------------------------------------------------
         *  FUNCTION - UpdateCheckListView
         *  ------------------------------------------------  
         *  update the list view on the "Manage Checks" tab
         */
        public void UpdateCheckListView()
        {
            if (CheckListView.Items.Count == 0)
            {
                foreach (Acct_check ac in AppHand.GetCheckHandler().SelectAllChecks())
                {
                    Account checkAccount = AppHand.GetAccountHandler().SelectAccount(ac.Account_id);
                    Bank checkAccountBank = AppHand.GetBankHandler().SelectBank(checkAccount.Bank_id);

                    ListViewItem lvi = new ListViewItem
                    (
                        new string[]
                        {
                            checkAccount.Account_number,
                            checkAccount.First_name,
                            checkAccount.Last_name,
                            ac.Check_number,
                            ac.Amount.ToString(),
                            checkAccountBank.Routing_number
                        }
                    );
                    CheckListView.Items.Add(lvi);
                }
            }
            else
            {
                foreach (ListViewItem lvi in CheckListView.Items)
                {
                    string acctNum = lvi.SubItems[0].Text;
                    string routNum = lvi.SubItems[5].Text;
                    string checkNum = lvi.SubItems[3].Text;

                    Acct_check check = AppHand.GetCheckHandler().SelectCheck(routNum, acctNum, checkNum);

                    if (check != null)
                    {
                        Account checkAccount = AppHand.GetAccountHandler().SelectAccount(check.Account_id);
                        Bank checkAccountBank = AppHand.GetBankHandler().SelectBank(checkAccount.Bank_id);

                        lvi.SubItems[0].Text = checkAccount.Account_number;
                        lvi.SubItems[1].Text = checkAccount.First_name;
                        lvi.SubItems[2].Text = checkAccount.Last_name;
                        lvi.SubItems[3].Text = check.Check_number;
                        lvi.SubItems[4].Text = check.Amount.ToString();
                        lvi.SubItems[5].Text = checkAccountBank.Routing_number;
                    }
                    else { CheckListView.Items.Remove(lvi); }
                }
            }
            CheckListView.Sort();
        }


        private void searchButton_Click(object sender, EventArgs e)
        {
            AccountsListView.SelectedItems.Clear();
            AccountsListView.SelectedIndices.Clear();
            if (accountNumSearchBox.Text != null && accountNameSearchBox.Text != null)
            {
                // message box choose one
                // then change rest to else if
            }
            if (accountNumSearchBox != null)
            {
                string sNum = accountNumSearchBox.Text;
                int i = 0;
                foreach (ListViewItem lvi in AccountsListView.Items)
                {
                    string actNum = lvi.SubItems[0].Text;
                    if (actNum == sNum)
                    {
                        AccountsListView.Items[i].Selected = true;
                        AccountsListView.TopItem = AccountsListView.Items[i];
                        AccountsListView.Select();
                        return;
                    }
                    i++;
                }
            }
            else if (accountNameSearchBox.Text != null)
            {
                string sName = accountNameSearchBox.Text;
                int i = 0;
                foreach (ListViewItem lvi in AccountsListView.Items)
                {
                    string fName = lvi.SubItems[1].ToString();
                    string lName = lvi.SubItems[2].ToString();
                    if (fName == sName || lName == sName)
                    {
                        AccountsListView.Items[i].Selected = true;
                        AccountsListView.TopItem = AccountsListView.Items[i];
                        AccountsListView.Select();

                        return;
                    }
                    i++;
                }
            }
        }

        


        

        private void viewChecksSearchButton_Click(object sender, EventArgs e)
        {
            CheckListView.SelectedIndices.Clear();
            CheckListView.SelectedItems.Clear();
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
                foreach (ListViewItem lvi in CheckListView.Items)
                {
                    string actNum = lvi.SubItems[0].Text;
                    string cNum = lvi.SubItems[3].Text;
                    if (actNum == sNum && cNum == sCNum)
                    {
                        CheckListView.Items[i].Selected = true;
                        CheckListView.TopItem = AccountsListView.Items[i];
                        CheckListView.Select();
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
                foreach (ListViewItem lvi in AccountsListView.Items)
                {
                    string fName = lvi.SubItems[1].ToString();
                    string lName = lvi.SubItems[2].ToString();
                    string cNum = lvi.SubItems[3].Text;

                    if ((fName == sName || lName == sName) && cNum == sCNum)
                    {
                        CheckListView.Items[i].Selected = true;
                        CheckListView.TopItem = CheckListView.Items[i];
                        CheckListView.Select();

                        return;
                    }
                    i++;
                }
            }
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


        private void unitTestsButton_Click(object sender, EventArgs e)
        {
            TestEntityAccess unitTest = new TestEntityAccess();
            unitTest.RunSQLTests();
            unitTest.RunAccountHandlerTests();
            unitTest.RunCheckHandlerTests();
            unitTestBox.Text = unitTest.getTestStr();
        }
    }
}
