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

    public enum EPaymentMethod
    {
        [Description("Cash")]
        Cash = 0,
        [Description("Credit card")]
        CREDIT_CARD = 1,
        [Description("Deferred payment")]
        DEFERRED_PAYMENT = 2
    }
}
