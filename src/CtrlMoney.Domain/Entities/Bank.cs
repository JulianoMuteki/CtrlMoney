using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlMoney.Domain.Entities
{
    class Bank
    {
        public string Name { get; private set; }
        public double BankBalance { get; private set; }

        public double InitialBalance { get; set; }
    }
}
