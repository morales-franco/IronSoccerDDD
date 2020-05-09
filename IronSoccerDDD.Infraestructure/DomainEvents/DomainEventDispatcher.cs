using IronSoccerDDD.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace IronSoccerDDD.Infraestructure.DomainEvents
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Dispatch(IEnumerable<IDomainEvent> events)
        {
            foreach (var domainEvent in events)
            {
                CallHandler(domainEvent);
            }
        }

        private void CallHandler(IDomainEvent domainEvent)
        {
            Type handlerType = typeof(IHandler<>).MakeGenericType(domainEvent.GetType());
            var service = (dynamic)_serviceProvider.GetService(handlerType);

            if (service == null)
                return;

            service.Handle((dynamic)domainEvent);
        }
    }
}
