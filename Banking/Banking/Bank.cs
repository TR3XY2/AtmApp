using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Banking
{
    public class Bank
    {
        public string Name { get; set; }
        public decimal CommissionRate { get; set; }

        public List<Card> Cards { get; set; } = new List<Card>();
    }
}
