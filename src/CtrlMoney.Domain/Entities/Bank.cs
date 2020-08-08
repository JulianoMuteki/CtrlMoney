using CtrlMoney.Domain.Common;

namespace CtrlMoney.Domain.Entities
{
    public class Bank : EntityBase
    {
        public string Name { get; set; }
        public double BankBalance { get; set; }
        public int BankCode { get; set; }
        public double InitialBalance { get; set; }
    }
}
