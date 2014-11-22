using CartExample.Domain.Carts;
using CartExample.Domain.Products;
using CartExample.Infrastructure;
using CartExample.Mock;
using CartExample.Projections;
using CommonDomain.Aggregates;
using CommonDomain.Implementation;
using CommonDomain.Messaging;
using CommonDomain.Persistence;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CartExample.Console
{
    class Program
    {
        public class StreamNamingConvention : IStreamNamingConvention
        {
            public string GetStreamName(Type type, IIdentity identity)
            {
                return string.Format("{0}-{1}", char.ToLower(type.Name[0]) + type.Name.Substring(1), identity.Value);
            }
        }

    public class MyEntityId : MyIdentity
    {
        public MyEntityId(Guid identityValue)
            : base(identityValue)
        {
        }
    }

    public class IncludeNonPublicMembersContractResolver : DefaultContractResolver
    {
        public IncludeNonPublicMembersContractResolver()
        {
            
        }

        override protected List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            BindingFlags flags;

            if (objectType == typeof(MyIdentity))
            {
                flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            }
            else
            {
                flags = BindingFlags.Instance | BindingFlags.Public;
            }

            MemberInfo[] fields = objectType.GetFields(flags);

            return fields
                .Concat(objectType.GetProperties(flags)
                .Where(propInfo => propInfo.CanWrite || propInfo.CanRead))
                .ToList();

        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, MemberSerialization.Fields);
        }   

        /*protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);

            if (type == typeof(MyIdentity))
            {
                var property = properties.Single(x =>
                         x.PropertyName == "Value"
                         && x.DeclaringType == JsonProperty. // member.DeclaringType);
                property.Readable = true;
            }

            return properties;
        }*/
    }

    public abstract class MyIdentity
    {
        protected bool convertableAsGuid;
        protected string value;
        public string Value
        {
            get { return this.value; }

            protected set
            {
                this.value = value;

                Guid guid;
                if (Guid.TryParse(this.Value, out guid) == false)
                    this.convertableAsGuid = false;
            }
        }

        public MyIdentity(string value)
        {
            this.Value = value;

            
        }

        public MyIdentity(Guid value)
        {
            this.Value = value.ToString();
        }
    }

        public class MyEvent
        {
            public MyEvent(MyEntityId id, string name)
            {
                this.Id = id;
                this.Name = name;
            }

            public readonly MyEntityId Id;
            public readonly string Name;
        }


        static void Main(string[] args)
        {
            var container = new Container();
            Wiring.WireUp(container);

        }
    }
}
