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
    public partial class admin_movie : Form
    {
        public admin_movie()
        {
            InitializeComponent();
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void InsertBTN_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            // Query to show all movies with details
            string query = @"
        SELECT 
            M.Catalog_no AS 'Catalog No', 
            M.Title AS 'Title', 
            M.Director AS 'Director', 
            M.Category AS 'Category', 
            M.Rating AS 'Rating', 
            M.Daily_Rental_Cost AS 'Daily Rental Cost'
        FROM Movie M";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Bind to DataGridView
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while fetching movies: " + ex.Message);
                }
            }
        }



        private void branch_filter_TextChanged(object sender, EventArgs e)
        {
            LoadMovies();
        }

        private void name_filter_TextChanged(object sender, EventArgs e)
        {
            LoadMovies();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            LoadMovies();
        }

        private void LoadMovies()
        {
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            string query = @"
        SELECT 
            M.Catalog_no AS 'Catalog No', 
            M.Title AS 'Title', 
            M.Director AS 'Director', 
            M.Category AS 'Category', 
            M.Rating AS 'Rating', 
            M.Daily_Rental_Cost AS 'Daily Rental Cost'
        FROM Movie M
        LEFT JOIN Movie_Copy MC ON M.Catalog_no = MC.Catalog_no
        LEFT JOIN Branch B ON MC.Branch_Number = B.Branch_Number
        WHERE 1=1";

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
                query += " AND B.City LIKE @BranchCity";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);

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
                        command.Parameters.AddWithValue("@BranchCity", "%" + textBox3.Text + "%");
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Bind to DataGridView
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while fetching filtered movies: " + ex.Message);
                }
            }
        }

      private void button1_Click(object sender, EventArgs e)
            {
                string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

                // Query to show all actors with details
                string query = @"
        SELECT 
            A.ID AS 'Actor ID', 
            A.Full_name AS 'Full Name', 
            A.Gender AS 'Gender'
        FROM Actor A";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind to DataGridView
                        dataGridView1.DataSource = dataTable;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while fetching actors: " + ex.Message);
                    }
                }
            }

        


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadActors();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            LoadActors();
        }

        private void LoadActors()
        {
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            string query = @"
        SELECT 
            A.ID AS 'Actor ID', 
            A.Full_name AS 'Full Name', 
            A.Gender AS 'Gender', 
            P.Role_Name AS 'Role', 
            M.Title AS 'Movie'
        FROM Actor A
        LEFT JOIN Plays_in P ON A.ID = P.Actor_ID
        LEFT JOIN Movie M ON P.Catalog_no = M.Catalog_no
        WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                query += " AND A.Full_name LIKE @ActorName";
            }
            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                query += " AND M.Title LIKE @MovieTitle";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);

                    if (!string.IsNullOrWhiteSpace(textBox1.Text))
                    {
                        command.Parameters.AddWithValue("@ActorName", "%" + textBox1.Text + "%");
                    }
                    if (!string.IsNullOrWhiteSpace(textBox2.Text))
                    {
                        command.Parameters.AddWithValue("@MovieTitle", "%" + textBox2.Text + "%");
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Bind to DataGridView
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while fetching filtered actors: " + ex.Message);
                }
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string title = textBox6.Text.Trim();      // Movie title
            string director = textBox5.Text.Trim();  // Movie director
            string category = textBox11.Text.Trim(); // Movie category
            string rating = textBox4.Text.Trim();    // Movie rating
            string rentalCostText = textBox7.Text.Trim(); // Rental cost
            string catalogIdText = textBox12.Text.Trim(); // Catalog ID for updating

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(director) ||
                string.IsNullOrWhiteSpace(category) || string.IsNullOrWhiteSpace(rating) ||
                string.IsNullOrWhiteSpace(rentalCostText) || !decimal.TryParse(rentalCostText, out decimal rentalCost) ||
                string.IsNullOrWhiteSpace(catalogIdText) || !int.TryParse(catalogIdText, out int catalogId))
            {
                MessageBox.Show("Please fill in all fields with valid data.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Check if the movie exists
                    string checkMovieQuery = "SELECT COUNT(*) FROM Movie WHERE Catalog_no = @CatalogNo";
                    SqlCommand checkMovieCommand = new SqlCommand(checkMovieQuery, connection);
                    checkMovieCommand.Parameters.AddWithValue("@CatalogNo", catalogId);

                    int movieExists = (int)checkMovieCommand.ExecuteScalar();

                    if (movieExists > 0)
                    {
                        // Update the existing movie
                        string updateMovieQuery = @"
                    UPDATE Movie
                    SET Title = @Title,
                        Director = @Director,
                        Category = @Category,
                        Rating = @Rating,
                        Daily_Rental_Cost = @RentalCost,
                        Poster = @Poster
                    WHERE Catalog_no = @CatalogNo";

                        SqlCommand updateMovieCommand = new SqlCommand(updateMovieQuery, connection);
                        updateMovieCommand.Parameters.AddWithValue("@Title", title);
                        updateMovieCommand.Parameters.AddWithValue("@Director", director);
                        updateMovieCommand.Parameters.AddWithValue("@Category", category);
                        updateMovieCommand.Parameters.AddWithValue("@Rating", rating);
                        updateMovieCommand.Parameters.AddWithValue("@RentalCost", rentalCost);
                        updateMovieCommand.Parameters.AddWithValue("@CatalogNo", catalogId);

                        // Convert the image to a byte array if a poster is provided
                        if (pictureBox1.Image != null)
                        {
                            using (var memoryStream = new System.IO.MemoryStream())
                            {
                                pictureBox1.Image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                                updateMovieCommand.Parameters.AddWithValue("@Poster", memoryStream.ToArray());
                            }
                        }
                        else
                        {
                            updateMovieCommand.Parameters.AddWithValue("@Poster", DBNull.Value);
                        }

                        updateMovieCommand.ExecuteNonQuery();
                        MessageBox.Show("Movie information updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Insert a new movie
                        string insertMovieQuery = @"
                    INSERT INTO Movie (Catalog_no, Title, Director, Category, Rating, Daily_Rental_Cost, Poster)
                    VALUES (@CatalogNo, @Title, @Director, @Category, @Rating, @RentalCost, @Poster)";

                        SqlCommand insertMovieCommand = new SqlCommand(insertMovieQuery, connection);
                        insertMovieCommand.Parameters.AddWithValue("@CatalogNo", catalogId);
                        insertMovieCommand.Parameters.AddWithValue("@Title", title);
                        insertMovieCommand.Parameters.AddWithValue("@Director", director);
                        insertMovieCommand.Parameters.AddWithValue("@Category", category);
                        insertMovieCommand.Parameters.AddWithValue("@Rating", rating);
                        insertMovieCommand.Parameters.AddWithValue("@RentalCost", rentalCost);

                        // Convert the image to a byte array if a poster is provided
                        if (pictureBox1.Image != null)
                        {
                            using (var memoryStream = new System.IO.MemoryStream())
                            {
                                pictureBox1.Image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                                insertMovieCommand.Parameters.AddWithValue("@Poster", memoryStream.ToArray());
                            }
                        }
                        else
                        {
                            insertMovieCommand.Parameters.AddWithValue("@Poster", DBNull.Value);
                        }

                        insertMovieCommand.ExecuteNonQuery();
                        MessageBox.Show("New movie added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    // Refresh the movie list
                    InsertBTN_Click(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while saving the movie information: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void admin_movie_Load(object sender, EventArgs e)
        {
            LoadActorsComboBox();
            LoadMoviesComboBox();
        }
        private void LoadActorsComboBox()
        {
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";
            string query = "SELECT Full_name FROM Actor";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    comboBox2.Items.Clear();
                    while (reader.Read())
                    {
                        comboBox2.Items.Add(reader["Full_name"].ToString());
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading actors: " + ex.Message);
                }
            }
        }

        private void LoadMoviesComboBox()
        {
            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";
            string query = "SELECT Title FROM Movie";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    comboBox1.Items.Clear();
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["Title"].ToString());
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading movies: " + ex.Message);
                }
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            // Id of the movie you want to delete
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            //title of the movie
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            //director of the movie

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //rating of the movie
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            //rental cost
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            //category
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Get the movie ID from the textbox
            string movieIdText = textBox8.Text.Trim();

            // Validate input
            if (string.IsNullOrWhiteSpace(movieIdText) || !int.TryParse(movieIdText, out int movieId))
            {
                MessageBox.Show("Please enter a valid Movie ID.");
                return;
            }

            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Check if the movie exists
                    string checkMovieQuery = "SELECT COUNT(*) FROM Movie WHERE Catalog_no = @CatalogNo";
                    SqlCommand checkMovieCommand = new SqlCommand(checkMovieQuery, connection);
                    checkMovieCommand.Parameters.AddWithValue("@CatalogNo", movieId);

                    int movieExists = (int)checkMovieCommand.ExecuteScalar();
                    if (movieExists == 0)
                    {
                        MessageBox.Show("Movie with the given ID does not exist.");
                        return;
                    }

                    // Delete the movie
                    string deleteMovieQuery = "DELETE FROM Movie WHERE Catalog_no = @CatalogNo";
                    SqlCommand deleteMovieCommand = new SqlCommand(deleteMovieQuery, connection);
                    deleteMovieCommand.Parameters.AddWithValue("@CatalogNo", movieId);
                    deleteMovieCommand.ExecuteNonQuery();

                    MessageBox.Show($"Movie with ID {movieId} has been successfully removed.");

                    // Optionally refresh the DataGridView to show the updated list of movies
                    InsertBTN_Click(sender, e); // Call the method to reload all movies
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while deleting the movie: " + ex.Message);
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //combobox that will have the names of all actors in the actor table but we can input a new name which if not in the table it will insert a new actor
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          //it will contain the title of all the movies for us to choose 
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            //gender of the actor (this is only necessary if the actor is new and not in the actor table befoer)

        }
        

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            // Role that he's playing
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string actorName = comboBox2.Text.Trim();
            string gender = textBox10.Text.Trim();
            string movieTitle = comboBox1.Text.Trim();
            string roleName = textBox9.Text.Trim();

            if (string.IsNullOrWhiteSpace(actorName) || string.IsNullOrWhiteSpace(movieTitle) || string.IsNullOrWhiteSpace(roleName))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Check if actor exists
                    string getActorQuery = "SELECT ID FROM Actor WHERE Full_name = @ActorName";
                    SqlCommand getActorCommand = new SqlCommand(getActorQuery, connection);
                    getActorCommand.Parameters.AddWithValue("@ActorName", actorName);

                    object actorIdObj = getActorCommand.ExecuteScalar();
                    int actorId;

                    if (actorIdObj == null) // Actor does not exist, insert into Actor table
                    {
                        if (string.IsNullOrWhiteSpace(gender))
                        {
                            MessageBox.Show("Please specify gender for new actor.");
                            return;
                        }

                        // Get the next Actor ID
                        string getNextActorIdQuery = "SELECT ISNULL(MAX(ID), 0) + 1 FROM Actor";
                        SqlCommand getNextActorIdCommand = new SqlCommand(getNextActorIdQuery, connection);
                        actorId = (int)getNextActorIdCommand.ExecuteScalar();

                        // Insert new actor
                        string insertActorQuery = "INSERT INTO Actor (ID, Full_name, Gender) VALUES (@ActorId, @ActorName, @Gender)";
                        SqlCommand insertActorCommand = new SqlCommand(insertActorQuery, connection);
                        insertActorCommand.Parameters.AddWithValue("@ActorId", actorId);
                        insertActorCommand.Parameters.AddWithValue("@ActorName", actorName);
                        insertActorCommand.Parameters.AddWithValue("@Gender", gender);
                        insertActorCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        actorId = (int)actorIdObj; // Actor already exists
                    }

                    // Get the Catalog_no for the selected movie
                    string getMovieCatalogQuery = "SELECT Catalog_no FROM Movie WHERE Title = @MovieTitle";
                    SqlCommand getMovieCatalogCommand = new SqlCommand(getMovieCatalogQuery, connection);
                    getMovieCatalogCommand.Parameters.AddWithValue("@MovieTitle", movieTitle);

                    object catalogNoObj = getMovieCatalogCommand.ExecuteScalar();
                    if (catalogNoObj == null)
                    {
                        MessageBox.Show("Selected movie does not exist.");
                        return;
                    }
                    int catalogNo = (int)catalogNoObj;

                    // Insert into Plays_in table
                    string insertPlaysInQuery = "INSERT INTO Plays_in (Actor_ID, Catalog_no, Role_Name) VALUES (@ActorId, @CatalogNo, @RoleName)";
                    SqlCommand insertPlaysInCommand = new SqlCommand(insertPlaysInQuery, connection);
                    insertPlaysInCommand.Parameters.AddWithValue("@ActorId", actorId);
                    insertPlaysInCommand.Parameters.AddWithValue("@CatalogNo", catalogNo);
                    insertPlaysInCommand.Parameters.AddWithValue("@RoleName", roleName);
                    insertPlaysInCommand.ExecuteNonQuery();

                    MessageBox.Show($"Actor '{actorName}' has been successfully associated with the role '{roleName}' in '{movieTitle}'.");

                    // Refresh the actor combo box
                    LoadActorsComboBox();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while adding actor and role: " + ex.Message);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog.FileName);
            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            //enter the catalog id of a movie to update its value
        }
    }
}
