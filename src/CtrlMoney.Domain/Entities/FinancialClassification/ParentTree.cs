using CtrlMoney.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlMoney.Domain.Entities.FinancialClassification
{
    public class ParentTree : EntityBase, INode<GrandChildTree, ChildTree>
    {
        public ParentTree()
            : base()
        {
            this.Children = new HashSet<ChildTree>();
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }

        public int LevelTree { get; set; }
        public GrandChildTree ParentNode { get; set; }
        public Guid? ParentNodeID { get; set; }
        public ICollection<ChildTree> Children { get; set; }

        public void Add(ChildTree node)
        {
            throw new NotImplementedException();
        }

        public void Remove(ChildTree node)
        {
            throw new NotImplementedException();
        }
    }
}
