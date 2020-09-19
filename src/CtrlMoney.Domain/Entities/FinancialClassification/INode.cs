using CtrlMoney.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlMoney.Domain.Entities.FinancialClassification
{
   public interface INode<T,U>
                              where T : class
                              where U : class
    {
        public string Title { get; set; }
        public int LevelTree { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Category or Class
        /// </summary>
        public string Tag { get; set; }
        public T ParentNode { get; set; }
        public Guid? ParentNodeID { get; set; }

        public ICollection<U> Children { get; set; }

        public void Add(U node);

        public void Remove(U node);
    }
}
