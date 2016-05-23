using AddressFinder.BA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddressFinder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Find_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AddressBox.Text))
            {
                Message.Text = "Please Enter address to find.";
                Message.ForeColor = Color.Red;
                Message.BackColor = Color.Yellow;
            }
            else
            {
                string address = GoogleGeoCodeFinder.FindAddressDetails(AddressBox.Text);

                Message.Text = address;
                Message.ForeColor = Color.Blue;
                Message.BackColor = Color.Yellow;
            }
        }

    }
}
