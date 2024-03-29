﻿using System.Collections.Generic;

namespace CtrlMoney.UI.Web.Models
{
    public class IncomeTaxTicket
    {
        public string TicketCode { get; set; }
        //public int Quantity { get; set; }
        //public string TotalValue { get; set; }
        public string Bookkeeping { get; set; }
        public string Conversion { get; set; }
        public ICollection<ResumeBrokerageHistories> ResumeBrokerageHistories { get; set; }
        //public ICollection<BrokerageHistoryVM> BrokerageHistoryVMs { get; set; }

        //public string Operation { get; set; }
        //public string TotalYearExercise { get; set; }
        //public string TotalCalendarYear { get; set; }


        public DataOperation DataOperationInput { get; set; }
        public DataOperation DataOperationOutput { get; set; }
        public int ExerciseYear { get; set; }
        public int CalendarYear { get; set; }
        public int LastYear { get; set; }
    }
}
