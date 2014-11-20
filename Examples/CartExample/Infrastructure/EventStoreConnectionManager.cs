using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace CartExample.Infrastructure
{
    public class EventStoreConnectionManager
    {
        static public IEventStoreConnection Init()
        {
            var integrationTestTcpEndPoint = new IPEndPoint(IPAddress.Loopback, 1113);
            var connection = EventStoreConnection.Create(integrationTestTcpEndPoint);
            var serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };
            connection.ConnectAsync().Wait();
            return connection;
        }
    }
}