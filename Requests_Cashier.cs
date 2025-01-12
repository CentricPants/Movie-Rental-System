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
    public partial class Requests_Cashier : Form
    {
        private int _personId; // Store Person_ID
        private string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

        public Requests_Cashier(int personId)
        {
            InitializeComponent();
            _personId = personId;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Show all requests for this cashier's branch
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                    SELECT RR.Transaction_ID, RR.User_ID, RR.Customer_ID, RR.Movie_Number, RR.Status_of, RR.Transaction_Date
                    FROM Request_Return RR
                    JOIN Customer C ON RR.Customer_ID = C.Customer_ID
                    WHERE RR.Status_of = 'Request' AND C.Branch_Number = (
                        SELECT S.Branch_Number
                        FROM Staff S
                        WHERE S.Person_ID = @PersonID
                    )";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PersonID", _personId);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while fetching requests: " + ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Show all returns for this cashier's branch
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                    SELECT RR.Transaction_ID, RR.User_ID, RR.Customer_ID, RR.Movie_Number, RR.Status_of, RR.Transaction_Date
                    FROM Request_Return RR
                    JOIN Customer C ON RR.Customer_ID = C.Customer_ID
                    WHERE RR.Status_of = 'Return' AND C.Branch_Number = (
                        SELECT S.Branch_Number
                        FROM Staff S
                        WHERE S.Person_ID = @PersonID
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
                    MessageBox.Show("An error occurred while fetching returns: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Process a request and create a Rental Agreement
            int requestId;
            if (!int.TryParse(textBox1.Text, out requestId))
            {
                MessageBox.Show("Please enter a valid request ID.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Get request details
                    string query = @"
        SELECT RR.Movie_Number, C.Customer_ID, C.Branch_Number
        FROM Request_Return RR
        JOIN Customer C ON RR.Customer_ID = C.Customer_ID
        WHERE RR.Transaction_ID = @TransactionID AND RR.Status_of = 'Request'";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TransactionID", requestId);

                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        MessageBox.Show("No valid request found with the given ID.");
                        return;
                    }

                    int movieNumber = reader.GetInt32(0);
                    int customerId = reader.GetInt32(1);
                    int branchNumber = reader.GetInt32(2);

                    reader.Close();

                    // Fetch the next Rental_ID
                    string getNextRentalIdQuery = "SELECT ISNULL(MAX(Rental_No), 0) + 1 FROM Rental_Agreement";
                    SqlCommand getNextRentalIdCommand = new SqlCommand(getNextRentalIdQuery, connection);
                    int nextRentalId = (int)getNextRentalIdCommand.ExecuteScalar();

                    // Insert into Rental Agreement
                    string insertRentalQuery = @"
        INSERT INTO Rental_Agreement (Rental_No, Movie_number, Customer_ID, Start_Date, Return_Date)
        VALUES (@RentalID, @MovieNumber, @CustomerID, GETDATE(), NULL)";

                    SqlCommand insertRentalCommand = new SqlCommand(insertRentalQuery, connection);
                    insertRentalCommand.Parameters.AddWithValue("@RentalID", nextRentalId);
                    insertRentalCommand.Parameters.AddWithValue("@MovieNumber", movieNumber);
                    insertRentalCommand.Parameters.AddWithValue("@CustomerID", customerId);
                    insertRentalCommand.ExecuteNonQuery();

                    // Update Movie_Copy quantity
                    string updateMovieCopyQuery = @"
        UPDATE Movie_Copy
        SET Quantity = Quantity - 1
        WHERE Movie_number = @MovieNumber AND Branch_Number = @BranchNumber";

                    SqlCommand updateMovieCopyCommand = new SqlCommand(updateMovieCopyQuery, connection);
                    updateMovieCopyCommand.Parameters.AddWithValue("@MovieNumber", movieNumber);
                    updateMovieCopyCommand.Parameters.AddWithValue("@BranchNumber", branchNumber);
                    updateMovieCopyCommand.ExecuteNonQuery();

                    // Remove request from Request_Return
                    string deleteRequestQuery = "DELETE FROM Request_Return WHERE Transaction_ID = @TransactionID";
                    SqlCommand deleteRequestCommand = new SqlCommand(deleteRequestQuery, connection);
                    deleteRequestCommand.Parameters.AddWithValue("@TransactionID", requestId);
                    deleteRequestCommand.ExecuteNonQuery();

                    MessageBox.Show("Rental Agreement created and request processed successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while processing the request: " + ex.Message);
                }
            }

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //input the id of the requests
        }

      

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //input the id of the returns transaction

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Process a return and delete the Rental Agreement
            int returnId;
            if (!int.TryParse(textBox2.Text, out returnId))
            {
                MessageBox.Show("Please enter a valid return ID.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Get return details
                    string query = @"
                    SELECT RR.Movie_Number, C.Branch_Number
                    FROM Request_Return RR
                    JOIN Customer C ON RR.Customer_ID = C.Customer_ID
                    WHERE RR.Transaction_ID = @TransactionID AND RR.Status_of = 'Return'";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TransactionID", returnId);

                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        MessageBox.Show("No valid return found with the given ID.");
                        return;
                    }

                    int movieNumber = reader.GetInt32(0);
                    int branchNumber = reader.GetInt32(1);

                    reader.Close();

                    // Delete Rental Agreement
                    string deleteRentalQuery = @"
                    DELETE FROM Rental_Agreement
                    WHERE Movie_number = @MovieNumber";

                    SqlCommand deleteRentalCommand = new SqlCommand(deleteRentalQuery, connection);
                    deleteRentalCommand.Parameters.AddWithValue("@MovieNumber", movieNumber);
                    deleteRentalCommand.ExecuteNonQuery();

                    // Update Movie_Copy quantity
                    string updateMovieCopyQuery = @"
                    UPDATE Movie_Copy
                    SET Quantity = Quantity + 1
                    WHERE Movie_number = @MovieNumber AND Branch_Number = @BranchNumber";

                    SqlCommand updateMovieCopyCommand = new SqlCommand(updateMovieCopyQuery, connection);
                    updateMovieCopyCommand.Parameters.AddWithValue("@MovieNumber", movieNumber);
                    updateMovieCopyCommand.Parameters.AddWithValue("@BranchNumber", branchNumber);
                    updateMovieCopyCommand.ExecuteNonQuery();

                    // Remove return from Request_Return
                    string deleteReturnQuery = "DELETE FROM Request_Return WHERE Transaction_ID = @TransactionID";
                    SqlCommand deleteReturnCommand = new SqlCommand(deleteReturnQuery, connection);
                    deleteReturnCommand.Parameters.AddWithValue("@TransactionID", returnId);
                    deleteReturnCommand.ExecuteNonQuery();

                    MessageBox.Show("Return processed and Rental Agreement deleted successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while processing the return: " + ex.Message);
                }
            }
        }
    }
}
