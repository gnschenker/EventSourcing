using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;

namespace ReadModelBuilder
{
    class Program
    {
        private static EventStoreAllCatchUpSubscription _subscription;
        private static Dictionary<Type, Info[]> _dictionary = new Dictionary<Type, Info[]>();
        const string MongoDbConnectionString = "mongodb://localhost:27017";
        const string MongoDbDatabaseName = "HeartBeat";

        static void Main()
        {
            WireObservers();
            var credentials = new UserCredentials("admin", "changeit");
            var connection = GetEventStoreConnection();
            _subscription = connection.SubscribeToAllFrom(Position.Start, false,
                EventAppeared,
                OnLiveProcessingStarted,
                OnSubscriptionDropped,
                credentials);

            Console.WriteLine("Hit <enter> to exit:");
            Console.ReadLine();
        }

        private static void EventAppeared(EventStoreCatchUpSubscription subscription, ResolvedEvent re)
        {
            // skip internal events
            if (re.OriginalEvent.EventType.StartsWith("$")) return;  
            // skip "invalid" events
            if (re.OriginalEvent.Metadata == null || re.OriginalEvent.Metadata.Any() == false) return;
            try
            {
                var e = re.DeserializeEvent();
                Dispatch(e).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem while processing event {re.OriginalEvent.EventType}\r\n{ex.Message}");
            }
        }

        private static async Task Dispatch(object e)
        {
            var eventType = e.GetType();
            if (_dictionary.ContainsKey(eventType) == false)
                return;
            foreach (var item in _dictionary[eventType])
            {
                try
                {
                    await ((Task)item.MethodInfo.Invoke(item.Observer, new[] {e}));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not dispatch event {eventType.Name} to observer {item.Observer.GetType().Name}\r\n{ex.Message}");
                }
            }
        }

        private static void OnLiveProcessingStarted(EventStoreCatchUpSubscription subscription)
        {
            Console.WriteLine("Live processing started");
        }

        private static void OnSubscriptionDropped(EventStoreCatchUpSubscription subscription, SubscriptionDropReason reason, Exception ex)
        {
            Console.WriteLine($"Subscription dropped, reason={reason}: {ex.Message}");
        }

        private static void WireObservers()
        {
            var factory = new MongoDbAtomicWriterFactory(MongoDbConnectionString, MongoDbDatabaseName);
            var observers = new  ObserverRegistry().GetObservers(factory);
            _dictionary = observers.Select(o => new { Observer = o, Type = o.GetType()})
                .Select(x => new
                {
                    x.Observer,
                    MethodInfos = x.Type
                        .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(m => m.Name == "When" && m.GetParameters().Length == 1)
                })
                .SelectMany(x => x.MethodInfos.Select(
                    y => new {x.Observer, MethodInfo=y, y.GetParameters().First().ParameterType}))
                .GroupBy(x => x.ParameterType)
                .ToDictionary(g => g.Key, 
                    g => g.Select(y => new Info {Observer = y.Observer, MethodInfo = y.MethodInfo}).ToArray());
        }

        private static IEventStoreConnection GetEventStoreConnection()
        {
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var endpoint = new IPEndPoint(ipAddress, 1113);
            var connection = EventStoreConnection.Create(endpoint);
            connection.ConnectAsync().Wait();
            return connection;
        }

        public class Info
        {
            public MethodInfo MethodInfo { get; set; }
            public object Observer { get; set; }
        }
    }
}
