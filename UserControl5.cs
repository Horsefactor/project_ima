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
    public partial class UserControl5 : UserControl
    {
        public static DataGridViewRow datagridRow;

        //connection object to db
        public string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;
                                           Data Source= sgbd.accdb";
        public double total;

        public OleDbConnection dbConn;


        // declare oleDb command object
        public OleDbCommand cmd;

        // declare the database reader
        public OleDbDataReader dbReader = null;

        public UserControl5()
        {
            InitializeComponent();
            label19.Text = datagridRow.Cells[2].Value.ToString();
            label20.Text = datagridRow.Cells[6].Value.ToString();
            label8.Text = datagridRow.Cells[4].Value.ToString() + " €";
            label6.Text = datagridRow.Cells[7].Value.ToString();

            if (datagridRow.Cells[3].Value.ToString() == "ink") label7.Text = "Price for 10L :"; else label7.Text = "Price for 1000 sheets :";

            LoadSupplyerData();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void LoadSupplyerData ()
        {
            try
            {
                dbConn = new OleDbConnection(connectionString);
                cmd = dbConn.CreateCommand();
                cmd.CommandText = "SELECT supplyer.street_number, supplyer.city, supplyer.street_name, supplyer.zip_code, supplyer.city, supplyer.email, supplyer.number " +
                                  "FROM supplyer "+
                                  "WHERE supplyer.id = @sp_id";

                cmd.CommandType = CommandType.Text;

                Console.WriteLine(int.Parse(datagridRow.Cells[1].Value.ToString()));

                cmd.Parameters.AddWithValue("@sp_id", int.Parse(datagridRow.Cells[1].Value.ToString()));

                //open connection
                dbConn.Open();

                
                dbReader = cmd.ExecuteReader();

                dbReader.Read();

                label20.Text = dbReader["street_number"].ToString() + " " + dbReader["street_name"].ToString() + "\n" + dbReader["zip_code"].ToString() + "\n" + dbReader["city"].ToString();
                label21.Text = dbReader["number"].ToString();
                label22.Text = dbReader["email"].ToString();
                


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
            try
            {
                dbConn = new OleDbConnection(connectionString);

                cmd = dbConn.CreateCommand();
                dbConn.Open();

                cmd.CommandText = "INSERT INTO [order] ([id_supplyer], [state], [total_price]) "+
                                  "VALUES(@id_supplyer, @state, @total_price)";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@id_supplyer", int.Parse(datagridRow.Cells[1].Value.ToString()));
                cmd.Parameters.AddWithValue("@state", "opened");
                cmd.Parameters.AddWithValue("@total_price", total);
                cmd.ExecuteNonQuery();
                {
                    MessageBox.Show("Encoding order successfull !");
                }

                cmd = null;
                cmd = dbConn.CreateCommand();

                cmd.CommandText = "INSERT INTO detail ( id_order, id_stock, quantity_out, comment)" +
                                  "SELECT MAX(id) AS Expr1, @id_stock AS Expr2, @quantity_out AS Expr3, @comment AS Expr4 FROM[order]";

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@id_stock", int.Parse(datagridRow.Cells[6].Value.ToString()));
                cmd.Parameters.AddWithValue("@quantity_out", int.Parse(label23.Text));
                cmd.Parameters.AddWithValue("@comment", textBox1.Text);

                cmd.ExecuteNonQuery();
                {
                    MessageBox.Show("Encoding detail successfull !");
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


        private void UserControl5_Load(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            
            if(Regex.IsMatch(textBox2.Text, @"^[0-9-]+$"))
            {
                double quantity = int.Parse(textBox2.Text);
                double total_quantity;

                if (datagridRow.Cells[3].Value.ToString() == "ink") total_quantity = quantity * 10.00; else total_quantity = quantity * 1000.00;

                label23.Text = total_quantity.ToString();

                double price = double.Parse(datagridRow.Cells[4].Value.ToString());

                double excl_taxes = quantity * price;
                label24.Text = excl_taxes.ToString() + " €";
                double val_added = excl_taxes * 0.21;
                label25.Text = val_added.ToString() + " €";
                total = excl_taxes * 1.21;
                label26.Text = total.ToString() + " €";
            }
        }

        private void label20_Click(object sender, EventArgs e)
        {

        }
    }
}
