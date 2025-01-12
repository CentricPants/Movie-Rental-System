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
    public partial class CustomerReq : Form
    {
        private int _personId;

        public CustomerReq(int personId)
        {
            InitializeComponent();
            _personId = personId;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //data grid to show only the requests from the request_return 
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            string query = @"
        SELECT 
            Transaction_ID AS 'Transaction ID',
            Movie_Number AS 'Movie Number',
            Status_of AS 'Status',
            Transaction_Date AS 'Date'
        FROM Request_Return
        WHERE User_ID = @PersonID AND Status_of = 'Request'";

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

                    dataGridView1.DataSource = dataTable; // Show requests in the first DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while fetching requests: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //input the transaction_id to remove if the person changed his mind (but this should only remove requests from the table of that person not the returns)
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //enter the transaction id of the return (only if status is of return) 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string transactionIdText = textBox2.Text.Trim();

            // Validate input
            if (string.IsNullOrWhiteSpace(transactionIdText) || !int.TryParse(transactionIdText, out int transactionId))
            {
                MessageBox.Show("Please enter a valid Transaction ID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            string query = @"
        DELETE FROM Request_Return
        WHERE Transaction_ID = @TransactionID AND User_ID = @PersonID AND Status_of = 'Return'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TransactionID", transactionId);
                    command.Parameters.AddWithValue("@PersonID", _personId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Return removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button3_Click(sender, e); // Refresh returns
                    }
                    else
                    {
                        MessageBox.Show("No matching return found for the given Transaction ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while removing the return: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string transactionIdText = textBox1.Text.Trim();

            // Validate input
            if (string.IsNullOrWhiteSpace(transactionIdText) || !int.TryParse(transactionIdText, out int transactionId))
            {
                MessageBox.Show("Please enter a valid Transaction ID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            string query = @"
        DELETE FROM Request_Return
        WHERE Transaction_ID = @TransactionID AND User_ID = @PersonID AND Status_of = 'Request'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TransactionID", transactionId);
                    command.Parameters.AddWithValue("@PersonID", _personId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Request removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button1_Click(sender, e); // Refresh requests
                    }
                    else
                    {
                        MessageBox.Show("No matching request found for the given Transaction ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while removing the request: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //data grid to show only the returns of this persons 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            string query = @"
        SELECT 
            Transaction_ID AS 'Transaction ID',
            Movie_Number AS 'Movie Number',
            Status_of AS 'Status',
            Transaction_Date AS 'Date'
        FROM Request_Return
        WHERE User_ID = @PersonID AND Status_of = 'Return'";

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

                    dataGridView2.DataSource = dataTable; // Show returns in the second DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while fetching returns: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
