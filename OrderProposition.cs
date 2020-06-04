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
    public partial class OrderProposition : UserControl
    {
        //connection object to db
        public string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;
                                           Data Source= sgbd.accdb";

        public OleDbConnection dbConn;


        // declare oleDb command object
        public OleDbCommand cmd;

        // declare the database reader
        public OleDbDataReader dbReader = null;

        public OrderProposition()
        {
            InitializeComponent();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void LoadAdvice()
        {
            if(Regex.IsMatch(textBox1.Text, @"^[0-9-]+$") && Regex.IsMatch(textBox2.Text, @"^[0-9-]+$"))
            {
                try
                {
                    dbConn = new OleDbConnection(connectionString);

                    cmd = dbConn.CreateCommand();

                    cmd.CommandText = "SELECT supplyer.id, supplyer.sp_name, stock.type, C.price, C.delevery_time, stock.id, stock.st_name, stock.quantity " +
                                        "FROM supplyer INNER JOIN(stock INNER JOIN[catalog] AS C ON stock.id = C.id_stock) " +
                                        "ON(supplyer.id = C.id_supplyer) AND(supplyer.id = C.id_supplyer) " +
                                        "WHERE(stock.quantity < @INK_SAFE_LIMIT " +
                                        "AND stock.type = @INK) " +
                                        "OR(stock.quantity < @PAPER_SAFE_LIMIT " +
                                        "AND stock.type = @PAPER) " +
                                        "ORDER BY C.price ";


                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@INK_SAFE_LIMIT", int.Parse(textBox2.Text));
                    cmd.Parameters.AddWithValue("@INK", "ink");
                    cmd.Parameters.AddWithValue("@PAPER_SAFE_LIMIT", int.Parse(textBox1.Text));
                    cmd.Parameters.AddWithValue("@PAPER", "paper");


                    //open connection
                    dbConn.Open();

                    //read data
                    dbReader = cmd.ExecuteReader();



                    DataTable dtSafe = new DataTable();

                    dtSafe.Load(dbReader);

                    //show the safe data table in datagrid
                    dataGridView1.DataSource = dtSafe;

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

        private void button1_Click(object sender, EventArgs e)
        {
            LoadAdvice();
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    if (MainForm.Instance.PnlContainer.Controls.ContainsKey("UserControl5"))
                    {
                        MainForm.Instance.PnlContainer.Controls.RemoveAt(1);
                    }

                    OrderForm.datagridRow = dataGridView1.Rows[e.RowIndex]; 

                    if (!MainForm.Instance.PnlContainer.Controls.ContainsKey("UserControl5"))
                    {
                        OrderForm userControl5 = new OrderForm();
                        userControl5.Dock = DockStyle.Fill;
                        MainForm.Instance.PnlContainer.Controls.Add(userControl5);
                        
                    }

                    MainForm.Instance.PnlContainer.Controls["UserControl5"].BringToFront();
                    MainForm.Instance.BackButton.Visible = true;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}