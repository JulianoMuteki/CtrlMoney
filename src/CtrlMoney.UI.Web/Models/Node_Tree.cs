using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CtrlMoney.UI.Web.Models
{
    public class Node_Tree
    {
        public string text { get; set; }
        public string href { get; set; }
        public string tag { get; set; }
        public string icon { get; set; }

        public ICollection<Node_Tree> nodes { get; set; }
    }

    public class DefaultData
    {
        public ICollection<Node_Tree> node_Trees { get; set; }

    }
}
