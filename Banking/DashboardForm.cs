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
    public partial class DashboardForm : Form
    {
        public DashboardForm()
        {
            InitializeComponent();
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            BalanceForm balanceForm = new BalanceForm();

            balanceForm.StartPosition = FormStartPosition.Manual;
            balanceForm.Location = this.Location;
            balanceForm.Size = this.Size;

            balanceForm.Show();

            balanceForm.FormClosed += (s, args) => this.Show();

            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DepositForm depositForm = new DepositForm();

            depositForm.StartPosition = FormStartPosition.Manual;
            depositForm.Location = this.Location;
            depositForm.Size = this.Size;

            depositForm.Show();

            depositForm.FormClosed += (s, args) => this.Show();

            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            WithdrawForm withdrawForm = new WithdrawForm();

            withdrawForm.StartPosition = FormStartPosition.Manual;
            withdrawForm.Location = this.Location;
            withdrawForm.Size = this.Size;

            withdrawForm.Show();

            withdrawForm.FormClosed += (s, args) => this.Show();

            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
