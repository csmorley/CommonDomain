using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Aggregates
{
    public abstract class Identity : IEquatable<Identity>, IIdentity
    {
        protected string value;

        // this absolutly must be fixed, do not use [JsonProperty] find alternative way to set value
        // currently this is public read/write, big no, no for identity must be fixed
        public string Value
        {
            get { return this.value; }

            set { this.value = value; }
        }

        public Identity(string value)
        {
            this.Value = value;
        }

        public Identity(Guid value)
        {
            this.Value = value.ToString();
        }

        public bool Equals(Identity identity)
        {
            if (object.ReferenceEquals(this, identity)) return true;
            if (object.ReferenceEquals(null, identity)) return false;
            return this.value.Equals(identity.value);
        }

        public override bool Equals(object anotherObject)
        {
            return Equals(anotherObject as Identity);
        }

        public override int GetHashCode()
        {
            return (this.GetType().GetHashCode() * 907) + this.value.GetHashCode();
        }

        public override string ToString()
        {
            return this.GetType().Name + " [Id=" + this.value + "]";
        }

        public static implicit operator Guid(Identity identity)
        {
            Guid guid;

            if (Guid.TryParse(identity.Value, out guid) == false)
            {
                throw new InvalidCastException("not possible to express [ " + identity.Value + " ] as GUID");
            }

            return guid;
        }

        public static implicit operator string(Identity identity)
        {
            return identity.Value;
        }
    }
}
