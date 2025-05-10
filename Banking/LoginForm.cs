using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Banking.Banking;
using static System.Collections.Specialized.BitVector32;

namespace Banking
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string cardNumber = textBox1.Text.Trim();
            string expiry = textBox2.Text.Trim() + "/" + textBox4.Text.Trim(); // формат MM/YY
            string cvv = textBox3.Text.Trim();

            if (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(expiry) || string.IsNullOrEmpty(cvv))
            {
                MessageBox.Show("Будь ласка, заповніть усі поля.");
                return;
            }

            if (cardNumber.Length != 16 )
            {
                MessageBox.Show("Номер карти повинен містити 16 цифр.");
                return;
            }

            // Сканування всіх XML
            string banksFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "banks");
            var xmlFiles = Directory.GetFiles(banksFolder, "*.xml");

            foreach (var file in xmlFiles)
            {
                Bank bank = BankDataManager.LoadBankFromXml(file);

                foreach (Card card in bank.Cards)
                {
                    if (card.CardNumber == cardNumber && card.Expiry == expiry)
                    {
                        if (card.CVV != cvv)
                        {
                            string cardEnding = Session.CurrentCard.CardNumber.Substring(Session.CurrentCard.CardNumber.Length - 4);
                            Logger.Log($"User entered wrong CVV code for {cardEnding};.");
                            MessageBox.Show("Неправильний CVV код.");
                        }

                        // Зберігаємо в глобальну змінну
                        Session.CurrentCard = card;
                        Session.CurrentBank = bank;

                        // Переходимо на PIN форму
                        PinForm pinForm = new PinForm();

                        pinForm.StartPosition = FormStartPosition.Manual;
                        pinForm.Location = this.Location;
                        pinForm.Size = this.Size;

                        pinForm.Show();
                        this.Hide();
                        return;
                    }
                }
            }

            MessageBox.Show("Картку не знайдено або дані некоректні.");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
