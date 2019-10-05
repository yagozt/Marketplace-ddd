using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marketplace.Framework
{
    public abstract class Entity
    {
        private readonly List<object> _events;
        protected Entity() => _events = new List<object>();
        protected void Apply(object @event)
        {
            When(@event);
            EnsureValidState();
            _events.Add(@event);
        }

        protected abstract void When(object @event);
        protected abstract void EnsureValidState();
        protected void Raise(object @event) => _events.Add(@event);
        public IEnumerable<object> GetChanges() => _events.AsEnumerable();
        public void ClearChanges() => _events.Clear();
    }
    public abstract class Entity<TId> : IInternalEventHandler where TId : Value<TId>
    {
        private readonly Action<object> _applier;
        public TId Id { get; protected set; }
        protected Entity(Action<object> applier) => _applier = applier;
        protected abstract void When(object @event);
        protected void Apply(object @event)
        {
            When(@event);
            _applier(@event);
        }
        void IInternalEventHandler.Handle(object @event) => When(@event);
    }
}
