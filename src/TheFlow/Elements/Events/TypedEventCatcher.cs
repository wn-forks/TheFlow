﻿using TheFlow.CoreConcepts;
using TheFlow.Elements.Data;

namespace TheFlow.Elements.Events
{
    public class TypedEventCatcher<TEvent> 
        : IEventCatcher, IDataProducer
        where TEvent : class
    {
        bool IEventCatcher.CanHandle(ExecutionContext context, object @event) => 
            CanHandleImpl(context, @event as TEvent);

        void IEventCatcher.Handle(ExecutionContext context, object @event)
        {
            context.Instance.SetDataObjectValue(
                context.Model.Conventions.Naming.DataObjectName(context.Token.ExecutionPoint),
                @event);
            _dataOutput?.Update(context, context.Token.ExecutionPoint, @event);

            HandleImpl(context, @event as TEvent);
        }

        private DataOutput _dataOutput = new DataOutput("default");
        void IEventCatcher.SetEventDataOutput(DataOutput dataOutput)
        {
            _dataOutput = dataOutput;
        }
        
        public bool CanHandle(ExecutionContext context,TEvent @event) =>
            CanHandleImpl(context, @event);

        public void Handle(ExecutionContext context, TEvent @event)
        {
            context.Instance.SetDataObjectValue(
                context.Model.Conventions.Naming.DataObjectName(context.Token.ExecutionPoint),
                @event
                );
            _dataOutput?.Update(context, context.Token.ExecutionPoint, @event);

            HandleImpl(context, @event);
        }

        protected virtual bool CanHandleImpl(ExecutionContext context, TEvent @event)
        {
            return @event != null;
        }

        protected virtual void HandleImpl(ExecutionContext context, TEvent @event)
        {
            return;
        }
        
        public static TypedEventCatcher<TEvent> Create() =>
            new TypedEventCatcher<TEvent>();

        public DataOutput GetDataOutputByName(string name) =>
            _dataOutput?.Name == name ? _dataOutput : null;
    }
}