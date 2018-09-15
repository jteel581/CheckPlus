using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace checkAdd
{
    public partial class ammountLabel : Form
    {

        psudoDatabase database = new psudoDatabase();
        public ammountLabel()
        {
            InitializeComponent();
            accountsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }


        private void addActButton_Click(object sender, EventArgs e)
        {
            string firstName = firstNameBox.Text;
            string lastName = lastNameBox.Text;
            int routingNumber = Convert.ToInt32(routingBox1.Text);
            int accountNumber = Convert.ToInt32(accountBox1.Text);
            psudoAccount act = new psudoAccount(firstName, lastName, routingNumber, accountNumber);
            database.addAccount(act);
            ListViewItem lvi = new ListViewItem(new string[] { act.getAccountNum().ToString("0000"), act.getFirstName(), act.getLastName(), act.getNumOfChecks().ToString("0000"), act.getCurBal().ToString() });
            accountsListView.Items.Add(lvi);

        }

        private void addChkButton_Click(object sender, EventArgs e)
        {
            int acctNum = Convert.ToInt32(accountBox2.Text);
            int routNum = Convert.ToInt32(routingBox2.Text);
            double ammount = Convert.ToDouble(ammountBox.Text);
            psudoCheck check = new psudoCheck(acctNum, routNum, ammount);
            database.getAccountByNum(acctNum).addCheck(check);
            updateListView();
            
        }
        public void updateListView()
        {
            int i = 0;
            foreach (ListViewItem lvi in accountsListView.Items)
            {
                psudoAccount act = database.getAccountByNum(Convert.ToInt32(lvi.SubItems[0].Text));
                lvi.SubItems[3].Text = act.getNumOfChecks().ToString();
                lvi.SubItems[4].Text = act.getCurBal().ToString();
            }
        }
    }
}
