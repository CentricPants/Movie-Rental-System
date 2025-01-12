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
    public partial class M_Staff : Form
    {
        private int _personid;
        public M_Staff(int personId)
        {

            _personid = personId; 
            InitializeComponent();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void InsertBTN_Click(object sender, EventArgs e)
        {
            // Define connection string
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            // Fetch and display staff from the same branch as the person ID
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to fetch staff information based on the branch of the logged-in user
                    string query = @"
                SELECT 
                    S.Staff_ID AS 'Staff ID', 
                    S.First_name AS 'First Name', 
                    S.Last_name AS 'Last Name', 
                    S.Position AS 'Position', 
                    S.Salary AS 'Salary', 
                    S.Joining_Date AS 'Joining Date', 
                    B.City AS 'Branch City', 
                    B.Street AS 'Branch Street'
                FROM Staff S
                JOIN Branch B ON S.Branch_Number = B.Branch_Number
                WHERE S.Branch_Number = (
                    SELECT Branch_Number
                    FROM Staff
                    WHERE Person_ID = @PersonID
                )";

                    // Prepare the SQL command
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PersonID", _personid);

                    // Execute the query and fill the data into a DataTable
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Bind the fetched data to the DataGridView
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    // Show error message if something goes wrong
                    MessageBox.Show("An error occurred while fetching staff information: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
