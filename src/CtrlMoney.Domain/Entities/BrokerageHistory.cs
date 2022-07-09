using CtrlMoney.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlMoney.Domain.Entities
{
    public class BrokerageHistory : EntityBase
    {
        public decimal TotalPrice { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string TicketCode { get; set; }
        public string StockBroker { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string Market { get; set; }
        public BrokerageHistory()
        : base()
        {
        }

        public BrokerageHistory(decimal totalPrice, decimal price, int quantity, string ticketCode, string stockBroker, DateTime expireDate, DateTime transactionDate, string transactionType, string market)
            :base()
        {
            TotalPrice = totalPrice;
            Price = price;
            Quantity = quantity;
            TicketCode = ticketCode;
            StockBroker = stockBroker;
            ExpireDate = expireDate;
            TransactionDate = transactionDate;
            TransactionType = transactionType;
            Market = market;
        }
    }
}
