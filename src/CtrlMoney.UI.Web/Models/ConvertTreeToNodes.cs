using CtrlMoney.Domain.Entities.FinancialClassification;
using CtrlMoney.UI.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CtrlMoney.UI.Web.Models
{
    public static class ConvertTreeToNodes
    {
        public static string ConvertToJson(ICollection<ParentTree> parentTrees)
        {
            JsonSerialize jsonS = new JsonSerialize();
            var defaultData = InsertNodes(parentTrees);

            var jsonNodeTree = jsonS.JsonSerializer<DefaultData>(defaultData);
            return jsonNodeTree;
        }

        private static DefaultData InsertNodes(ICollection<ParentTree> parentTrees)
        {
            DefaultData defaultData = new DefaultData();
            defaultData.node_Trees = new Collection<Node_Tree>();

            foreach (var item in parentTrees)
            {
                defaultData.node_Trees.Add(CreateParentTree(item));
            }

            return defaultData;
        }

        private static Node_Tree CreateParentTree(ParentTree parent)
        {
            Node_Tree nodeParent = CrateNodeTree(parent.Title, parent.Id.ToString(), parent.Tag);

            foreach (var item in parent.Children)
            {
                nodeParent.nodes.Add(CreateChildTree(item));
            }

            return nodeParent;
        }

        private static Node_Tree CreateChildTree(ChildTree child)
        {
            Node_Tree nodeChild = CrateNodeTree(child.Title, child.Id.ToString(), child.Tag);

            foreach (var item in child.Children)
            {
                nodeChild.nodes.Add(CreateGrandChildTree(item));
            }

            return nodeChild;
        }
        private static Node_Tree CreateGrandChildTree(GrandChildTree grandChild)
        {
            Node_Tree nodeGrandChild = CrateNodeTree(grandChild.Title, grandChild.Id.ToString(), grandChild.Tag);
            return nodeGrandChild;
        }

        private static Node_Tree CrateNodeTree(string title, string id, string tag)
        {
            return new Node_Tree()
            {
                text = title,
                href = id,
                tag = tag
                //icon = "fas fa-scroll"
            };
        }
    }
}
