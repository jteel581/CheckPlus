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
    public partial class ammountLabel : Form
    {
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

        /*  FUNCTION
         *  helpful for displaying success/error messages
         *      throughout the application
         */
        public void DisplayMessageNoResponse(string inHeading, string inMessage)
        {
            MessageBox.Show(inHeading, inMessage, MessageBoxButtons.OK);
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

        //on user click of button, attempt to add an account record
        private void addActButton_Click(object sender, EventArgs e)
        {   //stashing text input from the form into variables
            string firstName = firstNameBox.Text;
            string lastName = lastNameBox.Text;
            string routingNumber = routingBox1.Text;
            string accountNumber = accountBox1.Text;
            string address = stNumBox.Text + " " + stNameBox.Text;
            string city = cityBox.Text;
            string state = stateBox.Text;
            string zip = zipBox.Text;
            string phnNum = phoneNumBox.Text;

            //--------------------------------------------------------
            //start linq testing code chunk
            //--------------------------------------------------------


            //turn on ability to insert an account into the table
            accSQL.TurnOnInsert();
            //attempt to insert an account record
            Account tstAccount = accSQL.InsertAccount
            (   
                accSQL.BuildAccount
                (   //build an Account object using the information provided
                    firstName, lastName, 
                    routingNumber, 
                    address, city, state, zip, 
                    accountNumber, phnNum
                )
            );
            //turn off ability to insert an account
            accSQL.TurnOffInsert();

            //if attempt to insert account results in finding an exitsting account
            if (tstAccount != null)
            {   //display an error message
                DisplayMessageNoResponse("Error", "Account already exists.");
            }
            else
            {   //otherwise, it was a successful account addition
                ListViewItem lvi = new ListViewItem
                (new string[]
                    {
                        tstAccount.Account_number,
                        tstAccount.First_name, tstAccount.Last_name,
                        accSQL.GetChecksInAccount(tstAccount).Count().ToString("0000"),
                        accSQL.GetAccountBalance(tstAccount).ToString()
                    }
                );
                accountsListView.Items.Add(lvi);
                DisplayMessageNoResponse("Success!", "New account added.");
            }


            //--------------------------------------------------------
            //end linq testing code chunk
            //--------------------------------------------------------
            
            /*
            pseudoAccount act = new pseudoAccount(firstName, lastName, routingNumber, accountNumber, stNumBox.Text, stNameBox.Text, city, state, zip);
            database.addAccount(act);
            ListViewItem lvi = new ListViewItem
            (new string[] 
                {
                    act.getAccountNum(),
                    act.getFirstName(), act.getLastName(),
                    act.getNumOfChecks().ToString("0000"),
                    act.getCurBal().ToString()
                }
            );
            accountsListView.Items.Add(lvi);
            */

            //clear input boxes
            firstNameBox.Clear();
            lastNameBox.Clear();
            routingBox1.Clear();
            accountBox1.Clear();
            stNumBox.Clear();
            stNameBox.Clear();
            cityBox.Clear();
            stateBox.Clear();
            zipBox.Clear();
        }


        //on user click of button, attempt to add a check record
        private void addChkButton_Click(object sender, EventArgs e)
        {
            string acctNum = accountBox2.Text;
            string routNum = routingBox2.Text;
            string chkNum = checkNumBox.Text;
            Decimal ammount = Convert.ToDecimal(ammountBox.Text);
            DateTime dateWritten = Convert.ToDateTime(dateWrittenSelector.Text);

            //--------------------------------------------------------
            //start linq testing code chunk
            //--------------------------------------------------------


            acct_chkSQL.TurnOnInsert();
            Acct_check tstAcct_check = acct_chkSQL
                .InsertAcct_check
                (   
                    acct_chkSQL.BuildAcct_check
                    (
                        acctNum, routNum, 
                        chkNum, ammount, dateWritten
                    )
                );
            acct_chkSQL.TurnOffInsert();

            //if check record already existed
            if (tstAcct_check != null)
            {   //display an error message
                DisplayMessageNoResponse("Error", "Check already exists.");
            }
            else
            {   //otherwise, it was a successful account addition
                //probably want to make this better in future
                    /* return newly generated id?
                     * more careful return after the build thing?
                     * just some considerations.....
                     */
                Acct_check newAcct_check = acct_chkSQL
                    .GetAcct_check
                    (
                        acct_chkSQL.BuildAcct_check
                        (
                            acctNum, routNum,
                            chkNum, ammount, dateWritten
                        )
                    )
                    ;
                ListViewItem lvi = new ListViewItem
                (
                    new string[]
                    {
                        acct_chkSQL.GetAccountNumber(newAcct_check),
                        acct_chkSQL.GetFirstName(newAcct_check),
                        acct_chkSQL.GetLastName(newAcct_check),
                        newAcct_check.Check_number, 
                        newAcct_check.Amount.ToString()
                    }
                )
                ;
                checkListView.Items.Add(lvi);
                DisplayMessageNoResponse("Success!", "New check added.");
            }


            //--------------------------------------------------------
            //end linq testing code chunk
            //--------------------------------------------------------
            
            /*
            pseudoAccount act = database.getAccountByNum(acctNum);
            pseudoCheck check = new pseudoCheck(acctNum, routNum, ammount);
            database.getAccountByNum(acctNum).addCheck(check);
            ListViewItem lvi = new ListViewItem(new string[] { act.getAccountNum(), act.getFirstName(), act.getLastName(), check.getCheckNum().ToString(), check.getAmmount().ToString() });
            checkListView.Items.Add(lvi);
            updateAccountListView();
            updateCheckListView();
            */

            //clear input boxes
            accountBox2.Clear();
            routingBox2.Clear();
            ammountBox.Clear();
        }

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

        public void updateCheckListView()
        {
            if (checkListView.Items.Count == 0)
            {
                ListViewItem lvi;
                /*
                foreach (pseudoAccount act in database.getAccountsList())
                {
                    foreach (pseudoCheck check in act.getChecks())
                    {
                        lvi = new ListViewItem(new string[] { act.getAccountNum(), act.getFirstName(), act.getLastName(), check.getCheckNum().ToString(), check.getAmmount().ToString() });
                        checkListView.Items.Add(lvi);
                    }
                }
                */
                foreach(Acct_check ac in cpdb.Acct_checks)
                {
                    lvi = new ListViewItem
                    (
                        new string[]
                        {
                            acct_chkSQL.GetAccountNumber(ac),
                            acct_chkSQL.GetFirstName(ac),
                            acct_chkSQL.GetLastName(ac),
                            ac.Check_number,
                            ac.Amount.ToString()
                        }
                    );
                    checkListView.Items.Add(lvi);
                }
            }
            else
            {
                foreach (ListViewItem lvi in checkListView.Items)
                {
                    string accountNum = lvi.SubItems[0].Text;
                    string checkNum = lvi.SubItems[3].Text;
                    pseudoAccount act = database.getAccountByNum(accountNum);
                    if (act != null)
                    {
                        pseudoCheck check = act.getCheckByNum(checkNum);
                        if (check != null)
                        {
                            lvi.SubItems[0].Text = check.getAccountNum();
                            lvi.SubItems[1].Text = act.getFirstName();
                            lvi.SubItems[2].Text = act.getLastName();
                            lvi.SubItems[3].Text = check.getCheckNum().ToString();
                            lvi.SubItems[4].Text = check.getAmmount().ToString();

                        }
                        else
                        {
                            lvi.Remove();
                        }
                    }
                    else
                    {
                        lvi.Remove();
                    }
                    
                }
            }
            checkListView.Sort();
        }

        
        //  if      {}  --  when first opening the application, populate with the existing records
        //  else    {}  --  throughout use of the application, update the values displayed
        public void updateAccountListView()
        {
            if (accountsListView.Items.Count == 0)
            {
                ListViewItem lvi;
                /*
                 * foreach (pseudoAccount act in database.getAccountsList())
                {
                    lvi = new ListViewItem(new string[] { act.getAccountNum(), act.getFirstName(), act.getLastName(), act.getNumOfChecks().ToString("0000"), act.getCurBal().ToString() });
                    accountsListView.Items.Add(lvi);
                }
                */
                foreach(Account act in accSQL.GetAllAccounts())
                {
                    lvi = new ListViewItem
                    (
                        new string[] 
                        {
                            act.Account_number,
                            act.First_name, act.Last_name, 
                            accSQL.GetChecksInAccount(act).Count().ToString("0000"),
                            accSQL.GetAccountBalance(act).ToString()
                        }
                    );
                    accountsListView.Items.Add(lvi);
                }
            }
            else
            {
                foreach (ListViewItem lvi in accountsListView.Items)
                {
                    pseudoAccount act = database.getAccountByNum(lvi.SubItems[0].Text);
                    if (act != null)
                    {
                        lvi.SubItems[3].Text = act.getNumOfChecks().ToString();
                        lvi.SubItems[4].Text = act.getCurBal().ToString();
                    }
                    else
                    {
                        lvi.Remove();
                    }
                }
            }
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

        

        private void saveChangesButton_Click(object sender, EventArgs e)
        {
            string accountNum = accountsListView.SelectedItems[0].SubItems[0].Text;

            string firstName = firstNameBox.Text;
            string lastName = lastNameBox.Text;
            string routNum = routingBox1.Text;
            string address = stNumBox.Text + " " + stNameBox.Text;
            string city = cityBox.Text;
            string state = stateBox.Text;
            string zip = zipBox.Text;
            string acctNum = accountBox1.Text;

            var act = database.getAccountByNum(accountNum);

            accSQL.UpdateAccount
            (
                accSQL.BuildAccount
                (
                    act.getFirstName(), act.getLastName(),
                    act.getRoutingNum(),
                    act.getStNum() + " " + act.getStName(), act.getCity(), act.getState(), act.getZip(),
                    act.getAccountNum(), null
                ),
                accSQL.BuildAccount
                (
                    firstName, lastName,
                    routNum,
                    address, city, state, zip,
                    acctNum, null
                )
            )
            ;

            act.setAccountNum(accountBox1.Text);
            act.setFirstName(firstNameBox.Text);
            act.setLastName(lastNameBox.Text);
            act.setRoutingNum(routingBox1.Text);
            act.setAccountNum(accountBox1.Text);
            act.setStNum(stNumBox.Text);
            act.setStName(stNameBox.Text);
            act.setCity(cityBox.Text);
            act.setState(stateBox.Text);
            act.setZip(zipBox.Text);


        }

        private void deleteAccountButton_Click(object sender, EventArgs e)
        {
            string accountNum = accountsListView.SelectedItems[0].SubItems[0].Text;

            var act = database.getAccountByNum(accountNum);
            database.deleteAccount(act);
            updateAccountListView();
            updateCheckListView();
            firstNameBox.Clear();
            lastNameBox.Clear();
            routingBox1.Clear();
            accountBox1.Clear();
            stNumBox.Clear();
            stNameBox.Clear();
            cityBox.Clear();
            stateBox.Clear();
            zipBox.Clear();
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

        

        private void firstNameLabel_Click(object sender, EventArgs e)
        {

        }

        private void stateLabel_Click(object sender, EventArgs e)
        {

        }

        private void cityLabel_Click(object sender, EventArgs e)
        {

        }

        private void stNameLabel_Click(object sender, EventArgs e)
        {

        }

        private void stNumLabel_Click(object sender, EventArgs e)
        {

        }

        private void accountLabel1_Click(object sender, EventArgs e)
        {

        }

        private void lastNameLabel_Click(object sender, EventArgs e)
        {

        }

        private void zipLabel_Click(object sender, EventArgs e)
        {

        }

        private void routingLabel1_Click(object sender, EventArgs e)
        {

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
            if (accountsListView.SelectedItems.Count != 0)
            {
                string accountNum = accountsListView.SelectedItems[0].SubItems[0].Text;
                pseudoAccount act = database.getAccountByNum(accountNum);
                firstNameBox.Text = act.getFirstName();
                lastNameBox.Text = act.getLastName();
                routingBox1.Text = act.getRoutingNum();
                accountBox1.Text = act.getAccountNum();
                stNameBox.Text = act.getStName();
                stNumBox.Text = act.getStNum();
                cityBox.Text = act.getCity();
                stateBox.Text = act.getState();
                zipBox.Text = act.getZip();
            }
            
        }

        private void saveChangesButton_Click_1(object sender, EventArgs e)
        {
            string accountNum = accountsListView.SelectedItems[0].SubItems[0].Text;
            pseudoAccount act = database.getAccountByNum(accountNum);

        }

        private void checkListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkListView.SelectedItems.Count != 0)
            {
                string accountNum = checkListView.SelectedItems[0].SubItems[0].Text;
                pseudoAccount act = database.getAccountByNum(accountNum);
                string checkNum = checkListView.SelectedItems[0].SubItems[3].Text;
                pseudoCheck check = act.getCheckByNum(checkNum);
                fNameBox2.Text = act.getFirstName();
                lNameBox2.Text = act.getLastName();
                routingBox2.Text = act.getRoutingNum();
                accountBox2.Text = act.getAccountNum();
                ammountBox.Text = check.getAccountNum();
                checkNumBox.Text = check.getCheckNum().ToString();
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

        private void manageAccountPage_Click(object sender, EventArgs e)
        {

        }
    }
}
