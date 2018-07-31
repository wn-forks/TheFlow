﻿using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using TheFlow;
using TheFlow.CoreConcepts;
using TheFlow.Elements.Activities;
using TheFlow.Elements.Events;
using TheFlow.Infrastructure;
using TheFlow.Infrastructure.Parallel;
using TheFlow.Infrastructure.Stores;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = ProcessModel.Create()
                .AddEventCatcher("start")
                .AddActivity("activity", new UserActivity())
                .AddEventThrower("end")
                .AddSequenceFlow("start", "activity", "end");
            
            var instances = new InMemoryProcessInstancesStore();
            var models = new InMemoryProcessModelsStore(model);
            
            var manager = new ProcessManager(models, instances);

            var result = manager.HandleEvent(null).FirstOrDefault();
            var instance = manager
                .InstancesStore
                .GetById(result.ProcessInstanceId);
            
            Console.WriteLine(instance.IsDone);


            result = manager.HandleActivityCompletion(
                result.ProcessInstanceId,
                result.AffectedTokens.First(),
                null
            );

            Console.WriteLine(instance.IsDone);
        }
    }
}
