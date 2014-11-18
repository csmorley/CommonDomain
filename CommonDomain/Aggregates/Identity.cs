using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Aggregates
{
    public abstract class Identity : IEquatable<Identity>, IIdentity
    {
        public Identity()
        {
            this.id = Guid.NewGuid().ToString();
        }

        public Identity(string id)
        {
            this.id = id;
        }

        public Identity(Guid id)
        {
            this.id = id.ToString();
        }

        public readonly string id;

        public string Id { get { return this.id; } }

        public bool Equals(Identity id)
        {
            if (object.ReferenceEquals(this, id)) return true;
            if (object.ReferenceEquals(null, id)) return false;
            return this.id.Equals(id.id);
        }

        public override bool Equals(object anotherObject)
        {
            return Equals(anotherObject as Identity);
        }

        public override int GetHashCode()
        {
            return (this.GetType().GetHashCode() * 907) + this.id.GetHashCode();
        }

        public override string ToString()
        {
            return this.GetType().Name + " [Id=" + this.id + "]";
        }
    }
}
