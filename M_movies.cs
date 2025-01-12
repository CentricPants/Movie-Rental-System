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
    public partial class M_movies : Form
    {
        private int _personId;
        public M_movies(int personId)
        {
            _personId= personId; 
            InitializeComponent();
        }

        private string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

        private void button1_Click(object sender, EventArgs e)
        {
            // Show all movies in the Movie table
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Movie";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while fetching movies: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Add Movie_Copy to branch or update quantity
            string catalogNumberText = textBox1.Text.Trim();
            string quantityText = textBox2.Text.Trim();

            if (string.IsNullOrWhiteSpace(catalogNumberText) || !int.TryParse(catalogNumberText, out int catalogNumber))
            {
                MessageBox.Show("Please enter a valid Catalog Number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(quantityText) || !int.TryParse(quantityText, out int quantityToAdd) || quantityToAdd <= 0)
            {
                MessageBox.Show("Please enter a valid quantity.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Step 1: Check if the movie already exists in the branch
                    string checkQuery = @"
                        SELECT Quantity 
                        FROM Movie_Copy 
                        WHERE Catalog_no = @CatalogNo 
                          AND Branch_Number = (
                              SELECT Branch_Number FROM Staff WHERE Person_ID = @PersonID
                          )";

                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@CatalogNo", catalogNumber);
                    checkCommand.Parameters.AddWithValue("@PersonID", _personId);

                    object result = checkCommand.ExecuteScalar();

                    if (result != null) // Movie already exists in the branch
                    {
                        int currentQuantity = Convert.ToInt32(result);
                        int newQuantity = currentQuantity + quantityToAdd;

                        if (newQuantity > 25)
                        {
                            MessageBox.Show("Cannot add more. Quantity exceeds the limit of 25.", "Limit Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Update the quantity
                        string updateQuery = @"
                            UPDATE Movie_Copy
                            SET Quantity = @NewQuantity
                            WHERE Catalog_no = @CatalogNo 
                              AND Branch_Number = (
                                  SELECT Branch_Number FROM Staff WHERE Person_ID = @PersonID
                              )";

                        SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@NewQuantity", newQuantity);
                        updateCommand.Parameters.AddWithValue("@CatalogNo", catalogNumber);
                        updateCommand.Parameters.AddWithValue("@PersonID", _personId);

                        updateCommand.ExecuteNonQuery();
                        MessageBox.Show("Movie quantity updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else // Movie does not exist in the branch
                    {
                        // Step 2: Check if the movie exists in the Movie table
                        string checkMovieTableQuery = "SELECT COUNT(*) FROM Movie WHERE Catalog_no = @CatalogNo";
                        SqlCommand checkMovieTableCommand = new SqlCommand(checkMovieTableQuery, connection);
                        checkMovieTableCommand.Parameters.AddWithValue("@CatalogNo", catalogNumber);

                        int movieExistsInMovieTable = (int)checkMovieTableCommand.ExecuteScalar();

                        if (movieExistsInMovieTable > 0) // Movie exists in the Movie table
                        {
                            if (quantityToAdd > 25)
                            {
                                MessageBox.Show("Cannot add more. Quantity exceeds the limit of 25.", "Limit Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            // Fetch the last Movie_number globally (regardless of branch)
                            string fetchLastMovieNumberQuery = @"
            SELECT ISNULL(MAX(Movie_number), 0) + 1 
            FROM Movie_Copy";
                            SqlCommand fetchLastMovieNumberCommand = new SqlCommand(fetchLastMovieNumberQuery, connection);

                            int nextMovieNumber = (int)fetchLastMovieNumberCommand.ExecuteScalar();

                            // Insert the movie into the Movie_Copy table
                            string insertQuery = @"
            INSERT INTO Movie_Copy (Movie_number, Catalog_no, Branch_Number, Quantity, Status)
            VALUES (
                @MovieNumber, 
                @CatalogNo, 
                (SELECT Branch_Number FROM Staff WHERE Person_ID = @PersonID), 
                @Quantity, 
                'Available'
            )";

                            SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                            insertCommand.Parameters.AddWithValue("@MovieNumber", nextMovieNumber);
                            insertCommand.Parameters.AddWithValue("@CatalogNo", catalogNumber);
                            insertCommand.Parameters.AddWithValue("@PersonID", _personId);
                            insertCommand.Parameters.AddWithValue("@Quantity", quantityToAdd);

                            insertCommand.ExecuteNonQuery();
                            MessageBox.Show("Movie added to the branch successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else // Movie does not exist in the Movie table
                        {
                            MessageBox.Show("The movie with the provided Catalog Number does not exist in the Movie table.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //this one takes in the catalog number of the movie
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //this one takes in the amount of movies (quanity)
        }



        private void button3_Click(object sender, EventArgs e)
        {
            // Show movies with their quantities for this branch
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            MC.Movie_number AS 'Movie Number',
                            M.Title AS 'Title', 
                            MC.Quantity AS 'Quantity',
                            MC.Status AS 'Status'
                        FROM Movie_Copy MC
                        JOIN Movie M ON MC.Catalog_no = M.Catalog_no
                        WHERE MC.Branch_Number = (
                            SELECT Branch_Number FROM Staff WHERE Person_ID = @PersonID
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
                    MessageBox.Show("An error occurred while fetching movie information: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
