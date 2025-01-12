using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Movie_Rental
{
    public partial class AD_Customer : Form
    {
        public AD_Customer()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void AD_Customer_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'projDataSet.Customer' table. You can move, or remove it, as needed.
            this.customerTableAdapter.Fill(this.projDataSet.Customer);

        }

        private void branch_filter_TextChanged(object sender, EventArgs e)
        {
            //filter by branch
        }

        private void name_filter_TextChanged(object sender, EventArgs e)
        {
            //First name of the customer 
        }
    }
}
