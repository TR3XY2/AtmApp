using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Banking.Banking;
using Banking.Properties;

namespace Banking
{
    public partial class WithdrawForm : Form
    {
        public WithdrawForm()
        {
            InitializeComponent();
            
            if (Session.CurrentBank.Name != Session.ATMBank)
            {
                this.label6.Text = ((int)(Session.CurrentBank.CommissionRate*100)).ToString() + "%";
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show(Resources.MsgWithdrawSum);
                return;
            }

            if (!decimal.TryParse(textBox1.Text, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out decimal amount))
            {
                MessageBox.Show(Resources.MsgIncorrectNumber);
                return;
            }

            if (amount <= 0)
            {
                MessageBox.Show(Resources.MsgSum);
                return;
            }

            // Обрахунок суми з комісією
            decimal commission = Session.CurrentBank.Name != Session.ATMBank ? amount * Session.CurrentBank.CommissionRate : 0;
            decimal totalAmount = amount + commission;

            // Перевірка балансу з урахуванням комісії
            if (totalAmount > Session.CurrentCard.Balance)
            {
                MessageBox.Show(Resources.MsgNotEnough + $"{totalAmount:F2} " + Resources.MoneySymbol);
                return;
            }

            var previousBalance = Session.CurrentCard.Balance;
            // Зняття коштів
            Session.CurrentCard.Balance -= totalAmount;
            UpdateXmlBalance(Session.CurrentBank.Name, Session.CurrentCard.CardNumber, Session.CurrentCard.Balance);

            string cardEnding = Session.CurrentCard.CardNumber.Substring(Session.CurrentCard.CardNumber.Length - 4);
            Logger.Log($"User successfully withdrew {amount:F2} UAH from card ending with {cardEnding};.");
            MessageBox.Show(Resources.MsgSuccesfullWithdrawal + $"{amount:F2}" + Resources.MoneySymbol + "." + Environment.NewLine + 
                Resources.MsgCommission + $"{commission:F2}" + Resources.MoneySymbol + ".\n" +
                 Resources.MsgTotal + $"{totalAmount:F2}" + Resources.MoneySymbol + ".");
            PrintReceipt(previousBalance, totalAmount, "Withdraw");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void UpdateXmlBalance(string bankName, string cardNumber, decimal newBalance)
        {
            string filePath = $"banks/{bankName.ToLower()}.xml";

            if (!File.Exists(filePath))
            {
                MessageBox.Show(Resources.MsgBankNotFound);
                return;
            }

            XDocument doc = XDocument.Load(filePath);

            var cardElement = doc.Descendants("card")
                                 .FirstOrDefault(x => (string)x.Element("number") == cardNumber);

            if (cardElement != null)
            {
                cardElement.Element("balance").Value = newBalance.ToString(System.Globalization.CultureInfo.InvariantCulture);
                doc.Save(filePath);
            }
            else
            {
                MessageBox.Show(Resources.MsgCardNotFound);
            }
        }

        private void PrintReceipt(decimal previousBalance, decimal transactionAmount, string transactionType)
        {
            DialogResult result = MessageBox.Show(Resources.MsgSaveReceipt, Resources.MsgSaveReceiptCaption, MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                string cardEnding = Session.CurrentCard.CardNumber.Substring(Session.CurrentCard.CardNumber.Length - 4);
                Logger.Log($"User saved receipt for card ending with {cardEnding};");

                var receipt = new
                {
                    TransactionType = transactionType,
                    Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Card = new
                    {
                        Number = "**** **** **** " + Session.CurrentCard.CardNumber.Substring(Session.CurrentCard.CardNumber.Length - 4),
                        CVV = "***",
                        Expiry = Session.CurrentCard.Expiry
                    },
                    PreviousBalance = previousBalance.ToString("F2"),
                    TransactionAmount = transactionAmount.ToString("F2"),
                    CurrentBalance = Session.CurrentCard.Balance.ToString("F2")
                };

                string json = JsonSerializer.Serialize(receipt, new JsonSerializerOptions { WriteIndented = true });
                string fileName = $"receipt_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

                File.WriteAllText(filePath, json);
                MessageBox.Show(Resources.MsgReceiptSaved + filePath);
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            this.Controls.Clear();
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("uk");

            this.Controls.Clear();
            InitializeComponent();
        }
    }
}
