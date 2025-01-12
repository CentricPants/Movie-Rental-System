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
    public partial class Manager : Form
    {
        private bool MenuExpand = false;

        M_Staff staff;
        M_movies movies;
        private int _personId;
        showcustomers showCust;

        public Manager(int personId)
        {
            _personId = personId;
            InitializeComponent();

            this.FormClosing += Form_Closing;

        }
        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // Closes the entire application
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

        private void MenuTransition_Tick(object sender, EventArgs e)
        {
            // Smooth menu transition animation
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
                if (flowLayoutPanel1.Width >= 226)
                {
                    sidebarExpand = true;
                    sidebarTransition.Stop();

                }


            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            sidebarTransition.Start();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            MenuTransition.Start();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Staff button

            if (staff == null)
            {
                staff = new M_Staff(_personId);
                staff.FormClosed += staff_FormClosed;
                staff.MdiParent = this;
                staff.Dock = DockStyle.Fill;
                staff.Show();



            }
            else
            {
                staff.Activate();
            }
        }
        private void staff_FormClosed(object sender, FormClosedEventArgs e)
        {
            staff = null;
        }
        private void Pn_movies_Click(object sender, EventArgs e)
        {

            if (movies == null)
            {
                movies = new M_movies(_personId);
                movies.FormClosed += movies_FormClosed;
                movies.MdiParent = this;
                movies.Dock = DockStyle.Fill;
                movies.Show();



            }
            else
            {
                movies.Activate();
            }
        }
        private void movies_FormClosed(object sender, FormClosedEventArgs e)
        {
            movies = null;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //customers button
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

        man_dash Rentals;

        private void button1_Click(object sender, EventArgs e)
        {
            //dashboard button for manager
            if (Rentals == null)
            {
                Rentals = new man_dash(_personId);
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
    }
}
