using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Banking.Banking;

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
            string enteredPin = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(enteredPin))
            {
                MessageBox.Show("Введіть PIN код.");
                return;
            }

            if (Session.CurrentCard == null)
            {
                MessageBox.Show("Спочатку потрібно ввести дані картки.");
                return;
            }

            if (enteredPin != Session.CurrentCard.PIN)
            {
                MessageBox.Show("Неправильний PIN код.");
                return;
            }

            DashboardForm dashboardForm = new DashboardForm
            {
                StartPosition = FormStartPosition.Manual,
                Location = this.Location,
                Size = this.Size
            };

            dashboardForm.Show();
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
