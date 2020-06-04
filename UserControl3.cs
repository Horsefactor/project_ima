using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace project_ima
{
    public partial class UserControl3 : UserControl
    {
        //connection object to db
        public string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;
                                           Data Source= sgbd.accdb";

        public OleDbConnection dbConn;

        //string command
        public string queryString;

        // declare oleDb command object
        public OleDbCommand cmd;

        // declare the database reader
        public OleDbDataReader dbReader;


        public UserControl3()
        {
            InitializeComponent();
            LoadInventory();
            
        }

        private void LoadInventory()
        {
            try
            {
                queryString = "SELECT * FROM stock";

                dbConn = new OleDbConnection(connectionString);
                // open connection
                dbConn.Open();
                // next instructions
                cmd = new OleDbCommand(queryString, dbConn);
                dbReader = null;  
                dbReader = cmd.ExecuteReader();

                DataTable dt = new DataTable();

                dt.Load(dbReader);

                //show the data table in datagrid
                dataGridView1.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source " + ex.ToString());
            }
            finally
            {
                // Disconnect Database
                dbConn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadInventory();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
