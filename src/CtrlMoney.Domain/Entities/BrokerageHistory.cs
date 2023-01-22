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
        public DateTime TransactionDate { get; set; } //Data operação	
        public string TransactionType { get; set; }//Operação C/V	
        public string Market { get; set; }
        public DateTime ExpireDate { get; set; }
        public string StockBroker { get; set; }//Corretora	
        public string TicketCode { get; set; } //Código Ativo
        public int Quantity { get; set; } //TODO: Change to decimal
        public decimal Price { get; set; }//Preço unitário	
        public decimal TotalPrice { get; set; }
        public string Category { get; set; } //Categoria	
        public decimal Brokerage { get; set; } //Corretagem	
        public decimal Fees { get; set; } // Taxas
        public decimal Taxes { get; set; } // Impostos
        public decimal IRRF { get; set; }

        public BrokerageHistory()
        : base()
        {
        }

        /// <summary>
        /// B3 Import
        /// </summary>
        /// <param name="totalPrice"></param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <param name="ticketCode"></param>
        /// <param name="stockBroker"></param>
        /// <param name="expireDate"></param>
        /// <param name="transactionDate"></param>
        /// <param name="transactionType"></param>
        /// <param name="market"></param>
        public BrokerageHistory(decimal totalPrice, decimal price, int quantity, string ticketCode, string stockBroker, DateTime expireDate, DateTime transactionDate, string transactionType, string market)
            : base()
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


        /// <summary>
        /// Status Invest Import
        /// </summary>
        /// <param name="transactionDate"></param>
        /// <param name="transactionType"></param>
        /// <param name="market"></param>
        /// <param name="expireDate"></param>
        /// <param name="stockBroker"></param>
        /// <param name="ticketCode"></param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        /// <param name="totalPrice"></param>
        /// <param name="category"></param>
        /// <param name="brokerage"></param>
        /// <param name="fees"></param>
        /// <param name="taxes"></param>
        /// <param name="iRRF"></param>
        public BrokerageHistory(DateTime transactionDate, string transactionType, string stockBroker, string ticketCode, int quantity, decimal price, string category, decimal brokerage, decimal fees, decimal taxes, decimal iRRF)
        {
            TransactionDate = transactionDate;
            TransactionType = ConvertAcronym(transactionType);
            Market = "--";
            ExpireDate = DateTime.MinValue;
            StockBroker = stockBroker;
            TicketCode = ticketCode;
            Quantity = quantity;
            Price = price;
            TotalPrice = quantity * price;
            Category = category;
            Brokerage = brokerage;
            Fees = fees;
            Taxes = taxes;
            IRRF = iRRF;
        }

        private string ConvertAcronym(string transactionType)
        {
            switch (transactionType)
            {
                case "C":
                    transactionType = "Compra";
                    break;
                case "V":
                    transactionType = "Venda";
                    break;
                default:
                    break;                    
            }
            return transactionType;
        }
    }
}
