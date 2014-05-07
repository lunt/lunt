using System;

namespace Lunt.Descriptors
{
    internal sealed class ProcessorDescriptor : ComponentDescriptor
    {
        private readonly IProcessor _processor;
        private readonly Type _sourceType;
        private readonly Type _targetType;

        public IProcessor Processor
        {
            get { return _processor; }
        }

        public Type SourceType
        {
            get { return _sourceType; }
        }

        public Type TargetType
        {
            get { return _targetType; }
        }

        public ProcessorDescriptor(IProcessor processor)
        {
            _processor = processor;
            _sourceType = processor.GetSourceType();
            _targetType = processor.GetTargetType();
        }

        public override Type GetComponentType()
        {
            return _processor.GetType();
        }
    }
}