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
        }


        private void addActButton_Click(object sender, EventArgs e)
        {
            string firstName = firstNameBox.Text;
            string lastName = lastNameBox.Text;
            int routingNumber = Convert.ToInt32(routingBox1.Text);
            int accountNumber = Convert.ToInt32(accountBox1.Text);
            psudoAccount act = new psudoAccount(firstName, lastName, routingNumber, accountNumber);
            database.addAccount(act);
        }

        private void addChkButton_Click(object sender, EventArgs e)
        {

        }
    }
}
