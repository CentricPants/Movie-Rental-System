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
    public partial class Movie_Browse : Form
    {
        private int _personId;
        private int _currentIndex = 0;
        private DataTable _movies;
        private string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

        public Movie_Browse(int personId)
        {
            _personId = personId;
            InitializeComponent();
            LoadMovies();
            LoadMoviesToDataGrid(); // Load movies initially

        }
        private void LoadMovies()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                    SELECT 
                        M.Catalog_no, 
                        M.Title, 
                        MC.Quantity, 
                        MC.Status, 
                        M.Daily_Rental_Cost, 
                        M.Poster
                    FROM Movie M
                    JOIN Movie_Copy MC ON M.Catalog_no = MC.Catalog_no
                    WHERE MC.Branch_Number = (
                        SELECT Branch_Number
                        FROM Customer
                        WHERE Person_ID = @PersonID
                    )";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PersonID", _personId);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    _movies = new DataTable();
                    adapter.Fill(_movies);

                    if (_movies.Rows.Count > 0)
                    {
                        _currentIndex = 0;
                        DisplayMovie(_currentIndex);
                    }
                    else
                    {
                        MessageBox.Show("No movies found for your branch.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading movies: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DisplayMovie(int index)
        {
            if (index >= 0 && index < _movies.Rows.Count)
            {
                DataRow movie = _movies.Rows[index];

                label2.Text = movie["Catalog_no"].ToString(); // Catalog No
                label7.Text = movie["Title"].ToString();      // Title
                label5.Text = movie["Quantity"].ToString();   // Quantity
                label8.Text = movie["Status"].ToString();     // Status
                label9.Text = movie["Daily_Rental_Cost"].ToString(); // Daily Rent

                // Load the poster image if available
                if (movie["Poster"] != DBNull.Value)
                {
                    byte[] imageBytes = (byte[])movie["Poster"];
                    using (var ms = new System.IO.MemoryStream(imageBytes))
                    {
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    pictureBox1.Image = null; // Clear the picture box if no image
                }
            }
        }



        private void button3_Click(object sender, EventArgs e)
        {
            // Show previous movie
            if (_currentIndex > 0)
            {
                _currentIndex--;
                DisplayMovie(_currentIndex);
            }
            else
            {
                MessageBox.Show("You are already viewing the first movie.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            // Validate if a movie is currently displayed
            if (string.IsNullOrWhiteSpace(label2.Text))
            {
                MessageBox.Show("Please select a valid movie to view its actors.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string catalogNo = label2.Text.Trim();
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Query to fetch actors of the current movie
                    string query = @"
            SELECT 
                A.ID AS 'Actor ID', 
                A.Full_name AS 'Actor Name', 
                A.Gender AS 'Gender', 
                P.Role_Name AS 'Role'
            FROM Actor A
            JOIN Plays_in P ON A.ID = P.Actor_ID
            WHERE P.Catalog_no = @CatalogNo";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CatalogNo", catalogNo);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Display data in dataGridView1
                    dataGridView1.DataSource = dataTable;

                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("No actors found for this movie.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while fetching actors: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            //catalog no

        }

        private void label7_Click(object sender, EventArgs e)
        {
            //title
        }

        private void label5_Click(object sender, EventArgs e)
        {
            //quantity
        }

        private void label8_Click(object sender, EventArgs e)
        {
            //status
        }

        private void label9_Click(object sender, EventArgs e)
        {
            //daily rent
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //picture of the movie
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Show next movie
            if (_currentIndex < _movies.Rows.Count - 1)
            {
                _currentIndex++;
                DisplayMovie(_currentIndex);
            }
            else
            {
                MessageBox.Show("You are already viewing the last movie.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Validate if a movie is currently displayed
            if (string.IsNullOrWhiteSpace(label2.Text))
            {
                MessageBox.Show("Please select a valid movie to view its actors.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string catalogNo = label2.Text.Trim();
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Query to fetch actors of the current movie
                    string query = @"
            SELECT 
                A.ID AS 'Actor ID', 
                A.Full_name AS 'Actor Name', 
                A.Gender AS 'Gender', 
                P.Role_Name AS 'Role'
            FROM Actor A
            JOIN Plays_in P ON A.ID = P.Actor_ID
            WHERE P.Catalog_no = @CatalogNo";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CatalogNo", catalogNo);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Display data in dataGridView1
                    dataGridView1.DataSource = dataTable;

                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("No actors found for this movie.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while fetching actors: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Validate if a movie is currently displayed
            if (string.IsNullOrWhiteSpace(label2.Text))
            {
                MessageBox.Show("Please select a valid movie to view its actors.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string catalogNo = label2.Text.Trim();
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Query to fetch actors of the current movie
                    string query = @"
            SELECT 
                A.ID AS 'Actor ID', 
                A.Full_name AS 'Actor Name', 
                A.Gender AS 'Gender', 
                P.Role_Name AS 'Role'
            FROM Actor A
            JOIN Plays_in P ON A.ID = P.Actor_ID
            WHERE P.Catalog_no = @CatalogNo";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CatalogNo", catalogNo);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Display data in dataGridView1
                    dataGridView1.DataSource = dataTable;

                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("No actors found for this movie.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while fetching actors: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            // Validate that there is a movie displayed
            if (string.IsNullOrWhiteSpace(label2.Text) ||
                string.IsNullOrWhiteSpace(label5.Text) ||
                string.IsNullOrWhiteSpace(label8.Text))
            {
                MessageBox.Show("Please select a valid movie before making a request.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string catalogNo = label2.Text.Trim();
            string quantityText = label5.Text.Trim();
            string status = label8.Text.Trim();

            // Validate movie availability
            if (!int.TryParse(quantityText, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("This movie is currently unavailable for request.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Fetch necessary branch and customer information
                    string fetchDetailsQuery = @"
            SELECT C.Customer_ID, C.Branch_Number
            FROM Customer C
            WHERE C.Person_ID = @PersonID";

                    SqlCommand fetchCommand = new SqlCommand(fetchDetailsQuery, connection);
                    fetchCommand.Parameters.AddWithValue("@PersonID", _personId);

                    SqlDataReader reader = fetchCommand.ExecuteReader();

                    if (!reader.Read())
                    {
                        MessageBox.Show("Customer information not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        reader.Close();
                        return;
                    }

                    int customerId = reader.GetInt32(0);
                    int branchNumber = reader.GetInt32(1);
                    reader.Close();

                    // Insert into Request_Return table
                    string insertQuery = @"
            INSERT INTO Request_Return (User_ID, Customer_ID, Branch_ID, Movie_Number, Status_of)
            SELECT 
                @UserID, 
                @CustomerID, 
                @BranchID, 
                MC.Movie_number, 
                'Request'
            FROM Movie_Copy MC
            WHERE MC.Catalog_no = @CatalogNo AND MC.Branch_Number = @BranchID";

                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@UserID", _personId);
                    insertCommand.Parameters.AddWithValue("@CustomerID", customerId);
                    insertCommand.Parameters.AddWithValue("@BranchID", branchNumber);
                    insertCommand.Parameters.AddWithValue("@CatalogNo", catalogNo);

                    int rowsAffected = insertCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Request submitted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Optionally decrement the quantity in Movie_Copy
                        string updateQuantityQuery = @"
                UPDATE Movie_Copy
                SET Quantity = Quantity - 1
                WHERE Catalog_no = @CatalogNo AND Branch_Number = @BranchID";

                        SqlCommand updateCommand = new SqlCommand(updateQuantityQuery, connection);
                        updateCommand.Parameters.AddWithValue("@CatalogNo", catalogNo);
                        updateCommand.Parameters.AddWithValue("@BranchID", branchNumber);
                        updateCommand.ExecuteNonQuery();

                        // Refresh displayed quantity
                        label5.Text = (quantity - 1).ToString();
                    }
                    else
                    {
                        MessageBox.Show("Failed to create a request. The movie might not exist in your branch.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while submitting the request: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void FilterMovies()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
            SELECT 
                M.Catalog_no AS 'Catalog No', 
                M.Title AS 'Title', 
                MC.Quantity AS 'Quantity', 
                MC.Status AS 'Status', 
                M.Daily_Rental_Cost AS 'Daily Rent'
            FROM Movie M
            JOIN Movie_Copy MC ON M.Catalog_no = MC.Catalog_no
            WHERE MC.Branch_Number = (
                SELECT Branch_Number
                FROM Customer
                WHERE Person_ID = @PersonID
            )";

                    // Apply filters if provided
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

                    // Bind filtered data to dataGridView2
                    dataGridView2.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while filtering movies: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void branch_filter_TextChanged(object sender, EventArgs e)
        {
            FilterMovies();//category filter textbox
        }

        private void name_filter_TextChanged(object sender, EventArgs e)
        {
            FilterMovies();  //movie name
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            FilterMovies(); //status
        }




        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void LoadMoviesToDataGrid()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
            SELECT 
                M.Catalog_no AS 'Catalog No', 
                M.Title AS 'Title', 
                MC.Quantity AS 'Quantity', 
                MC.Status AS 'Status', 
                M.Daily_Rental_Cost AS 'Daily Rent'
            FROM Movie M
            JOIN Movie_Copy MC ON M.Catalog_no = MC.Catalog_no
            WHERE MC.Branch_Number = (
                SELECT Branch_Number
                FROM Customer
                WHERE Person_ID = @PersonID
            )";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PersonID", _personId);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Bind data to dataGridView2
                    dataGridView2.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading movies: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //load all the movies in the branch inside this data too and the filter should only work on this 
        }
    }
}
