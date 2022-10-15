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
        public decimal Price { get; set; }//Preço unitário	
        public decimal TotalPrice { get; set; }
        public decimal TotalNetAmount { get; set; }  // Total líq.
        public string Category { get; set; } //Categoria	
        public DateTime WithDate { get; set; }

        public Earning()
            : base()
        {

        }

        public Earning(string ticketCode, DateTime paymentDate, string eventType, string stockBroker, int quantity, decimal price, decimal totalPrice)
            : base()
        {
            TicketCode = ticketCode;
            PaymentDate = paymentDate;
            EventType = eventType;
            StockBroker = stockBroker;
            Quantity = quantity;
            Price = price;
            TotalPrice = totalPrice;
        }

        /// <summary>
        /// Status Invest Earnings
        /// </summary>
        /// <param name="ticketCode"></param>
        /// <param name="paymentDate"></param>
        /// <param name="eventType"></param>
        /// <param name="stockBroker"></param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        /// <param name="totalPrice"></param>
        /// <param name="totalNetAmount"></param>
        /// <param name="category"></param>
        /// <param name="withDate"></param>
        public Earning(string ticketCode, DateTime paymentDate, string eventType, string stockBroker, int quantity, decimal price, decimal totalPrice, decimal totalNetAmount, string category, DateTime withDate)
            : base()
        {
            TotalNetAmount = totalNetAmount;
            Category = category;
            WithDate = withDate;
            TicketCode = ticketCode;
            PaymentDate = paymentDate;
            EventType = eventType;
            StockBroker = stockBroker;
            Quantity = quantity;
            Price = price;
            TotalPrice = totalPrice;
        }
    }
}
