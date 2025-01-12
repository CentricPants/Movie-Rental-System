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
    public partial class Movie_Customer : Form
    {
        private int _personId;
        public Movie_Customer(int personId)
        {
            InitializeComponent();
            _personId = personId;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void InsertBTN_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Query to fetch movies specific to the customer's branch
                    string query = @"
                SELECT 
                    M.Catalog_no AS 'Catalog ID',
                    M.Title AS 'Movie Title',
                    MC.Quantity AS 'Available Quantity',
                    MC.Status AS 'Status',
                    M.Daily_Rental_Cost AS 'Daily Rent'
                FROM Movie M
                JOIN Movie_Copy MC ON M.Catalog_no = MC.Catalog_no
                JOIN Customer C ON MC.Branch_Number = C.Branch_Number
                WHERE C.Person_ID = @PersonID";

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
                    MessageBox.Show("An error occurred while fetching movies: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
                //input the catalog number of the movie
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";
            string catalogIdText = textBox11.Text.Trim(); // Assuming this textbox is used for inputting Catalog ID

            // Validate input
            if (string.IsNullOrWhiteSpace(catalogIdText) || !int.TryParse(catalogIdText, out int catalogId))
            {
                MessageBox.Show("Please enter a valid Catalog ID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Step 1: Check if the movie is available
                    string checkAvailabilityQuery = @"
                SELECT MC.Movie_number, MC.Quantity, C.Branch_Number, C.Customer_ID
                FROM Movie_Copy MC
                JOIN Customer C ON MC.Branch_Number = C.Branch_Number
                WHERE MC.Catalog_no = @CatalogId AND C.Person_ID = @PersonID";

                    SqlCommand checkCommand = new SqlCommand(checkAvailabilityQuery, connection);
                    checkCommand.Parameters.AddWithValue("@CatalogId", catalogId);
                    checkCommand.Parameters.AddWithValue("@PersonID", _personId);

                    SqlDataReader reader = checkCommand.ExecuteReader();

                    if (reader.Read())
                    {
                        int movieNumber = reader.GetInt32(0);
                        int quantity = reader.GetInt32(1);
                        int branchNumber = reader.GetInt32(2);
                        int customerId = reader.GetInt32(3);

                        reader.Close();

                        if (quantity > 0)
                        {
                            // Step 2: Insert into Request_Return table
                            string insertQuery = @"
                        INSERT INTO Request_Return (User_ID, Customer_ID, Branch_ID, Movie_Number, Status_of)
                        VALUES (@UserID, @CustomerID, @BranchID, @MovieNumber, 'Request')";

                            SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                            insertCommand.Parameters.AddWithValue("@UserID", _personId);
                            insertCommand.Parameters.AddWithValue("@CustomerID", customerId);
                            insertCommand.Parameters.AddWithValue("@BranchID", branchNumber);
                            insertCommand.Parameters.AddWithValue("@MovieNumber", movieNumber);

                            insertCommand.ExecuteNonQuery();

                            MessageBox.Show("Movie request submitted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("The selected movie is not available.", "Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No such movie found in your branch.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while processing your request: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void branch_filter_TextChanged(object sender, EventArgs e)
        {
            LoadFilteredMovies();
        }

        private void name_filter_TextChanged(object sender, EventArgs e)
        {
            LoadFilteredMovies();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            LoadFilteredMovies();
        }

        private void LoadFilteredMovies()
        {
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            string query = @"
        SELECT 
            M.Catalog_no AS 'Catalog ID',
            M.Title AS 'Movie Title',
            MC.Quantity AS 'Available Quantity',
            MC.Status AS 'Status',
            M.Daily_Rental_Cost AS 'Daily Rent'
        FROM Movie M
        JOIN Movie_Copy MC ON M.Catalog_no = MC.Catalog_no
        JOIN Customer C ON MC.Branch_Number = C.Branch_Number
        WHERE C.Person_ID = @PersonID";

            if (!string.IsNullOrWhiteSpace(branch_filter.Text))
            {
                query += " AND M.Category LIKE @Category";
            }

            if (!string.IsNullOrWhiteSpace(name_filter.Text))
            {
                query += " AND M.Title LIKE @Title";
            }

            if (!string.IsNullOrWhiteSpace(textBox3.Text))
            {
                query += " AND MC.Status LIKE @Status";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PersonID", _personId);

                    if (!string.IsNullOrWhiteSpace(branch_filter.Text))
                    {
                        command.Parameters.AddWithValue("@Category", "%" + branch_filter.Text + "%");
                    }

                    if (!string.IsNullOrWhiteSpace(name_filter.Text))
                    {
                        command.Parameters.AddWithValue("@Title", "%" + name_filter.Text + "%");
                    }

                    if (!string.IsNullOrWhiteSpace(textBox3.Text))
                    {
                        command.Parameters.AddWithValue("@Status", "%" + textBox3.Text + "%");
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while filtering movies: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            LoadAllActors();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            LoadFilteredActors();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadFilteredActors();
        }

        private void LoadAllActors()
        {
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            string query = @"
        SELECT 
            A.Full_name AS 'Actor Name',
            P.Role_Name AS 'Role',
            M.Title AS 'Movie Title'
        FROM Actor A
        JOIN Plays_in P ON A.ID = P.Actor_ID
        JOIN Movie M ON P.Catalog_no = M.Catalog_no
        JOIN Movie_Copy MC ON M.Catalog_no = MC.Catalog_no
        JOIN Customer C ON MC.Branch_Number = C.Branch_Number
        WHERE C.Person_ID = @PersonID";

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

                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading actors: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadFilteredActors()
        {
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            string query = @"
        SELECT 
            A.Full_name AS 'Actor Name',
            P.Role_Name AS 'Role',
            M.Title AS 'Movie Title'
        FROM Actor A
        JOIN Plays_in P ON A.ID = P.Actor_ID
        JOIN Movie M ON P.Catalog_no = M.Catalog_no
        JOIN Movie_Copy MC ON M.Catalog_no = MC.Catalog_no
        JOIN Customer C ON MC.Branch_Number = C.Branch_Number
        WHERE C.Person_ID = @PersonID";

            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                query += " AND M.Title LIKE @MovieTitle";
            }

            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                query += " AND A.Full_name LIKE @ActorName";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PersonID", _personId);

                    if (!string.IsNullOrWhiteSpace(textBox2.Text))
                    {
                        command.Parameters.AddWithValue("@MovieTitle", "%" + textBox2.Text + "%");
                    }

                    if (!string.IsNullOrWhiteSpace(textBox1.Text))
                    {
                        command.Parameters.AddWithValue("@ActorName", "%" + textBox1.Text + "%");
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while filtering actors: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        bool filter_bar = true;
        private void filter_trans_Tick(object sender, EventArgs e)
        {
            if (!filter_bar)
            {
                panel1.Height += 10;
                if (panel1.Height >= 198)
                {
                    filter_trans.Stop();
                    filter_bar = true;
                }
            }
            else
            {
                panel1.Height -= 10;
                if (panel1.Height <= 43)
                {
                    filter_trans.Stop();
                    filter_bar = false;
                }
            }
        }
    }
}
