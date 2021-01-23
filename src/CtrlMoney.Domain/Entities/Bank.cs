using CtrlMoney.Domain.Common;
using System.Collections.Generic;

namespace CtrlMoney.Domain.Entities
{
    public class Bank : EntityBase
    {
        public string Name { get; set; }
        public double BankBalance { get; set; }
        public int BankCode { get; set; }
        public double InitialBalance { get; set; }
        public ICollection<FinancialTransaction> FinancialTransactions { get; set; }
        public Bank()
        {
            this.FinancialTransactions = new HashSet<FinancialTransaction>();

        }

    }
}
