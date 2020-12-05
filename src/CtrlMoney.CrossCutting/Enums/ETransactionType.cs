using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CtrlMoney.CrossCutting.Enums
{
    public enum ETransactionType
    {
        [Description("Income")]
        INCOME = 0,
        [Description("Expense")]
        EXPENSE = 1,
        [Description("Transfer")]
        TRANSFER = 2,
        [Description("Investiment")]
        INVESTIMENT = 3
    }
}
