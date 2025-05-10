using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Banking.Banking;

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
                MessageBox.Show("Будь ласка, введіть суму для зняття.");
                return;
            }

            if (!decimal.TryParse(textBox1.Text, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out decimal amount))
            {
                MessageBox.Show("Введено некоректне число. Будь ласка, введіть лише цифри.");
                return;
            }

            if (amount <= 0)
            {
                MessageBox.Show("Сума має бути більшою за нуль.");
                return;
            }

            // Обрахунок суми з комісією
            decimal commission = Session.CurrentBank.Name != Session.ATMBank ? amount * Session.CurrentBank.CommissionRate : 0;
            decimal totalAmount = amount + commission;

            // Перевірка балансу з урахуванням комісії
            if (totalAmount > Session.CurrentCard.Balance)
            {
                MessageBox.Show($"Недостатньо коштів на рахунку. З комісією потрібно {totalAmount:F2} UAH.");
                return;
            }

            var previousBalance = Session.CurrentCard.Balance;
            // Зняття коштів
            Session.CurrentCard.Balance -= totalAmount;
            UpdateXmlBalance(Session.CurrentBank.Name, Session.CurrentCard.CardNumber, Session.CurrentCard.Balance);

            string cardEnding = Session.CurrentCard.CardNumber.Substring(Session.CurrentCard.CardNumber.Length - 4);
            Logger.Log($"User successfully withdrew {amount:F2} UAH from card ending with {cardEnding};.");
            MessageBox.Show($"Успішно знято {amount:F2} UAH.\nКомісія: {commission:F2} UAH.\nРазом списано: {totalAmount:F2} UAH.");
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
                MessageBox.Show("Файл банку не знайдено.");
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
                MessageBox.Show("Картку не знайдено в банку.");
            }
        }

        private void PrintReceipt(decimal previousBalance, decimal transactionAmount, string transactionType)
        {
            DialogResult result = MessageBox.Show("Бажаєте зберегти чек у файл?", "Збереження чеку", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
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
                MessageBox.Show($"Чек збережено у: {filePath}");
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
