using RabbitMQ.Client.Core.DependencyInjection.Services;
using RabbitMQ.Client;

namespace CarnetsTests.IntegrationTests.ControllersTests.Mocks
{
    public class MockQueueService : IQueueService
    {
        public IConnection ConsumingConnection => null;

        public IModel ConsumingChannel => null;

        public IConnection Connection => null;

        public IModel Channel => null;

        public void Send<T>(T @object, string exchangeName, string routingKey) where T : class
        {
        }

        public void Send<T>(T @object, string exchangeName, string routingKey, int millisecondsDelay) where T : class
        {
        }

        public void Send(ReadOnlyMemory<byte> bytes, IBasicProperties properties, string exchangeName, string routingKey)
        {
        }

        public void Send(ReadOnlyMemory<byte> bytes, IBasicProperties properties, string exchangeName, string routingKey, int millisecondsDelay)
        {
        }

        public Task SendAsync<T>(T @object, string exchangeName, string routingKey) where T : class
        {
            return Task.CompletedTask;
        }

        public Task SendAsync<T>(T @object, string exchangeName, string routingKey, int millisecondsDelay) where T : class
        {
            return Task.CompletedTask;
        }

        public Task SendAsync(ReadOnlyMemory<byte> bytes, IBasicProperties properties, string exchangeName, string routingKey)
        {
            return Task.CompletedTask;
        }

        public Task SendAsync(ReadOnlyMemory<byte> bytes, IBasicProperties properties, string exchangeName, string routingKey, int millisecondsDelay)
        {
            return Task.CompletedTask;
        }

        public void SendJson(string json, string exchangeName, string routingKey)
        {
        }

        public void SendJson(string json, string exchangeName, string routingKey, int millisecondsDelay)
        {
        }

        public Task SendJsonAsync(string json, string exchangeName, string routingKey)
        {
            return Task.CompletedTask;
        }

        public Task SendJsonAsync(string json, string exchangeName, string routingKey, int millisecondsDelay)
        {
            return Task.CompletedTask;
        }

        public void SendString(string message, string exchangeName, string routingKey)
        {
        }

        public void SendString(string message, string exchangeName, string routingKey, int millisecondsDelay)
        {
        }

        public Task SendStringAsync(string message, string exchangeName, string routingKey)
        {
            return Task.CompletedTask;
        }

        public Task SendStringAsync(string message, string exchangeName, string routingKey, int millisecondsDelay)
        {
            return Task.CompletedTask;
        }

        public void StartConsuming()
        {
        }

        public void StopConsuming()
        {
        }
    }
}
