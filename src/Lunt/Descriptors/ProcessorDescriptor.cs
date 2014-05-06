using System;

namespace Lunt.Descriptors
{
    internal sealed class ProcessorDescriptor : ComponentDescriptor
    {
        private readonly ILuntProcessor _processor;
        private readonly Type _sourceType;
        private readonly Type _targetType;

        public ILuntProcessor Processor
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

        public ProcessorDescriptor(ILuntProcessor processor)
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