using CtrlMoney.CrossCutting.Enums;
using CtrlMoney.Domain.Common;
using System;

namespace CtrlMoney.Domain.Entities
{
    public class Position : EntityBase
    {
        public DateTime PositionDate { get; set; }
        public EInvestmentType EInvestmentType { get; set; }
        public string StockBroker { get; set; }
        public string TicketCode { get; set; }
        public string ISINCode { get; set; }
        public string Type { get; set; }
        public string Bookkeeping { get; set; }
        public int Quantity { get; set; }
        public int QuantityAvailable { get; set; }
        public int QuantityUnavailable { get; set; }
        public string Reason { get; set; }
        public decimal ClosingPrice { get; set; }
        public decimal ValueUpdated { get; set; }

        public Position()
        : base()
        {

        }

        public Position(DateTime positionDate, EInvestmentType eInvestmentType, string stockBroker, string ticketCode, string iSINCode, string type, string bookkeeping, int quantity, int quantityAvailable, int quantityUnavailable, string reason, decimal closingPrice, decimal valueUpdated)
        : base()
        {
            PositionDate = positionDate;
            EInvestmentType = eInvestmentType;
            StockBroker = stockBroker;
            TicketCode = ticketCode;
            ISINCode = iSINCode;
            Type = type;
            Bookkeeping = bookkeeping;
            Quantity = quantity;
            QuantityAvailable = quantityAvailable;
            QuantityUnavailable = quantityUnavailable;
            Reason = reason;
            ClosingPrice = closingPrice;
            ValueUpdated = valueUpdated;
        }
    }
}
