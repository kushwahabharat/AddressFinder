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
    public partial class AddressInfo : Form
    {
        public AddressInfo()
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
                var results = GoogleGeoCodeApiBroker.FindAddressDetails(AddressBox.Text);

                if (results != null)
                {
                    dataGridView1.DataSource = results;

                    Message.Text = results.Count + " results found";
                    Message.ForeColor = Color.Blue;
                    Message.BackColor = Color.Yellow;
                }
                else
                {
                    Message.Text = "0 results found.";
                    Message.ForeColor = Color.Red;
                    Message.BackColor = Color.Yellow;
                }
            }
        }

        private void findNearbyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NearbyLocations location = new NearbyLocations();
            location.Show();
            this.Hide();
        }

    }
}
