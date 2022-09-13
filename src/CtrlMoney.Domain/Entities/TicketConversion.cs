using CtrlMoney.Domain.Common;
using System;

namespace CtrlMoney.Domain.Entities
{
    public class TicketConversion : EntityBase
    {
        public string TicketInput { get; set; }
        public string TicketOutput { get; set; }
        public string StockBroker { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public DateTime Date { get; set; }

        public TicketConversion()
        {

        }

        public TicketConversion(string ticketInput, string ticketOutput, string stockBroker, int quantity, decimal unitPrice)
        {
            TicketInput = ticketInput;            
            TicketOutput = ticketOutput;            
            StockBroker = stockBroker;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
    }
}
