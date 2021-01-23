using CtrlMoney.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlMoney.Domain.Entities.FinancialClassification
{
   public class ChildTree : EntityBase, INode<ParentTree, GrandChildTree>
    {
        public ChildTree()
            : base()
        {
            this.Children = new HashSet<GrandChildTree>();
            this.FinancialTransactions = new HashSet<FinancialTransaction>();
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }

        public int LevelTree { get; set; }
        public ParentTree ParentNode { get; set; }
        public Guid? ParentNodeID { get; set; }
        public ICollection<GrandChildTree> Children { get; set; }
        public ICollection<FinancialTransaction> FinancialTransactions { get; set; }

        public void Add(GrandChildTree node)
        {
            throw new NotImplementedException();
        }

        public void Remove(GrandChildTree node)
        {
            throw new NotImplementedException();
        }


    }
}
