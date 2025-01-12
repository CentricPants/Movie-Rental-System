using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Movie_Rental
{
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
            this.FormClosing += Form_Closing;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 mainForm = Application.OpenForms["Form1"] as Form1;

            if (mainForm == null)
            {
                mainForm = new Form1();
                mainForm.Show();
            }
            else
            {
                mainForm.Show();
            }

            this.Hide();
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // Closes the entire application
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            StaffSignup staffsign = Application.OpenForms["StaffSignup"] as StaffSignup;

            if (staffsign == null)
            {
                staffsign = new StaffSignup();
                staffsign.Show();
            }
            else
            {
                staffsign.Show();
            }

            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CustomerSignUp custsign = Application.OpenForms["CustomerSignUp"] as CustomerSignUp;

            if (custsign == null)
            {
                custsign = new CustomerSignUp();
                custsign.Show();
            }
            else
            {
                custsign.Show();
            }

            this.Hide();

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
