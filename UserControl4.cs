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
    public partial class UserControl4 : UserControl
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
        public OleDbDataReader dbReader = null;

        public UserControl4()
        {
            InitializeComponent();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void UserControl4_Load(object sender, EventArgs e)
        {

        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 0 && Regex.IsMatch(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(), @"^[0-9-]+$"))
                {
                    try
                    {
                        dbConn = new OleDbConnection(connectionString);
                        cmd = dbConn.CreateCommand();
                        dbConn.Open();

                        cmd.CommandText = "UPDATE stock SET quantity = quantity + @quantity WHERE st_name = @st_name";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@quantity", int.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()));
                        cmd.Parameters.AddWithValue("@st_name", dataGridView1.Rows[e.RowIndex].Cells[12].Value.ToString());


                        cmd.ExecuteNonQuery();
                        {
                            MessageBox.Show("Encoding successfull !");
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
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Regex.IsMatch(textBox6.Text, @"^[0-9-]+$"))
            {
                try
                {
                    dbConn = new OleDbConnection(connectionString);
                    cmd = dbConn.CreateCommand();
                    dbConn.Open();

                    cmd.CommandText = "SELECT detail.quantity_out, order.total_price, supplyer.*, stock.st_name, order.date_creation " +
                                  "FROM supplyer " +
                                  "INNER JOIN(stock " +
                                  "INNER JOIN([order] " +
                                  "INNER JOIN detail " +
                                  "ON detail.id_order = order.id) " +
                                  "ON stock.id = detail.id_stock) " +
                                  "ON(supplyer.id = order.id_supplyer) " +
                                  "AND(supplyer.id = order.id_supplyer) " +
                                  "WHERE order.id = @order_id; ";

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@order_id", int.Parse(textBox6.Text));
                    cmd.ExecuteNonQuery();

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
        }
    }
}
