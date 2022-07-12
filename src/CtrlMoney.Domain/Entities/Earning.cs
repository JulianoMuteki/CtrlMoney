using CtrlMoney.Domain.Common;
using System;

namespace CtrlMoney.Domain.Entities
{
    public class Earning : EntityBase
    {
        public string TicketCode { get; set; }
        public DateTime PaymentDate { get; set; }
        public string EventType { get; set; }
        public string StockBroker { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal NetValue { get; set; }

        public Earning()
            : base()
        {

        }

        public Earning(string ticketCode, DateTime paymentDate, string eventType, string stockBroker, int quantity, decimal unitPrice, decimal netValue)
            : base()
        {
            TicketCode = ticketCode;
            PaymentDate = paymentDate;
            EventType = eventType;
            StockBroker = stockBroker;
            Quantity = quantity;
            UnitPrice = unitPrice;
            NetValue = netValue;
        }
    }
}
