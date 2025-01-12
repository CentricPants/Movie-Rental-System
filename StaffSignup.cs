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
    public partial class StaffSignup : Form
    {
        public StaffSignup()
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Position, Contains two options Manager , Cashier (no bullets behind ,just Manager then new line Cashier)
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();
            string firstName = textBox3.Text.Trim();
            string lastName = textBox4.Text.Trim();
            string selectedBranch = comboBox2.SelectedItem?.ToString();
            string position = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(selectedBranch) || string.IsNullOrWhiteSpace(position))
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

                    // Check if a manager already exists in the branch
                    if (position.Equals("Manager", StringComparison.OrdinalIgnoreCase))
                    {
                        string checkManagerQuery = "SELECT COUNT(*) FROM Staff WHERE Branch_Number = @BranchNumber AND Position = 'Manager'";
                        SqlCommand checkManagerCommand = new SqlCommand(checkManagerQuery, connection);
                        checkManagerCommand.Parameters.AddWithValue("@BranchNumber", branchNumber);

                        int managerExists = (int)checkManagerCommand.ExecuteScalar();
                        if (managerExists > 0)
                        {
                            MessageBox.Show("A manager already exists in the selected branch.", "Manager Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Fetch the next Person_ID
                    string getNextPersonIdQuery = "SELECT ISNULL(MAX(Person_ID), 0) + 1 FROM Users";
                    SqlCommand getNextPersonIdCommand = new SqlCommand(getNextPersonIdQuery, connection);
                    int nextPersonId = (int)getNextPersonIdCommand.ExecuteScalar();

                    // Insert into Users table
                    string insertUserQuery = "INSERT INTO Users (Person_ID, Username, Password, Role) VALUES (@PersonID, @Username, @Password, 'Staff')";
                    SqlCommand insertUserCommand = new SqlCommand(insertUserQuery, connection);
                    insertUserCommand.Parameters.AddWithValue("@PersonID", nextPersonId);
                    insertUserCommand.Parameters.AddWithValue("@Username", username);
                    insertUserCommand.Parameters.AddWithValue("@Password", password);
                    insertUserCommand.ExecuteNonQuery();

                    // Insert into Staff table
                    string insertStaffQuery = @"
                INSERT INTO Staff (Staff_ID, Person_ID, Branch_Number, First_name, Last_name, Position, Salary, Joining_Date)
                VALUES (
                    @StaffID,
                    @PersonID,
                    @BranchNumber,
                    @FirstName,
                    @LastName,
                    @Position,
                    @Salary,
                    GETDATE()
                )";
                    SqlCommand insertStaffCommand = new SqlCommand(insertStaffQuery, connection);
                    insertStaffCommand.Parameters.AddWithValue("@StaffID", nextPersonId);
                    insertStaffCommand.Parameters.AddWithValue("@PersonID", nextPersonId);
                    insertStaffCommand.Parameters.AddWithValue("@BranchNumber", branchNumber);
                    insertStaffCommand.Parameters.AddWithValue("@FirstName", firstName);
                    insertStaffCommand.Parameters.AddWithValue("@LastName", lastName);
                    insertStaffCommand.Parameters.AddWithValue("@Position", position);
                    insertStaffCommand.Parameters.AddWithValue("@Salary", position.Equals("Manager") ? 60000 : 40000); // Example salary
                    insertStaffCommand.ExecuteNonQuery();

                    if (position.Equals("Manager", StringComparison.OrdinalIgnoreCase))
                    {
                        // Count the current number of cashiers in the branch
                        string countCashiersQuery = "SELECT COUNT(*) FROM Staff WHERE Branch_Number = @BranchNumber AND Position = 'Cashier'";
                        SqlCommand countCashiersCommand = new SqlCommand(countCashiersQuery, connection);
                        countCashiersCommand.Parameters.AddWithValue("@BranchNumber", branchNumber);

                        int numberOfCashiers = (int)countCashiersCommand.ExecuteScalar();

                        // Insert into Manager table
                        string insertManagerQuery = "INSERT INTO Manager (Staff_ID, Bonus, Number_of_Employees_Supervised) VALUES (@StaffID, 5000, @NumberOfEmployees)";
                        SqlCommand insertManagerCommand = new SqlCommand(insertManagerQuery, connection);
                        insertManagerCommand.Parameters.AddWithValue("@StaffID", nextPersonId);
                        insertManagerCommand.Parameters.AddWithValue("@NumberOfEmployees", numberOfCashiers);
                        insertManagerCommand.ExecuteNonQuery();

                        MessageBox.Show("Manager account created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Open Manager form
                        Manager managerForm = new Manager(nextPersonId);
                        managerForm.Show();
                        this.Hide(); // Hide the current form
                    }
                    else if (position.Equals("Cashier", StringComparison.OrdinalIgnoreCase))
                    {
                        // Insert into Cashier table
                        string insertCashierQuery = "INSERT INTO Cashier (Staff_ID, Shift_Timing) VALUES (@StaffID, 'Morning')";
                        SqlCommand insertCashierCommand = new SqlCommand(insertCashierQuery, connection);
                        insertCashierCommand.Parameters.AddWithValue("@StaffID", nextPersonId);
                        insertCashierCommand.ExecuteNonQuery();

                        // Update the number of employees supervised for the manager of this branch
                        string updateManagerQuery = @"
                    UPDATE Manager
                    SET Number_of_Employees_Supervised = Number_of_Employees_Supervised + 1
                    WHERE Staff_ID = (
                        SELECT Staff_ID
                        FROM Staff
                        WHERE Branch_Number = @BranchNumber AND Position = 'Manager'
                    )";
                        SqlCommand updateManagerCommand = new SqlCommand(updateManagerQuery, connection);
                        updateManagerCommand.Parameters.AddWithValue("@BranchNumber", branchNumber);
                        updateManagerCommand.ExecuteNonQuery();

                        MessageBox.Show("Cashier account created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Open Cashier form
                        Cashier cashierForm = new Cashier(nextPersonId);
                        cashierForm.Show();
                        this.Hide(); // Hide the current form
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while creating the staff account: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //password
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //username
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //first name
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //last name
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //branches should be loaded into this and the person can only choose an existing branch
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void StaffSignup_Load(object sender, EventArgs e)
        {
            loadBranches();
        }
        private void loadBranches()
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

    }
}
