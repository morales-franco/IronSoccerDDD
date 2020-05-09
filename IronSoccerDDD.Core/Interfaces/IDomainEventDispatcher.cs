using System.Collections.Generic;

namespace IronSoccerDDD.Core.Interfaces
{
    public interface IDomainEventDispatcher
    {
        void Dispatch(IEnumerable<IDomainEvent> events);
    }
}
