using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Banking.Banking
{
    public static class TransactionManager
    {
        public static void Withdraw(Card card, decimal amount, decimal commissionRate)
        {
            decimal commission = card.Bank != "MonoBank" ? amount * commissionRate : 0;
            card.Balance -= amount + commission;
        }

        public static void Deposit(Card card, decimal amount)
        {
            card.Balance += amount;
        }

        public static void PrintReceipt(Card card, decimal oldBalance, decimal transactionAmount, decimal newBalance)
        {
            var receipt = new
            {
                Date = DateTime.Now,
                CardNumber = $"**** **** **** {card.CardNumber.Substring(card.CardNumber.Length - 4)}",
                CVV = "***",
                OldBalance = oldBalance,
                TransactionAmount = transactionAmount,
                NewBalance = newBalance
            };

            string json = JsonSerializer.Serialize(receipt, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText($"receipt_{DateTime.Now:yyyyMMdd_HHmmss}.json", json);
        }
    }
}
