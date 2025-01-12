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
    public partial class showcustomers : Form
    {
        private int _personId; // Store Person_ID

        public showcustomers(int personId)
        {
            _personId = personId;
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to fetch all customers from the branch of the person
                    string query = @"
                SELECT 
                    C.Customer_ID AS 'Customer ID',
                    C.First_name AS 'First Name',
                    C.Last_name AS 'Last Name',
                    C.Street AS 'Street',
                    C.City AS 'City',
                    C.Zip_Code AS 'Zip Code'
                FROM Customer C
                JOIN Staff S ON C.Branch_Number = S.Branch_Number
                WHERE S.Person_ID = @PersonID";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PersonID", _personId);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Bind the result to the DataGridView
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while fetching customers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
