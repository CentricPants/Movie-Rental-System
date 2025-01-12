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
    public partial class CustomerSignUp : Form
    {
        public CustomerSignUp()
        {
            InitializeComponent();
            this.BackgroundImageLayout = ImageLayout.Stretch;

            // Set the panel's Paint event for semi-transparency
            panel1.Paint += panel1_Paint;

            // Ensure panel has no default background color (fully transparent)
            panel1.BackColor = Color.Transparent;
            // Handle form closing
            this.FormClosing += Form_Closing;

        }

        private void CustomerSignUp_Load(object sender, EventArgs e)
        {
            loadbranch();
        }

        private void loadbranch()
        {
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Branch_Number, City FROM Branch";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    comboBox2.Items.Clear(); // Clear existing items before loading

                    while (reader.Read())
                    {
                        string branchName = $"{reader["Branch_Number"]} - {reader["City"]}";
                        comboBox2.Items.Add(branchName);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading branches: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Define a semi-transparent black color (30% opacity)
            Color semiTransparentBlack = Color.FromArgb(77, 0, 0, 0); // 77 = 30% opacity

            // Create a brush with the semi-transparent black color
            using (SolidBrush brush = new SolidBrush(semiTransparentBlack))
            {
                // Fill the panel with the semi-transparent black color
                g.FillRectangle(brush, 0, 0, panel1.Width, panel1.Height);
            }
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

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //usernames
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //password
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //first name 
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //last name
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            //street
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            //city
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            //zip
        }
        
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validate and add a new customer and user
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();
            string firstName = textBox3.Text.Trim();
            string lastName = textBox4.Text.Trim();
            string street = textBox5.Text.Trim();
            string city = textBox7.Text.Trim();
            string zipCode = textBox6.Text.Trim();
            string selectedBranch = comboBox2.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(street) || string.IsNullOrWhiteSpace(city) ||
                string.IsNullOrWhiteSpace(zipCode) || string.IsNullOrWhiteSpace(selectedBranch))
            {
                MessageBox.Show("Please fill in all fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string[] branchParts = selectedBranch.Split('-');
            if (branchParts.Length < 2 || !int.TryParse(branchParts[0].Trim(), out int branchNumber))
            {
                MessageBox.Show("Invalid branch selected. Please try again.", "Branch Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Check if the username is already taken
                    string checkUsernameQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    SqlCommand checkUsernameCommand = new SqlCommand(checkUsernameQuery, connection);
                    checkUsernameCommand.Parameters.AddWithValue("@Username", username);

                    int usernameExists = (int)checkUsernameCommand.ExecuteScalar();
                    if (usernameExists > 0)
                    {
                        MessageBox.Show("Username is already taken. Please choose a different one.", "Username Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Fetch the next Person_ID
                    string getNextPersonIdQuery = "SELECT ISNULL(MAX(Person_ID), 0) + 1 FROM Users";
                    SqlCommand getNextPersonIdCommand = new SqlCommand(getNextPersonIdQuery, connection);
                    int nextPersonId = (int)getNextPersonIdCommand.ExecuteScalar();

                    // Insert into Users table
                    string insertUserQuery = "INSERT INTO Users (Person_ID, Username, Password, Role) VALUES (@PersonID, @Username, @Password, 'Customer')";
                    SqlCommand insertUserCommand = new SqlCommand(insertUserQuery, connection);
                    insertUserCommand.Parameters.AddWithValue("@PersonID", nextPersonId);
                    insertUserCommand.Parameters.AddWithValue("@Username", username);
                    insertUserCommand.Parameters.AddWithValue("@Password", password);
                    insertUserCommand.ExecuteNonQuery();

                    // Insert into Customer table
                    string insertCustomerQuery = @"
                INSERT INTO Customer (Customer_ID, Person_ID, Branch_Number, First_name, Last_name, Street, City, Zip_Code)
                VALUES (
                    @CustomerID,
                    @PersonID,
                    @BranchNumber,
                    @FirstName,
                    @LastName,
                    @Street,
                    @City,
                    @ZipCode
                )";
                    SqlCommand insertCustomerCommand = new SqlCommand(insertCustomerQuery, connection);
                    insertCustomerCommand.Parameters.AddWithValue("@CustomerID", nextPersonId);
                    insertCustomerCommand.Parameters.AddWithValue("@PersonID", nextPersonId);
                    insertCustomerCommand.Parameters.AddWithValue("@BranchNumber", branchNumber);
                    insertCustomerCommand.Parameters.AddWithValue("@FirstName", firstName);
                    insertCustomerCommand.Parameters.AddWithValue("@LastName", lastName);
                    insertCustomerCommand.Parameters.AddWithValue("@Street", street);
                    insertCustomerCommand.Parameters.AddWithValue("@City", city);
                    insertCustomerCommand.Parameters.AddWithValue("@ZipCode", zipCode);
                    insertCustomerCommand.ExecuteNonQuery();

                    MessageBox.Show("Customer account created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Open CustomerDash form with the new Person_ID
                    CustomerDash customerDash = new CustomerDash(nextPersonId);
                    customerDash.Show();
                    this.Hide(); // Hide the current form
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while creating the account: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
