using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Banking
{
    public static class Session
    {
        public static Card CurrentCard { get; set; }
        public static Bank CurrentBank { get; set; }
    }
}
