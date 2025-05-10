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
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Banking.Banking;
using Banking.Properties;

namespace Banking
{
    public partial class DepositForm : Form
    {
        public DepositForm()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show(Resources.MsgDepositEmpty);
                return;
            }

            string input = textBox1.Text.Replace('.', ',');

            if (!decimal.TryParse(input, out decimal amount))
            {
                MessageBox.Show(Resources.MsgIncorrectNumber);
                return;
            }

            if (amount <= 0)
            {
                MessageBox.Show(Resources.MsgSum);
                return;
            }

            var previousBalance = Session.CurrentCard.Balance;
            Session.CurrentCard.Balance += amount;
            UpdateXmlBalance(Session.CurrentBank.Name, Session.CurrentCard.CardNumber, Session.CurrentCard.Balance);

            string cardEnding = Session.CurrentCard.CardNumber.Substring(Session.CurrentCard.CardNumber.Length - 4);
            Logger.Log($"User successfully deposited {amount:F2} UAH to card ending with {cardEnding};.");
            MessageBox.Show(Resources.MsgSuccesfullDeposit + $"{amount:F2}" + Resources.MoneySymbol);
            PrintReceipt(previousBalance, Session.CurrentCard.Balance - previousBalance, "Deposit");
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

        private void button1_Click(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            this.Controls.Clear();
            InitializeComponent();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("uk");

            this.Controls.Clear();
            InitializeComponent();
        }
    }
}
