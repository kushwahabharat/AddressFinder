using AddressFinder.BA;
using Newtonsoft.Json.Linq;
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
    public partial class NearbyLocations : Form
    {
        public NearbyLocations()
        {
            InitializeComponent();
        }

        private void Findbtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LocationBox.Text))
            {
                Message.Text = "Please Enter address to find.";
                Message.ForeColor = Color.Red;
                Message.BackColor = Color.Yellow;
            }
            else
            {
                string locationType = null;
                if (locationTypeddl.SelectedIndex > -1)
                    locationType = locationTypeddl.SelectedItem.ToString();
                
                var results = GooglePlacesApiBroker.FindNearby(latLong: LocationBox.Text, locationType: locationType);

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

        private void addressDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddressInfo address = new AddressInfo();
            address.Show();
            this.Hide();
        }

    }
}
