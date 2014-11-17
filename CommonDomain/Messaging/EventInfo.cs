using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Messaging
{
    public class EventInfo
    {
        public readonly string Identity;
        public readonly string EventType;
        public readonly int EventNumber;
        public readonly int OriginalPosition;
        public readonly Guid UniqueEventId;
        public readonly DateTime Timestamp;
        public readonly Byte[] Payload;

        public EventInfo(
            string identity,
            string eventType,
            int eventNumber,
            int originalPosition,
            Guid uniqueEventId,
            DateTime timestamp,
            byte[] payload)
        {
            this.Identity = identity;
            this.EventType = eventType;
            this.EventNumber = eventNumber;
            this.OriginalPosition = originalPosition;
            this.UniqueEventId = uniqueEventId;
            this.Timestamp = timestamp;
            this.Payload = payload;
        }
    }
}
