using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Banking
{
    public static class CardValidator
    {
        public static Card ValidateCard(string number, string expiry, string cvv, string pin, List<Bank> banks)
        {
            foreach (var bank in banks)
            {
                foreach (var card in bank.Cards)
                {
                    if (card.CardNumber == number &&
                        card.Expiry == expiry &&
                        card.CVV == cvv &&
                        card.PIN == pin)
                    {
                        return card;
                    }
                }
            }

            return null;
        }
    }
}
