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
    public partial class Dashboard_A : Form
    {
        public Dashboard_A()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            // SQL query to fetch Branch information excluding the primary key
            string query = "SELECT City, Street, Zip_Code, Telephone_No FROM Branch";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Bind the fetched data to the DataGridView
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while fetching the branch data: " + ex.Message);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Get input data from the textboxes
            string city = textBox1.Text.Trim();     // City TextBox
            string street = textBox2.Text.Trim();   // Street TextBox
            string zipCode = textBox3.Text.Trim();  // Zip Code TextBox
            string telephone = textBox4.Text.Trim(); // Telephone TextBox

            if (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(street) ||
                string.IsNullOrWhiteSpace(zipCode) || string.IsNullOrWhiteSpace(telephone))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Get the next Branch_Number
                    string getLastBranchNumberQuery = "SELECT ISNULL(MAX(Branch_Number), 0) + 1 FROM Branch";
                    SqlCommand getBranchNumberCommand = new SqlCommand(getLastBranchNumberQuery, connection);
                    int nextBranchNumber = (int)getBranchNumberCommand.ExecuteScalar();

                    // Insert new branch into the table
                    string insertBranchQuery = @"
                INSERT INTO Branch (Branch_Number, City, Street, Zip_Code, Telephone_No) 
                VALUES (@BranchNumber, @City, @Street, @ZipCode, @Telephone)";
                    SqlCommand insertBranchCommand = new SqlCommand(insertBranchQuery, connection);
                    insertBranchCommand.Parameters.AddWithValue("@BranchNumber", nextBranchNumber);
                    insertBranchCommand.Parameters.AddWithValue("@City", city);
                    insertBranchCommand.Parameters.AddWithValue("@Street", street);
                    insertBranchCommand.Parameters.AddWithValue("@ZipCode", zipCode);
                    insertBranchCommand.Parameters.AddWithValue("@Telephone", telephone);

                    insertBranchCommand.ExecuteNonQuery();

                    MessageBox.Show("Branch successfully added!");

                    // Optionally, refresh the data in the DataGridView to show the updated branch data
                    button1_Click(sender, e); // Reuse the fetch logic from button1
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while adding the branch: " + ex.Message);
                }
            }
        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //Zip Code TextBox
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //City Textbox
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //Street Textbox
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //Telephone Textbox
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            // SQL query to fetch rental agreement information with branch details
            string query = @"
        SELECT 
            RA.Rental_No,
            M.Title, 
            RA.Start_Date, 
            RA.Return_Date, 
            CU.First_name, 
            CU.Last_name, 
            B.City AS Branch_City, 
            B.Street AS Branch_Street
        FROM Rental_Agreement RA
        JOIN Movie_Copy MC ON RA.Movie_number = MC.Movie_number
        JOIN Movie M ON MC.Catalog_no = M.Catalog_no
        JOIN Customer CU ON RA.Customer_ID = CU.Customer_ID
        JOIN Branch B ON CU.Branch_Number = B.Branch_Number";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Bind the fetched data to the DataGridView
                    dataGridView2.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while fetching the rental agreements: " + ex.Message);
                }
            }
        }



        private void button4_Click(object sender, EventArgs e)
        {
            // Get input data from the textboxes
            string city = textBox1.Text.Trim();
            string street = textBox2.Text.Trim();
            string zipCode = textBox3.Text.Trim();

            if (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(street) || string.IsNullOrWhiteSpace(zipCode))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Start a transaction
                    SqlTransaction transaction = connection.BeginTransaction();

                    // Step 1: Get the Branch_Number
                    string getBranchNumberQuery = "SELECT Branch_Number FROM Branch WHERE City = @City AND Street = @Street AND Zip_Code = @ZipCode";
                    SqlCommand getBranchNumberCommand = new SqlCommand(getBranchNumberQuery, connection, transaction);
                    getBranchNumberCommand.Parameters.AddWithValue("@City", city);
                    getBranchNumberCommand.Parameters.AddWithValue("@Street", street);
                    getBranchNumberCommand.Parameters.AddWithValue("@ZipCode", zipCode);

                    object branchNumberObj = getBranchNumberCommand.ExecuteScalar();
                    if (branchNumberObj == null)
                    {
                        MessageBox.Show("Branch not found.");
                        return;
                    }
                    int branchNumber = (int)branchNumberObj;

                    // Step 2: Remove Rental Agreements associated with this branch
                    string deleteRentalAgreementsQuery = @"
                DELETE RA
                FROM Rental_Agreement RA
                JOIN Movie_Copy MC ON RA.Movie_number = MC.Movie_number
                WHERE MC.Branch_Number = @BranchNumber";
                    SqlCommand deleteRentalAgreementsCommand = new SqlCommand(deleteRentalAgreementsQuery, connection, transaction);
                    deleteRentalAgreementsCommand.Parameters.AddWithValue("@BranchNumber", branchNumber);
                    deleteRentalAgreementsCommand.ExecuteNonQuery();

                    // Step 3: Remove Movie Copies and Movies associated with this branch
                    string deleteMoviesQuery = @"
                DELETE MC
                FROM Movie_Copy MC
                WHERE MC.Branch_Number = @BranchNumber";
                    SqlCommand deleteMoviesCommand = new SqlCommand(deleteMoviesQuery, connection, transaction);
                    deleteMoviesCommand.Parameters.AddWithValue("@BranchNumber", branchNumber);
                    deleteMoviesCommand.ExecuteNonQuery();

                    // Step 4: Remove Staff (Managers/Cashiers) and related Users
                    string deleteStaffRolesQuery = @"
                DELETE M
                FROM Manager M
                JOIN Staff S ON M.Staff_ID = S.Staff_ID
                WHERE S.Branch_Number = @BranchNumber;
                
                DELETE C
                FROM Cashier C
                JOIN Staff S ON C.Staff_ID = S.Staff_ID
                WHERE S.Branch_Number = @BranchNumber;";
                    SqlCommand deleteStaffRolesCommand = new SqlCommand(deleteStaffRolesQuery, connection, transaction);
                    deleteStaffRolesCommand.Parameters.AddWithValue("@BranchNumber", branchNumber);
                    deleteStaffRolesCommand.ExecuteNonQuery();

                    string deleteStaffQuery = "DELETE FROM Staff WHERE Branch_Number = @BranchNumber";
                    SqlCommand deleteStaffCommand = new SqlCommand(deleteStaffQuery, connection, transaction);
                    deleteStaffCommand.Parameters.AddWithValue("@BranchNumber", branchNumber);
                    deleteStaffCommand.ExecuteNonQuery();

                    string deleteUsersQuery = @"
                DELETE U
                FROM Users U
                JOIN Staff S ON U.Person_ID = S.Person_ID
                WHERE S.Branch_Number = @BranchNumber";
                    SqlCommand deleteUsersCommand = new SqlCommand(deleteUsersQuery, connection, transaction);
                    deleteUsersCommand.Parameters.AddWithValue("@BranchNumber", branchNumber);
                    deleteUsersCommand.ExecuteNonQuery();

                    // Step 5: Remove Customers associated with this branch
                    string deleteCustomersQuery = "DELETE FROM Customer WHERE Branch_Number = @BranchNumber";
                    SqlCommand deleteCustomersCommand = new SqlCommand(deleteCustomersQuery, connection, transaction);
                    deleteCustomersCommand.Parameters.AddWithValue("@BranchNumber", branchNumber);
                    deleteCustomersCommand.ExecuteNonQuery();

                    // Step 6: Remove the Branch
                    string deleteBranchQuery = "DELETE FROM Branch WHERE Branch_Number = @BranchNumber";
                    SqlCommand deleteBranchCommand = new SqlCommand(deleteBranchQuery, connection, transaction);
                    deleteBranchCommand.Parameters.AddWithValue("@BranchNumber", branchNumber);
                    deleteBranchCommand.ExecuteNonQuery();

                    // Commit the transaction
                    transaction.Commit();

                    MessageBox.Show("Branch and all associated data successfully removed!");

                    // Optionally refresh the DataGridView
                    button1_Click(sender, e); // Refresh branch data
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while removing the branch: " + ex.Message);
                }
            }
        }

        private void insert1_trans_Tick(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            //movie no 

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            //customer ID
        }

        private void label9_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            //Return date
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            //start date 
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Get input data from the textboxes
            string movieNumber = textBox8.Text.Trim();   // Movie number
            string customerId = textBox7.Text.Trim();   // Customer ID
            string startDate = textBox5.Text.Trim();    // Start Date
            string returnDate = textBox6.Text.Trim();   // Return Date

            if (string.IsNullOrWhiteSpace(movieNumber) || string.IsNullOrWhiteSpace(customerId) ||
                string.IsNullOrWhiteSpace(startDate) || string.IsNullOrWhiteSpace(returnDate))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Get the next Rental_No
                    string getLastRentalNoQuery = "SELECT ISNULL(MAX(Rental_No), 0) + 1 FROM Rental_Agreement";
                    SqlCommand getLastRentalNoCommand = new SqlCommand(getLastRentalNoQuery, connection);
                    int nextRentalNo = (int)getLastRentalNoCommand.ExecuteScalar();

                    // Insert new rental agreement
                    string insertRentalQuery = @"
                INSERT INTO Rental_Agreement (Rental_No, Movie_number, Customer_ID, Start_Date, Return_Date)
                VALUES (@RentalNo, @MovieNumber, @CustomerId, @StartDate, @ReturnDate)";
                    SqlCommand insertRentalCommand = new SqlCommand(insertRentalQuery, connection);
                    insertRentalCommand.Parameters.AddWithValue("@RentalNo", nextRentalNo);
                    insertRentalCommand.Parameters.AddWithValue("@MovieNumber", movieNumber);
                    insertRentalCommand.Parameters.AddWithValue("@CustomerId", customerId);
                    insertRentalCommand.Parameters.AddWithValue("@StartDate", DateTime.Parse(startDate));
                    insertRentalCommand.Parameters.AddWithValue("@ReturnDate", DateTime.Parse(returnDate));

                    insertRentalCommand.ExecuteNonQuery();

                    MessageBox.Show("Rental agreement successfully added!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while adding the rental agreement: " + ex.Message);
                }
            }
        }


        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            //Rental No (only take this data for removing a rental agreement not for insertion)

        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Get the Rental_No from the textbox
            string rentalNo = textBox9.Text.Trim();

            if (string.IsNullOrWhiteSpace(rentalNo))
            {
                MessageBox.Show("Please provide the Rental No.");
                return;
            }

            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Delete rental agreement by Rental_No
                    string deleteRentalQuery = "DELETE FROM Rental_Agreement WHERE Rental_No = @RentalNo";
                    SqlCommand deleteRentalCommand = new SqlCommand(deleteRentalQuery, connection);
                    deleteRentalCommand.Parameters.AddWithValue("@RentalNo", rentalNo);

                    int rowsAffected = deleteRentalCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Rental agreement successfully removed!");
                    }
                    else
                    {
                        MessageBox.Show("Rental agreement not found.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while removing the rental agreement: " + ex.Message);
                }
            }
        }

    }
}
