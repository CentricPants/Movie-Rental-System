using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Movie_Rental
{
    public partial class Cashier : Form
    {
        private bool MenuExpand = false;
        private int _personId; // Store Person_ID
        showcustomers showCust;
        Requests_Cashier showReq;
        showRentals Rentals;
        public Cashier(int personId)
        {
            InitializeComponent();
            _personId = personId;
            this.FormClosing += Form_Closing;
        }
        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // Closes the entire application
        }
        private void button7_Click(object sender, EventArgs e)
        {
            MenuTransition.Start();
        }

        private void Pn_movies_Click(object sender, EventArgs e)
        {
            if (showCust == null)
            {
                showCust = new showcustomers(_personId);
                showCust.FormClosed += showCust_FormClosed;
                showCust.MdiParent = this;
                showCust.Dock = DockStyle.Fill;
                showCust.Show();



            }
            else
            {
                showCust.Activate();
            }
        }
        private void showCust_FormClosed(object sender, FormClosedEventArgs e)
        {
            showCust = null;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (showReq == null)
            {
                showReq = new Requests_Cashier(_personId);
                showReq.FormClosed += showReq_FormClosed;
                showReq.MdiParent = this;
                showReq.Dock = DockStyle.Fill;
                showReq.Show();



            }
            else
            {
                showReq.Activate();
            }

        }
        private void showReq_FormClosed(object sender, FormClosedEventArgs e)
        {
            showReq = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 login = Application.OpenForms["Form1"] as Form1;

            if (login == null)
            {
                login = new Form1();
                login.Show();
            }
            else
            {
                login.Show();
            }

            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (showCust == null)
            {
                Rentals = new showRentals(_personId);
                Rentals.FormClosed += showRent_FormClosed;
                Rentals.MdiParent = this;
                Rentals.Dock = DockStyle.Fill;
                Rentals.Show();



            }
            else
            {
                Rentals.Activate();
            }
        }
        private void showRent_FormClosed(object sender, FormClosedEventArgs e)
        {
            Rentals = null;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            sidebarTransition.Start();
        }

        bool sidebarExpand = true;
        private void sidebarTransition_Tick(object sender, EventArgs e)
        {
            if (sidebarExpand)
            {
                flowLayoutPanel1.Width -= 10;
                if (flowLayoutPanel1.Width <= 59)
                {
                    sidebarExpand = false;
                    sidebarTransition.Stop();

                }

            }
            else
            {
                flowLayoutPanel1.Width += 10;
                if (flowLayoutPanel1.Width >= 227)
                {
                    sidebarExpand = true;
                    sidebarTransition.Stop();

                }


            }

        }

        private void MenuTransition_Tick(object sender, EventArgs e)
        {
            if (!MenuExpand)
            {
                menuContainer.Height += 10;
                if (menuContainer.Height >= 214)
                {
                    MenuTransition.Stop();
                    MenuExpand = true;
                }
            }
            else
            {
                menuContainer.Height -= 10;
                if (menuContainer.Height <= 50)
                {
                    MenuTransition.Stop();
                    MenuExpand = false;
                }
            }

        }
    }
}
