using System;

namespace Lunt.Descriptors
{
    internal sealed class WriterDescriptor : ComponentDescriptor
    {
        private readonly ILuntWriter _writer;
        private readonly Type _targetType;

        public ILuntWriter Writer
        {
            get { return _writer; }
        }

        public Type TargetType
        {
            get { return _targetType; }
        }

        public WriterDescriptor(ILuntWriter writer)
        {
            _writer = writer;
            _targetType = writer.GetTargetType();
        }

        public override Type GetComponentType()
        {
            return _writer.GetType();
        }
    }
}