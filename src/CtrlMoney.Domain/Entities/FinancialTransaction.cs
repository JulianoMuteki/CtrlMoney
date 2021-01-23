using CtrlMoney.CrossCutting.Enums;
using CtrlMoney.Domain.Common;
using CtrlMoney.Domain.Entities.FinancialClassification;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlMoney.Domain.Entities
{
    public class FinancialTransaction : EntityBase
    {
        public FinancialTransaction()
        : base()
        {
        }

        public string Description { get; private set; }
        public decimal Value { get; private set; }
        public DateTime? PaymentDate { get; private set; }
        public bool IsOperationDone { get; private set; }

        public ETransactionType ETransactionType { get; private set; }
        public EPaymentMethod EPaymentMethod { get; private set; }

        public Bank Bank { get; private set; }
        public Guid BankID { get; private set; }

        public Guid ChildTreeID { get; private set; }
        public ChildTree ChildTree { get; private set; }

        public Guid ParentTreeID { get; private set; }
        public ParentTree ParentTree { get; private set; }

        public GrandChildTree GrandChildTree { get; private set; }
        public Guid GrandChildTreeID { get; private set; }
    }
}
