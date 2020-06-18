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
    public partial class AddStock : UserControl
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

        private int order_id;


        public AddStock()
        {
            InitializeComponent();
            button2.Enabled = false;
            button3.Enabled = false;
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
                        dbConn.Open();
                        /*
                        cmd = dbConn.CreateCommand();
                        dbConn.Open();

                        cmd.CommandText = "UPDATE stock SET quantity = quantity + @quantity WHERE st_name = @st_name";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@quantity", int.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()));
                        cmd.Parameters.AddWithValue("@st_name", dataGridView1.Rows[e.RowIndex].Cells[13].Value.ToString());

                        cmd.ExecuteNonQuery();
                        {
                            MessageBox.Show("Encoding successfull !");
                        }
                        */
                        cmd = dbConn.CreateCommand();

                        cmd.CommandText = "UPDATE detail SET detail.quantity_out = detail.quantity_out - @quantity WHERE detail.id_order = @id_order AND detail.quantity_out = @quantity_out";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@quantity", int.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()));
                        cmd.Parameters.AddWithValue("@id_order", dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
                        cmd.Parameters.AddWithValue("@quantity_out", dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                        cmd.ExecuteNonQuery();
                        {
                            MessageBox.Show("encoding of details successfull !");
                        }
                        LoadForm();
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
                this.order_id = int.Parse(textBox6.Text);
                LoadForm();
                button2.Enabled = true;
                button3.Enabled = true;
            }
        }

        private void LoadForm()
        {
            try
            {
                dbConn = new OleDbConnection(connectionString);
                cmd = dbConn.CreateCommand();
                dbConn.Open();

                cmd.CommandText = "SELECT detail.quantity_out, detail.id_order, order.total_price, supplyer.*, stock.st_name, order.date_creation " +
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
                cmd.Parameters.AddWithValue("@order_id", this.order_id);
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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                dbConn = new OleDbConnection(connectionString);
                cmd = dbConn.CreateCommand();
                dbConn.Open();
                cmd.CommandText = "UPDATE [order] SET state = @state WHERE id = @order_id";

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@state", "closed");
                cmd.Parameters.AddWithValue("@order_id", this.order_id);
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

        private void button3_Click(object sender, EventArgs e)
        {
            LoadForm();
        }
    }
}
