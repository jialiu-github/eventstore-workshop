using System;
using Eventstore_workshop;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Embedded;
using Xunit;

namespace Eventstore_workshop_test
{
    public class EventFact
    {
        [Fact]
        public async void should_raise_user_created()
        {
            var clusterVNode = EmbeddedVNodeBuilder.AsSingleNode().OnDefaultEndpoints().RunInMemory().Build();
            await clusterVNode.StartAndWaitUntilReady();
            var connection = EmbeddedEventStoreConnection.Create(clusterVNode);
            await connection.ConnectAsync();

            var userCreated = new UserCreated {Name="name"};
            await connection.AppendToStreamAsync("user-1", ExpectedVersion.Any,
                new EventData(Guid.NewGuid(), "UserCreated", true, Json.SerializeToBytes(userCreated), new byte[] {}));

            var streamEventsSlice = await connection.ReadStreamEventsForwardAsync("user-1", 0, 100, false);
            Assert.Single(streamEventsSlice.Events);
            var fromBytes = Json.DeserializeFromBytes<UserCreated>(streamEventsSlice.Events[0].Event.Data);
            Assert.Equal("name", fromBytes.Name);
        }
    }

    class UserCreated
    {
        public string Name { get; set; }
    }
}
