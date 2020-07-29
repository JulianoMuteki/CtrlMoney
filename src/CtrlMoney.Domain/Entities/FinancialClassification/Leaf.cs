using CtrlMoney.Domain.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CtrlMoney.Domain.Entities.FinancialClassification
{
   public class Leaf : EntityBase, IComponent<Composite>
    {
        public Composite Composite { get; private set; }
        public Guid CompositeID { get; private set; }
        public string Title { get; set; }

        public Leaf()
            :base()
        {

        }
        public string Operation()
        {
            return "Leaf";
        }

        public bool IsComposite()
        {
            return false;
        }

    }
}
