﻿using System;
using TheFlow.CoreConcepts;
using TheFlow.Elements.Data;

namespace TheFlow.Elements.Events
{
    public class CatchAnyEventCatcher 
        : IEventCatcher, IDataProducer
    {
        private CatchAnyEventCatcher()
        {}
        
        public static IEventCatcher Create() 
            => new CatchAnyEventCatcher();

        public bool CanHandle(ExecutionContext context, object @event) => true;

        public void Handle(ExecutionContext context, object @event)
        {
            
        }
        
        private DataOutput _dataOutput;

        public void SetEventDataOutput(DataOutput dataOutput)
        {
            _dataOutput = dataOutput;
        }

        public DataOutput GetDataOutputByName(string name) => 
            _dataOutput?.Name == name ? _dataOutput : null;
    }
}