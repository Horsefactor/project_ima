using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project_ima
{
    public partial class Form1 : Form
    {
        static Form1 _obj;

        public static Form1 Instance
        {
            get
            {
                if (_obj == null)
                {
                    _obj = new Form1();

                }
                return _obj;
            }
        }

        public Panel PnlContainer
        {
            get { return panelContainer; }
            set { panelContainer = value; }
        }

        public Button BackButton
        {
            get { return btnBack;  }
            set { btnBack = value;  }
        }

        public Form1()
        {
            InitializeComponent();
            sidePanel.Height = button1.Height;
            sidePanel.Top = button1.Top;
            userControl1.BringToFront();
            panelLogo.BackgroundImageLayout = ImageLayout.Stretch;
            btnBack.Visible = false;
            _obj = this;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sidePanel.Height = button2.Height;
            sidePanel.Top = button2.Top;
            userControl2.BringToFront();

        }

        private void panelSideMenu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            sidePanel.Height = button1.Height;
            sidePanel.Top = button1.Top;
            userControl1.BringToFront();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sidePanel.Height = button3.Height;
            sidePanel.Top = button3.Top;
            userControl3.BringToFront();
        }

        private void sidePanel_Paint(object sender, PaintEventArgs e)
        {
        }

        private void userControl2_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            sidePanel.Height = button4.Height;
            sidePanel.Top = button4.Top;
            userControl4.BringToFront();
        }

        private void panelLogo_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            sidePanel.Height = button2.Height;
            sidePanel.Top = button2.Top;
            userControl2.BringToFront();
            btnBack.Visible = false;
        }
    }
}
