using CtrlMoney.Domain.Common;
using System;
using System.Collections.Generic;

namespace CtrlMoney.Domain.Entities
{
    public class Address : EntityBase
    {
        public string CEP { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Estate { get; set; }
        public string Reference { get; set; }

        //public virtual ICollection<Client> Clients { get; set; }

        private Address()
            : base()
        {
            //this.Clients = new HashSet<Client>();
        }

        public void Init()
        {
            if (this.Id == null || this.Id == Guid.Empty)
            {
                base.InitBase();
            }
        }
    }
}
