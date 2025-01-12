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
    public partial class AdminDash : Form
    {

        Ad_Staff StaffDash;
        AD_Customer CustomerDash;
        Dashboard_A Dashboard;
        admin_movie Movies;
        // State variable to track menu expansion
        private bool MenuExpand = false;

        public AdminDash()
        {
            InitializeComponent();
            this.FormClosing += Form_Closing;
        }

        private void AdminDash_Load(object sender, EventArgs e)
        {
            // Initialization code for the Admin Dashboard can go here
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Handle custom painting for panel1 if needed
        }

        private void button1_Click(object sender, EventArgs e)//Dashboard button
        {

            if (Dashboard == null)
            {
                Dashboard = new Dashboard_A();
                Dashboard.FormClosed += Dashboard_FormClosed;
                Dashboard.MdiParent = this;
                Dashboard.Dock = DockStyle.Fill;
                Dashboard.Show();



            }
            else
            {
                Dashboard.Activate();
            }  //Dashboard

        }
        private void Dashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dashboard = null;
        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            // Action for label1 click
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
           
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

        private void button4_Click(object sender, EventArgs e)//STAFF Button
        {
            if (StaffDash == null)
            {
                StaffDash = new Ad_Staff();
                StaffDash.FormClosed += StaffDash_FormClosed;
                StaffDash.MdiParent = this;
                StaffDash.Dock = DockStyle.Fill; 
                StaffDash.Show();
                


            }
            else { 
            StaffDash.Activate(); 
            }
            
        }
        private void StaffDash_FormClosed(object sender, FormClosedEventArgs e)
        {
            StaffDash = null;
        }

        private void menuContainer_Paint(object sender, PaintEventArgs e)
        {
            // Handle custom painting for menuContainer if needed
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Start menu transition on button7 click
            MenuTransition.Start();
        }
        bool sidebarExpand = true;
        private void sidebarTransition_Tick(object sender, EventArgs e)
        {
            if (sidebarExpand) {
                flowLayoutPanel1.Width -= 10;
                if(flowLayoutPanel1.Width <= 59)
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

        private void button2_Click(object sender, EventArgs e)
        {
            SignUp signUpForm = Application.OpenForms["SignUp"] as SignUp;

            if (signUpForm == null)
            {
                signUpForm = new SignUp();
                signUpForm.Show();
            }
            else
            {
                signUpForm.Show();
            }

            this.Hide();
        }
        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // Closes the entire application
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (CustomerDash == null)
            {
                CustomerDash = new AD_Customer();
                CustomerDash.FormClosed += CustomerDash_FormClosed;
                CustomerDash.MdiParent = this;
                CustomerDash.Dock = DockStyle.Fill;
                CustomerDash.Show();
            }
            else
            {
                CustomerDash.Activate();
            }

        }
        private void CustomerDash_FormClosed(object sender, FormClosedEventArgs e)
        {
            CustomerDash = null;
        }

        private void Pn_movies_Click(object sender, EventArgs e)
        {
            if (Movies == null)
            {
                Movies = new admin_movie();
                Movies.FormClosed += Movies_FormClosed;
                Movies.MdiParent = this;
                Movies.Dock = DockStyle.Fill;
                Movies.Show();



            }
            else
            {
                Movies.Activate();
            }  //Dashboard

        }

        private void Movies_FormClosed(object sender, FormClosedEventArgs e)
        {
            Movies = null;
        }
    }
}
