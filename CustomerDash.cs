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
    public partial class CustomerDash : Form
    {
        private bool MenuExpand = false;
        private int _personId; // Store Person_ID

        Dashboard_Cust Dash;
        Movie_Browse Movies;
        CustomerReq Req_Ret;
        public CustomerDash(int personId)
        {
            InitializeComponent();
            _personId = personId; // Assign Person_ID
            this.FormClosing += Form_Closing;
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // Closes the entire application
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            sidebarTransition.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Dash == null)
            {
                Dash = new Dashboard_Cust(_personId);
                Dash.FormClosed += Dash_FormClosed;
                Dash.MdiParent = this;
                Dash.Dock = DockStyle.Fill;
                Dash.Show();
            }
            else
            {
                Dash.Activate();
            }

        }
        private void Dash_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dash = null;
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

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

        private void button6_Click(object sender, EventArgs e)
        {
            if (Req_Ret == null)
            {
                Req_Ret = new CustomerReq(_personId);
                Req_Ret.FormClosed += Request_FormClosed;
                Req_Ret.MdiParent = this;
                Req_Ret.Dock = DockStyle.Fill;
                Req_Ret.Show();
            }
            else
            {
                Req_Ret.Activate();
            }
        }

        private void Request_FormClosed(object sender, FormClosedEventArgs e)
        {
            Req_Ret = null;
        }
        private void Pn_movies_Click(object sender, EventArgs e)
        {
            if (Movies == null)
            {
                Movies = new Movie_Browse(_personId); // Pass Person_ID to Movie_Customer
                Movies.FormClosed += Movies_FormClosed;
                Movies.MdiParent = this;
                Movies.Dock = DockStyle.Fill;
                Movies.Show();
            }
            else
            {
                Movies.Activate();
            }
        }

        private void Movies_FormClosed(object sender, FormClosedEventArgs e)
        {
            Movies = null;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MenuTransition.Start();
        }

        private void MenuTransition_Tick(object sender, EventArgs e)
        {
            if (!MenuExpand)
            {
                menuContainer.Height += 10;
                if (menuContainer.Height >= 163)
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
