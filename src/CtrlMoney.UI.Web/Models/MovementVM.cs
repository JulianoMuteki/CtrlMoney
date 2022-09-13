using System;
using System.ComponentModel.DataAnnotations;

namespace CtrlMoney.UI.Web.Models
{
    public class MovementVM
    {
        public Guid TicketId { get; set; }
        public string TicketCode { get; set; }
        public DateTime Date { get; set; }
        public string InputOutput { get; set; }
        public string MovimentType { get; set; }
        public string StockBroker { get; set; }

        [Required(ErrorMessage = "Please select Quantity")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Please select UnitPrice")]
        public decimal UnitPrice { get; set; }
        public string TransactionValue { get; set; }
    }
}
