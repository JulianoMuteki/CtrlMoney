using CtrlMoney.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CtrlMoney.UI.Web.Models
{
    public class BankVM
    {
        public string DT_RowId { get; set; }
        public string Name { get; set; }
        public string BankBalance { get; set; }
        public string BankCode { get; set; }
        public string InitialBalance { get; set; }
        public BankVM(Bank bank)
        {
            this.DT_RowId = bank.Id.ToString();
            this.BankCode = bank.BankCode.ToString();
            this.BankBalance = bank.BankBalance.ToString();
            this.InitialBalance = bank.InitialBalance.ToString();
            this.Name = bank.Name;
        }
    }
}
