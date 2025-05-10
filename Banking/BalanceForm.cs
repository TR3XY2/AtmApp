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
    public partial class BalanceForm : Form
    {
        public BalanceForm()
        {
            InitializeComponent();
            label3.Text = Math.Round(Session.CurrentCard.Balance, 2).ToString() + " UAH";
            string cardEnding = Session.CurrentCard.CardNumber.Substring(Session.CurrentCard.CardNumber.Length - 4);
            Logger.Log($"User viewed balance on card ending with {cardEnding};.");
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
