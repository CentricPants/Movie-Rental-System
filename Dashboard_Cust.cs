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
    public partial class Dashboard_Cust : Form
    {
        private int _personId;
        public Dashboard_Cust(int personId)
        {
            InitializeComponent();
            _personId = personId;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            string query = @"
        SELECT 
            U.Username AS 'Username',
            U.Password AS 'Password',
            C.First_name AS 'First Name',
            C.Last_name AS 'Last Name',
            C.Street AS 'Street',
            C.City AS 'City',
            C.Zip_Code AS 'Zip Code'
        FROM Users U
        JOIN Customer C ON U.Person_ID = C.Person_ID
        WHERE U.Person_ID = @PersonID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PersonID", _personId);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable; // Bind the user details to the DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while fetching user details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //textbox to input a new username 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string newUsername = textBox3.Text.Trim();

            if (string.IsNullOrWhiteSpace(newUsername))
            {
                MessageBox.Show("Please enter a valid username.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            string query = "UPDATE Users SET Username = @NewUsername WHERE Person_ID = @PersonID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NewUsername", newUsername);
                    command.Parameters.AddWithValue("@PersonID", _personId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Username updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button2_Click(sender, e); // Refresh user details
                    }
                    else
                    {
                        MessageBox.Show("Failed to update username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while updating the username: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            




        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //textbox to input a new Password 

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            string query = @"
        SELECT 
            RA.Rental_No AS 'Rental No',
            M.Title AS 'Movie Title',
            RA.Start_Date AS 'Start Date',
            RA.Return_Date AS 'Return Date'
        FROM Rental_Agreement RA
        JOIN Movie_Copy MC ON RA.Movie_number = MC.Movie_number
        JOIN Movie M ON MC.Catalog_no = M.Catalog_no
        WHERE RA.Customer_ID = (
            SELECT Customer_ID FROM Customer WHERE Person_ID = @PersonID
        )";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PersonID", _personId);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView2.DataSource = dataTable; // Bind the rental agreements to the DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while fetching rental agreements: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string newPassword = textBox2.Text.Trim();

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Please enter a valid password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            string query = "UPDATE Users SET Password = @NewPassword WHERE Person_ID = @PersonID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NewPassword", newPassword);
                    command.Parameters.AddWithValue("@PersonID", _personId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Password updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to update password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while updating the password: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //input the id of the rental agreement
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string rentalIdText = textBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(rentalIdText) || !int.TryParse(rentalIdText, out int rentalId))
            {
                MessageBox.Show("Please enter a valid Rental ID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Step 1: Check if a return entry already exists for this rental agreement
                    string checkQuery = @"
                SELECT COUNT(*)
                FROM Request_Return RR
                JOIN Rental_Agreement RA ON RR.Movie_Number = RA.Movie_number
                WHERE RA.Rental_No = @RentalID
                  AND RR.Status_of = 'Return'
                  AND RR.Customer_ID = (
                      SELECT Customer_ID FROM Customer WHERE Person_ID = @PersonID
                  )";

                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@RentalID", rentalId);
                    checkCommand.Parameters.AddWithValue("@PersonID", _personId);

                    int existingCount = (int)checkCommand.ExecuteScalar();

                    if (existingCount > 0)
                    {
                        MessageBox.Show("A return request for this rental agreement already exists.", "Duplicate Request", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Step 2: Insert the return request if it doesn't exist
                    string insertQuery = @"
                INSERT INTO Request_Return (User_ID, Customer_ID, Branch_ID, Movie_Number, Status_of)
                SELECT 
                    @UserID,
                    RA.Customer_ID,
                    MC.Branch_Number,
                    RA.Movie_number,
                    'Return'
                FROM Rental_Agreement RA
                JOIN Movie_Copy MC ON RA.Movie_number = MC.Movie_number
                WHERE RA.Rental_No = @RentalID
                  AND RA.Customer_ID = (
                      SELECT Customer_ID FROM Customer WHERE Person_ID = @PersonID
                  )";

                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@UserID", _personId);
                    insertCommand.Parameters.AddWithValue("@RentalID", rentalId);
                    insertCommand.Parameters.AddWithValue("@PersonID", _personId);

                    int rowsAffected = insertCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Return request submitted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No matching rental agreement found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while submitting the return request: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


    }
}
