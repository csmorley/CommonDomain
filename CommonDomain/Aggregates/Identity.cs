using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Aggregates
{
    public abstract class Identity : IEquatable<Identity>, IIdentity
    {
        protected readonly bool isConstructedWithGuid;
        protected readonly string identityValue;
        public bool IsConvertableToGuid { get { return this.isConstructedWithGuid; } }
        public string IdentityValue { get { return this.identityValue; } }

        public Identity()
        {
            this.identityValue = Guid.NewGuid().ToString();
        }

        public Identity(string value)
        {
            this.identityValue = value;
            this.isConstructedWithGuid = false;
        }

        public Identity(Guid id)
        {
            this.identityValue = id.ToString();
            this.isConstructedWithGuid = true;
        }

        public bool Equals(Identity id)
        {
            if (object.ReferenceEquals(this, id)) return true;
            if (object.ReferenceEquals(null, id)) return false;
            return this.identityValue.Equals(id.identityValue);
        }

        public override bool Equals(object anotherObject)
        {
            return Equals(anotherObject as Identity);
        }

        public override int GetHashCode()
        {
            return (this.GetType().GetHashCode() * 907) + this.identityValue.GetHashCode();
        }

        public override string ToString()
        {
            return this.GetType().Name + " [Id=" + this.identityValue + "]";
        }

        public static implicit operator Guid(Identity identity)
        {
            if (identity.IsConvertableToGuid == false)
                throw new InvalidCastException("not possible to express [ " + identity.IdentityValue + " ] as GUID");

            return new Guid(identity.IdentityValue);
        }

        public static implicit operator string(Identity identity)
        {
            if (identity.IsConvertableToGuid == true)
                throw new InvalidCastException("not possible to express [ " + identity.IdentityValue + " ] as string");

            return identity.IdentityValue;
        }
    }
}
