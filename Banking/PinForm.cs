﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Banking.Banking;
using Banking.Properties;

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
                MessageBox.Show(Resources.MsgEnterPIN);
                return;
            }

            if (Session.CurrentCard == null)
            {
                MessageBox.Show(Resources.MsgCardNull);
                return;
            }

            if (enteredPin != Session.CurrentCard.PIN)
            {
                string cardEnd = Session.CurrentCard.CardNumber.Substring(Session.CurrentCard.CardNumber.Length - 4);
                Logger.Log($"User entered wrong PIN for card ending with {cardEnd};");
                MessageBox.Show(Resources.MsgWrongPIN);
                return;
            }

            DashboardForm dashboardForm = new DashboardForm
            {
                StartPosition = FormStartPosition.Manual,
                Location = this.Location,
                Size = this.Size
            };

            string cardEnding = Session.CurrentCard.CardNumber.Substring(Session.CurrentCard.CardNumber.Length - 4);
            Logger.Log($"User successfully entered pin for card ending with {cardEnding};.");
            dashboardForm.Show();
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
