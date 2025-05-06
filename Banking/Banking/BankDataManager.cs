using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Banking.Banking
{
    public static class BankDataManager
    {
        public static Bank LoadBankFromXml(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            var bankElement = doc.Element("bank");
            Bank bank = new Bank
            {
                Name = bankElement.Element("name").Value,
                CommissionRate = decimal.Parse(bankElement.Element("commission").Value, System.Globalization.CultureInfo.InvariantCulture)
            };

            foreach (var cardElement in bankElement.Elements("card"))
            {
                bank.Cards.Add(new Card
                {
                    CardNumber = cardElement.Element("number").Value,
                    Expiry = cardElement.Element("expiry").Value,
                    CVV = cardElement.Element("cvv").Value,
                    PIN = cardElement.Element("pin").Value,
                    Balance = decimal.Parse(cardElement.Element("balance").Value, System.Globalization.CultureInfo.InvariantCulture),
                    Bank = bank.Name
                });
            }

            return bank;
        }

        public static void SaveBankToXml(Bank bank, string filePath)
        {
            var doc = new XDocument(
                new XElement("bank",
                    new XElement("name", bank.Name),
                    new XElement("commission", bank.CommissionRate),
                    bank.Cards.Select(c =>
                        new XElement("card",
                            new XElement("number", c.CardNumber),
                            new XElement("expiry", c.Expiry),
                            new XElement("cvv", c.CVV),
                            new XElement("pin", c.PIN),
                            new XElement("balance", c.Balance)
                        )
                    )
                )
            );

            doc.Save(filePath);
        }
    }
}