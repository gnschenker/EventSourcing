using System.Net;
using EventSourcing.Infrastructure;
using EventStore.ClientAPI;

namespace EventSourcing
{
    public class Bootstrap
    {
        public static void Init()
        {
            var connection = GetEventStoreConnection();
            IoC.GesRepository = new GesRepository(connection);
        }

        private static IEventStoreConnection GetEventStoreConnection()
        {
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var endpoint = new IPEndPoint(ipAddress, 1113);
            var connection = EventStoreConnection.Create(endpoint);
            connection.ConnectAsync().Wait();
            return connection;
        }
    }
}