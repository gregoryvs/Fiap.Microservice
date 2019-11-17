using AutoMapper;
using GeekBurger.Productions.Contracts;
using GeekBurger.Productions.Models;
using GeekBurger.Productions.Repository;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeekBurger.Productions.Service
{
    public class ProductionAreaChangedService : IProductionAreaChangedService
    {
        private const string Topic = "ProductionAreaChanged";
        private readonly IConfiguration _configuration;
        private readonly ILogService _logService;
        private readonly List<Message> _messages;
        private CancellationTokenSource _cancelMessages;
        private readonly IServiceBusNamespace _namespace;
        private Task _lastTask;
        private IServiceProvider _serviceProvider { get; }

        public ProductionAreaChangedService(
            IConfiguration configuration, ILogService logService, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _logService = logService;
            _messages = new List<Message>();
            _namespace = _configuration.GetServiceBusNamespace();
            _cancelMessages = new CancellationTokenSource();
            _serviceProvider = serviceProvider;
        }
        public async void SendMessagesAsync()
        {
            if (_lastTask != null && !_lastTask.IsCompleted)
                return;

            var config = _configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();
            var topicClient = new TopicClient(config.ConnectionString, Topic);

            _logService.SendMessagesAsync("Production was changed");

            _lastTask = SendAsync(topicClient, _cancelMessages.Token);

            await _lastTask;

            var closeTask = topicClient.CloseAsync();
            await closeTask;
            HandleException(closeTask);
        }

        public async Task SendAsync(TopicClient topicClient,
            CancellationToken cancellationToken)
        {
            var tries = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_messages.Count <= 0)
                    break;

                Message message;
                lock (_messages)
                {
                    message = _messages.FirstOrDefault();
                }

                var sendTask = topicClient.SendAsync(message);
                await sendTask;
                var success = HandleException(sendTask);

                if (!success)
                {
                    var cancelled = cancellationToken.WaitHandle.WaitOne(10000 * (tries < 60 ? tries++ : tries));
                    if (cancelled) break;
                }
                else
                {
                    if (message == null) continue;
                    //AddOrUpdateEvent(new ProductionAreaChangedEvent() { EventId = new Guid(message.MessageId) });
                    _messages.Remove(message);
                }
            }
        }

        private void AddOrUpdateEvent(ProductionAreaChangedEvent productionAreaChangedEvent)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IProductionAreaChangedEventRepository>();

                ProductionAreaChangedEvent evt;
                if (productionAreaChangedEvent.EventId == Guid.Empty
                    || (evt = scopedProcessingService.Get(productionAreaChangedEvent.EventId)) == null)
                    scopedProcessingService.Add(productionAreaChangedEvent);
                else
                {
                    evt.MessageSent = true;
                    scopedProcessingService.Update(evt);
                }

                scopedProcessingService.Save();
            }
        }

        public void EnsureTopicIsCreated()
        {
            if (!_namespace.Topics.List()
                .Any(topic => topic.Name
                    .Equals(Topic, StringComparison.InvariantCultureIgnoreCase)))
                _namespace.Topics.Define(Topic)
                    .WithSizeInMB(1024).Create();

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            EnsureTopicIsCreated();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancelMessages.Cancel();

            return Task.CompletedTask;
        }

        public bool HandleException(Task task)
        {
            if (task.Exception == null || task.Exception.InnerExceptions.Count == 0) return true;

            task.Exception.InnerExceptions.ToList().ForEach(innerException =>
            {
                Console.WriteLine($"Error in SendAsync task: {innerException.Message}. Details:{innerException.StackTrace} ");

                if (innerException is ServiceBusCommunicationException)
                    Console.WriteLine("Connection Problem with Host. Internet Connection can be down");
            });

            return false;
        }

        public void AddToMessageList(IEnumerable<EntityEntry<Production>> changes)
        {
            _messages.AddRange(changes
            .Where(entity => entity.State != EntityState.Detached
                    && entity.State != EntityState.Unchanged)
            .Select(GetMessage).ToList());
        }

        public Message GetMessage(EntityEntry<Production> entity)
        {
            var productionAreaChanged = Mapper.Map<ProductionAreaChangedEvent>(entity);
            var productionAreaChangedSerialized = JsonConvert.SerializeObject(productionAreaChanged);
            var productChangedByteArray = Encoding.UTF8.GetBytes(productionAreaChangedSerialized);

            var productionAreaChangedEvent = Mapper.Map<ProductionAreaChangedEvent>(entity);
            AddOrUpdateEvent(productionAreaChangedEvent);

            return new Message
            {
                Body = productChangedByteArray,
                MessageId = productionAreaChangedEvent.EventId.ToString(),
                Label = productionAreaChanged.ProductionId.ToString()
            };
        }
    }
}
