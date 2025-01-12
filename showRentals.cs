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
    public partial class showRentals : Form
    {
        private int _personId; // Store Person_ID
        private string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

        public showRentals(int personId)
        {
            InitializeComponent();
            _personId = personId;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Show rental agreements for the branch of the person
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                    SELECT RA.Rental_No, M.Title AS 'Movie Title', RA.Start_Date, RA.Return_Date, CU.First_name, CU.Last_name
                    FROM Rental_Agreement RA
                    JOIN Movie_Copy MC ON RA.Movie_number = MC.Movie_number
                    JOIN Movie M ON MC.Catalog_no = M.Catalog_no
                    JOIN Customer CU ON RA.Customer_ID = CU.Customer_ID
                    WHERE MC.Branch_Number = (
                        SELECT Branch_Number
                        FROM Staff
                        WHERE Person_ID = @PersonID
                    )";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PersonID", _personId);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView2.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while fetching rental agreements: " + ex.Message);
                }
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Define connection string
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            // Show information of the current cashier
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to fetch cashier-specific information
                    string query = @"
                SELECT 
                    U.Username AS 'Username', 
                    S.First_name AS 'First Name', 
                    S.Last_name AS 'Last Name', 
                    S.Position AS 'Position', 
                    S.Salary AS 'Salary', 
                    S.Joining_Date AS 'Joining Date', 
                    Ca.Shift_Timing AS 'Shift Timing', 
                    B.City AS 'Branch City'
                FROM Users U
                JOIN Staff S ON U.Person_ID = S.Person_ID
                JOIN Cashier Ca ON S.Staff_ID = Ca.Staff_ID
                JOIN Branch B ON S.Branch_Number = B.Branch_Number
                WHERE U.Person_ID = @PersonID";

                    // Prepare SQL command
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PersonID", _personId);

                    // Execute query and populate DataGridView
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Bind the fetched data to the DataGridView
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    // Show error message if something goes wrong
                    MessageBox.Show("An error occurred while fetching user information: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //input for new updated username
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Update the username for the current person
            string newUsername = textBox3.Text.Trim();

            if (string.IsNullOrWhiteSpace(newUsername))
            {
                MessageBox.Show("Please enter a valid username.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Check if the new username is already taken
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @NewUsername";
                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@NewUsername", newUsername);

                    int count = (int)checkCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("This username is already taken. Please choose another.");
                        return;
                    }

                    // Update the username
                    string updateQuery = "UPDATE Users SET Username = @NewUsername WHERE Person_ID = @PersonID";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@NewUsername", newUsername);
                    updateCommand.Parameters.AddWithValue("@PersonID", _personId);

                    updateCommand.ExecuteNonQuery();

                    MessageBox.Show("Username updated successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while updating the username: " + ex.Message);
                }
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //input for updated password
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Update the password for the current person
            string newPassword = textBox2.Text.Trim();

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Please enter a valid password.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Update the password
                    string updateQuery = "UPDATE Users SET Password = @NewPassword WHERE Person_ID = @PersonID";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@NewPassword", newPassword);
                    updateCommand.Parameters.AddWithValue("@PersonID", _personId);

                    updateCommand.ExecuteNonQuery();

                    MessageBox.Show("Password updated successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while updating the password: " + ex.Message);
                }
            }
        }



       
    }
}
