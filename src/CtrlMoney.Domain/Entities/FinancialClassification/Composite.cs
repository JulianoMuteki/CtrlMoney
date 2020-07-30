using CtrlMoney.Domain.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CtrlMoney.Domain.Entities.FinancialClassification
{
    public class Composite : EntityBase, IComponent<Leaf>
    {
        public ICollection<Leaf> Leaves { get; set; }
        public string Title { get; set; }

        public Leaf LeafParent { get; private set; }
        public Guid? LeafParentID { get; private set; }
        public Composite()
            :base()
        {
            this.Leaves = new HashSet<Leaf>();
        }
        public void Add(Leaf component)
        {
            this.Leaves.Add(component);
        }

        public void Remove(Leaf component)
        {
            this.Leaves.Remove(component);
        }

        // The Composite executes its primary logic in a particular way. It
        // traverses recursively through all its children, collecting and
        // summing their results. Since the composite's children pass these
        // calls to their children and so forth, the whole object tree is
        // traversed as a result.
        public string Operation()
        {
            int i = 0;
            string result = "Branch(";

            foreach (Leaf component in this.Leaves)
            {
                result += component.Operation();
                if (i != this.Leaves.Count - 1)
                {
                    result += "+";
                }
                i++;
            }

            return result + ")";
        }
    }
}
