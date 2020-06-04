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
using System.Text.RegularExpressions;

namespace project_ima
{
    public partial class AddPrint : UserControl
    {
        //connection object to db
        public string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;
                                           Data Source= sgbd.accdb";

        public OleDbConnection dbConn;

        // declare the string command
        public string queryString;

        // declare oleDb command object
        public OleDbCommand cmd = new OleDbCommand();

        //database reader
        public OleDbDataReader dbReader;
        public int quantity;

        public AddPrint()
        {
            InitializeComponent();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string st_name = comboBox1.Text + "_" + comboBox2.Text;


            if (Regex.IsMatch(textBox1.Text, @"^[0-9-]+$")){
                quantity = int.Parse(textBox1.Text);
                try
                {
                    dbConn = new OleDbConnection(connectionString);

                    cmd = dbConn.CreateCommand();
                    cmd.CommandText = "SELECT quantity FROM stock WHERE st_name = @st_name";
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@st_name", st_name);

                    dbConn.Open();

                    dbReader = cmd.ExecuteReader();

                    int dbQuantity = 0;

                    if (dbReader.Read())
                    {

                        dbQuantity = int.Parse(dbReader["quantity"].ToString());

                        string name = quantity + " " + comboBox1.Text + " " + comboBox2.Text;

                        if (dbQuantity >= quantity)
                        {
                            int newQuantity = dbQuantity - quantity;

                            cmd = null;
                            cmd = dbConn.CreateCommand();
                            cmd.CommandText = "UPDATE stock SET quantity = quantity - @quantity WHERE st_name = @st_name";
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@quantity", quantity);
                            cmd.Parameters.AddWithValue("@st_name", st_name);

                            dataGridView1.Rows.Add(name, "printing...", "/");
                            // execute command
                            cmd.ExecuteNonQuery();
                            {
                                MessageBox.Show("Encoding successfull !");
                            }
                        }
                        else
                        {
                            dataGridView1.Rows.Add(name, "not printing", "not enough paper");
                            MessageBox.Show("Not enough paper in stock");
                        }
                    }
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
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
