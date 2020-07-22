using CtrlMoney.Domain.Interfaces.Base;
using System;

namespace CtrlMoney.Domain.Common
{
    public abstract class EntityBase
    {
        public Guid Id { get; protected set; }
        public DateTime CreationDate { get; protected set; }
        public DateTime DateModified { get; protected set; }
        public bool IsDelete { get; protected set; }
        public bool IsDisable { get; protected set; }

        private IComponentValidate _component;
        public IComponentValidate ComponentValidator
        {
            get
            {
                if (_component == null)
                    _component = new BaseValidate();

                return _component;
            }
        }

        public EntityBase()
        {
            InitBase();
        }

        public void InitBase()
        {
            this.DateModified = DateTime.Now;
            this.CreationDate = DateTime.Now;
            this.Id = Guid.NewGuid();
            this.IsDelete = false;
            this.IsDisable = false;
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as EntityBase;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(EntityBase a, EntityBase b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EntityBase a, EntityBase b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }
    }
}
