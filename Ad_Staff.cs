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
    public partial class Ad_Staff : Form
    {
        public Ad_Staff()
        {
            InitializeComponent();
        }
        private string connectionString = "Data Source=Lenovo_Ideapad3\\SQLEXPRESS;Initial Catalog=proj;Integrated Security=True";

        private void Ad_Staff_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'projDataSet1.Staff' table. You can move, or remove it, as needed.
            this.staffTableAdapter.Fill(this.projDataSet1.Staff);
          

            // Load branches into the combo box
            LoadBranches();

        }

        private void InsertBTN_Click(object sender, EventArgs e)
        {

            string query = @"
        SELECT 
            U.Username,
            S.First_name, 
            S.Last_name, 
            S.Position, 
            B.City AS Branch_City
        FROM Staff S
        JOIN Branch B ON S.Branch_Number = B.Branch_Number
        JOIN Users U ON S.Person_ID = U.Person_ID";


            // Fetch and bind data
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
       

    }

    private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            filter_trans.Start();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

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

        private void branch_filter_TextChanged(object sender, EventArgs e)
        {
            LoadStaffData();
        }

        private void name_filter_TextChanged(object sender, EventArgs e)
        {
            LoadStaffData();
        }

        private void role_filter_TextChanged(object sender, EventArgs e)
        {
            LoadStaffData();
        }

        private void label3_Click(object sender, EventArgs e)
        {
           
        }
        private void LoadStaffData()
        {

            string query = @"SELECT S.First_name, S.Last_name, S.Position, B.City AS Branch_City
                     FROM Staff S
                     JOIN Branch B ON S.Branch_Number = B.Branch_Number WHERE 1=1";

            // Append filters dynamically based on input
            if (!string.IsNullOrWhiteSpace(branch_filter.Text))
            {
                query += " AND B.City LIKE @BranchCity";
            }
            if (!string.IsNullOrWhiteSpace(name_filter.Text))
            {
                query += " AND (S.First_name LIKE @Name OR S.Last_name LIKE @Name)";
            }
            if (!string.IsNullOrWhiteSpace(role_filter.Text))
            {
                query += " AND S.Position LIKE @Role";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);

                    // Add parameters dynamically
                    if (!string.IsNullOrWhiteSpace(branch_filter.Text))
                    {
                        command.Parameters.AddWithValue("@BranchCity", "%" + branch_filter.Text + "%");
                    }
                    if (!string.IsNullOrWhiteSpace(name_filter.Text))
                    {
                        command.Parameters.AddWithValue("@Name", "%" + name_filter.Text + "%");
                    }
                    if (!string.IsNullOrWhiteSpace(role_filter.Text))
                    {
                        command.Parameters.AddWithValue("@Role", "%" + role_filter.Text + "%");
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void InsertStaff()
        {
            // Get input data
            string username = textBox1.Text.Trim();  // Username
            string password = textBox2.Text.Trim();  // Password
            string firstName = textBox3.Text.Trim();  // First name
            string lastName = textBox4.Text.Trim();  // Last name
            string branchCity = comboBox2.SelectedItem?.ToString();  // Branch
            string position = comboBox1.SelectedItem?.ToString();  // Position
            decimal salary = 3000.00m; // Default salary
            DateTime joiningDate = DateTime.Now; // Current date

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(branchCity) || string.IsNullOrWhiteSpace(position))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Check for unique username
                    string checkUsernameQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    SqlCommand checkUsernameCommand = new SqlCommand(checkUsernameQuery, connection);
                    checkUsernameCommand.Parameters.AddWithValue("@Username", username);
                    int usernameExists = (int)checkUsernameCommand.ExecuteScalar();

                    if (usernameExists > 0)
                    {
                        MessageBox.Show("The username is already taken. Please choose a different username.");
                        return;
                    }

                    // Begin transaction
                    SqlTransaction transaction = connection.BeginTransaction();

                    // Get the next Person_ID
                    string getLastPersonIdQuery = "SELECT ISNULL(MAX(Person_ID), 0) + 1 FROM Users";
                    SqlCommand personIdCommand = new SqlCommand(getLastPersonIdQuery, connection, transaction);
                    int personId = (int)personIdCommand.ExecuteScalar();

                    // Insert into Users table
                    string insertUserQuery = "INSERT INTO Users (Person_ID, Username, Password, Role) VALUES (@PersonID, @Username, @Password, 'Staff')";
                    SqlCommand userCommand = new SqlCommand(insertUserQuery, connection, transaction);
                    userCommand.Parameters.AddWithValue("@PersonID", personId);
                    userCommand.Parameters.AddWithValue("@Username", username);
                    userCommand.Parameters.AddWithValue("@Password", password);
                    userCommand.ExecuteNonQuery();

                    // Get Branch_Number from branch city
                    string getBranchQuery = "SELECT Branch_Number FROM Branch WHERE City = @City";
                    SqlCommand branchCommand = new SqlCommand(getBranchQuery, connection, transaction);
                    branchCommand.Parameters.AddWithValue("@City", branchCity);
                    int branchNumber = (int)branchCommand.ExecuteScalar();

                    // Get the next Staff_ID
                    string getLastStaffIdQuery = "SELECT ISNULL(MAX(Staff_ID), 0) + 1 FROM Staff";
                    SqlCommand staffIdCommand = new SqlCommand(getLastStaffIdQuery, connection, transaction);
                    int staffId = (int)staffIdCommand.ExecuteScalar();

                    // Insert into Staff table
                    string insertStaffQuery = @"
                INSERT INTO Staff (Staff_ID, Person_ID, Branch_Number, First_name, Last_name, Position, Salary, Joining_Date) 
                VALUES (@StaffID, @PersonID, @BranchNumber, @FirstName, @LastName, @Position, @Salary, @JoiningDate)";
                    SqlCommand staffCommand = new SqlCommand(insertStaffQuery, connection, transaction);
                    staffCommand.Parameters.AddWithValue("@StaffID", staffId);
                    staffCommand.Parameters.AddWithValue("@PersonID", personId);
                    staffCommand.Parameters.AddWithValue("@BranchNumber", branchNumber);
                    staffCommand.Parameters.AddWithValue("@FirstName", firstName);
                    staffCommand.Parameters.AddWithValue("@LastName", lastName);
                    staffCommand.Parameters.AddWithValue("@Position", position);
                    staffCommand.Parameters.AddWithValue("@Salary", salary);
                    staffCommand.Parameters.AddWithValue("@JoiningDate", joiningDate);
                    staffCommand.ExecuteNonQuery();

                    // Insert into Manager or Cashier table based on position
                    if (position.Equals("Manager", StringComparison.OrdinalIgnoreCase))
                    {
                        string insertManagerQuery = "INSERT INTO Manager (Staff_ID, Bonus, Number_of_Employees_Supervised) VALUES (@StaffID, @Bonus, @EmployeesSupervised)";
                        SqlCommand managerCommand = new SqlCommand(insertManagerQuery, connection, transaction);
                        managerCommand.Parameters.AddWithValue("@StaffID", staffId);
                        managerCommand.Parameters.AddWithValue("@Bonus", 1000.00m); // Default bonus
                        managerCommand.Parameters.AddWithValue("@EmployeesSupervised", 0); // Default value
                        managerCommand.ExecuteNonQuery();
                    }
                    else if (position.Equals("Cashier", StringComparison.OrdinalIgnoreCase))
                    {
                        string insertCashierQuery = "INSERT INTO Cashier (Staff_ID, Shift_Timing) VALUES (@StaffID, @ShiftTiming)";
                        SqlCommand cashierCommand = new SqlCommand(insertCashierQuery, connection, transaction);
                        cashierCommand.Parameters.AddWithValue("@StaffID", staffId);
                        cashierCommand.Parameters.AddWithValue("@ShiftTiming", "9:00 AM - 5:00 PM"); // Default shift timing
                        cashierCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        throw new Exception("Invalid position selected.");
                    }

                    // Commit transaction
                    transaction.Commit();

                    MessageBox.Show("Staff member successfully added.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }



        private void button3_Click(object sender, EventArgs e)//insert button for creating a staff member
        {
            InsertStaff();

        }

        bool insertbox = true;
        private void insert_trans_Tick(object sender, EventArgs e)
        {
            if (!insertbox)
            {
                panel2.Height += 10;
                if (panel2.Height >= 186)
                {
                    insert_trans.Stop();
                    insertbox = true;
                }
            }
            else
            {
                panel2.Height -= 10;
                if (panel2.Height <= 15)
                {
                    insert_trans.Stop();
                    insertbox = false;
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            insert_trans.Start();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)//username for staff member
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //password for the staff member
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //First name of the staff
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //last name of the staff
        }

        private void LoadBranches()
        {
            // SQL query to fetch branch names
            string query = "SELECT City FROM Branch";

            // Using SQL connection
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    // Clear existing items in the combo box
                    comboBox2.Items.Clear();

                    // Add each branch name to the combo box
                    while (reader.Read())
                    {
                        comboBox2.Items.Add(reader["City"].ToString());
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading branches: " + ex.Message);
                }
            }
        }
        
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //branch (it should show only the branches that are currently in the branch table
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //only offer 2 manager and cashier.
        }

        private void RemoveStaff()
        {
            // Get input data
            string username = textBox1.Text.Trim();  // Username
            string firstName = textBox3.Text.Trim();  // First name
            string lastName = textBox4.Text.Trim();  // Last name
            string branchCity = comboBox2.SelectedItem?.ToString();  // Branch
            string position = comboBox1.SelectedItem?.ToString();  // Position

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(branchCity) ||
                string.IsNullOrWhiteSpace(position))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Begin transaction
                    SqlTransaction transaction = connection.BeginTransaction();

                    // Find the Branch_Number for the given branch city
                    string getBranchQuery = "SELECT Branch_Number FROM Branch WHERE City = @City";
                    SqlCommand branchCommand = new SqlCommand(getBranchQuery, connection, transaction);
                    branchCommand.Parameters.AddWithValue("@City", branchCity);
                    object branchResult = branchCommand.ExecuteScalar();
                    if (branchResult == null)
                    {
                        throw new Exception("Branch not found.");
                    }
                    int branchNumber = (int)branchResult;

                    // Find Person_ID and Staff_ID for the staff to be removed
                    string getStaffQuery = @"
                SELECT S.Person_ID, S.Staff_ID
                FROM Staff S
                JOIN Users U ON S.Person_ID = U.Person_ID
                WHERE U.Username = @Username
                  AND S.First_name = @FirstName
                  AND S.Last_name = @LastName
                  AND S.Position = @Position
                  AND S.Branch_Number = @BranchNumber";
                    SqlCommand staffCommand = new SqlCommand(getStaffQuery, connection, transaction);
                    staffCommand.Parameters.AddWithValue("@Username", username);
                    staffCommand.Parameters.AddWithValue("@FirstName", firstName);
                    staffCommand.Parameters.AddWithValue("@LastName", lastName);
                    staffCommand.Parameters.AddWithValue("@Position", position);
                    staffCommand.Parameters.AddWithValue("@BranchNumber", branchNumber);
                    SqlDataReader reader = staffCommand.ExecuteReader();

                    if (!reader.Read())
                    {
                        throw new Exception("Staff member not found.");
                    }

                    int personId = reader.GetInt32(0);
                    int staffId = reader.GetInt32(1);
                    reader.Close();

                    // Delete from Manager or Cashier table based on position
                    if (position.Equals("Manager", StringComparison.OrdinalIgnoreCase))
                    {
                        string deleteManagerQuery = "DELETE FROM Manager WHERE Staff_ID = @StaffID";
                        SqlCommand deleteManagerCommand = new SqlCommand(deleteManagerQuery, connection, transaction);
                        deleteManagerCommand.Parameters.AddWithValue("@StaffID", staffId);
                        deleteManagerCommand.ExecuteNonQuery();
                    }
                    else if (position.Equals("Cashier", StringComparison.OrdinalIgnoreCase))
                    {
                        string deleteCashierQuery = "DELETE FROM Cashier WHERE Staff_ID = @StaffID";
                        SqlCommand deleteCashierCommand = new SqlCommand(deleteCashierQuery, connection, transaction);
                        deleteCashierCommand.Parameters.AddWithValue("@StaffID", staffId);
                        deleteCashierCommand.ExecuteNonQuery();
                    }

                    // Delete from Staff table
                    string deleteStaffQuery = "DELETE FROM Staff WHERE Staff_ID = @StaffID";
                    SqlCommand deleteStaffCommand = new SqlCommand(deleteStaffQuery, connection, transaction);
                    deleteStaffCommand.Parameters.AddWithValue("@StaffID", staffId);
                    deleteStaffCommand.ExecuteNonQuery();

                    // Delete from Users table
                    string deleteUserQuery = "DELETE FROM Users WHERE Person_ID = @PersonID";
                    SqlCommand deleteUserCommand = new SqlCommand(deleteUserQuery, connection, transaction);
                    deleteUserCommand.Parameters.AddWithValue("@PersonID", personId);
                    deleteUserCommand.ExecuteNonQuery();

                    // Commit transaction
                    transaction.Commit();

                    MessageBox.Show("Staff member successfully removed.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            RemoveStaff();

            //the show all table should be able to shwo the usernames of the staff, not the password
            //the remove should take the username and the firstname, last name, branch, and position of the person and then delete them 
            //remove button should only take in the following, name, last name, position, branch.
        }
    }
}
