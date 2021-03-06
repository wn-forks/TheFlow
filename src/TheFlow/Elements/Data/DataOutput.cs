using System;
using System.Linq;
using TheFlow.CoreConcepts;

namespace TheFlow.Elements.Data
{
    public class DataOutput : IDataProducer
    {
        public string Name { get; }

        public DataOutput(string name)
        {
            if (name == string.Empty)
            {
                throw new ArgumentException(name);
            }
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
        
        
        public void Update(
            ExecutionContext context,
            string parentElementId,
            object newValue
            )
        {

            if (Name == "default" && context.Token != null)
            {
                context.Token.LastDefaultOutput = newValue;
            }
            
            var allAssociations = context.Model.Elements
                .OfType<INamedProcessElement<DataAssociation>>();
            
            var associations = allAssociations
                .Select(namedAssociation => namedAssociation.Element)
                .Where(association =>
                    association.DataProducerName == parentElementId &&
                    association.OutputName == Name
                );

            foreach (var association in associations)
            {
                // TODO: Target not found?
                var consumerElement = context.Model.GetElementByName(association.DataConsumerName)
                    ?.Element as IDataConsumer;
                var input = consumerElement?.GetDataInputByName(association.InputName);

                input?.Update(context, association.DataConsumerName, newValue);
            }
        }

        public DataOutput GetDataOutputByName(string name) 
            => name == Name ? this : null;

        public static implicit operator DataOutput(string name) 
            => new DataOutput(name);
    }
}