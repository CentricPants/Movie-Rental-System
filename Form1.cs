using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

using System.Windows.Forms;

namespace Movie_Rental
{
    public partial class Form1 : Form
    {

       
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += Form_Closing;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Username Textbox
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //Password Text box 
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // Closes the entire application
        }

        private string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

        private void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // SQL query to check username, password, and retrieve Person_ID and Role
            string query = @"
        SELECT U.Person_ID, U.Role, S.Position
        FROM Users U
        LEFT JOIN Staff S ON U.Person_ID = S.Person_ID
        WHERE U.Username = @Username AND U.Password = @Password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            int personId = reader.GetInt32(0); // Retrieve Person_ID
                            string role = reader.GetString(1); // Retrieve Role
                            string position = reader.IsDBNull(2) ? null : reader.GetString(2); // Retrieve Position if available

                            // Clear textboxes
                            textBox1.Text = "";
                            textBox2.Text = "";

                            if (role.Equals("Customer", StringComparison.OrdinalIgnoreCase))
                            {
                                // Open CustomerDash with Person_ID
                                CustomerDash customerDash = new CustomerDash(personId);
                                customerDash.Show();
                                this.Hide();
                            }
                            else if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                            {
                                // Open AdminDash
                                AdminDash adminDash = new AdminDash();
                                adminDash.Show();
                                this.Hide();
                            }
                            else if (role.Equals("Staff", StringComparison.OrdinalIgnoreCase) && position != null && position.Equals("Cashier", StringComparison.OrdinalIgnoreCase))
                            {
                                // Open Cashier form for staff with the Cashier position
                                Cashier cashierForm = new Cashier(personId);
                                cashierForm.Show();
                                this.Hide();
                            }
                            else if (role.Equals("Staff", StringComparison.OrdinalIgnoreCase) && position != null && position.Equals("Manager", StringComparison.OrdinalIgnoreCase))
                            {
                                // Open Cashier form for staff with the Cashier position
                                Manager managerform = new Manager(personId);
                                managerform.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show($"Welcome, {role}!", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                // Handle other roles as needed
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


    }
}
