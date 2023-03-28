using CtrlMoney.Domain.Common;
using System;

namespace CtrlMoney.Domain.Entities
{
    public class Moviment : EntityBase
    {
        public string InputOutput { get; set; }
        public DateTime Date { get; set; }
        public string MovimentType { get; set; }
        public string TicketCode { get; set; }
        public string StockBroker { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TransactionValue { get; set; }

        public Moviment()
            :base()
        {

        }

        public Moviment(string inputOutput, DateTime date, string movimentType, string ticketCode, string stockBroker, decimal quantity, decimal unitPrice, decimal transactionValue)
        {
            InputOutput = inputOutput;
            Date = date;
            MovimentType = movimentType;
            TicketCode = ticketCode;
            StockBroker = stockBroker;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TransactionValue = transactionValue;
        }
    }
}
