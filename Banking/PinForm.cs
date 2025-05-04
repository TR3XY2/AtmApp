using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Banking
{
    public partial class PinForm : Form
    {
        public PinForm()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            DashboardForm dashboardForm = new DashboardForm();

            dashboardForm.StartPosition = FormStartPosition.Manual;
            dashboardForm.Location = this.Location;
            dashboardForm.Size = this.Size;

            dashboardForm.Show();

            this.Hide();
        }
    }
}
