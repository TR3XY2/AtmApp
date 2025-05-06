using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Banking
{
    public class Card
    {
        public string CardNumber { get; set; }
        public string Expiry { get; set; }
        public string CVV { get; set; }
        public string PIN { get; set; }
        public decimal Balance { get; set; }
        public string Bank { get; set; }
    }
}
