using System;
using Lunt.IO;
using NSubstitute;

namespace Lunt.Testing
{
    public class FakeWriter : IWriter
    {
        private readonly Action<Context, IFile, object> _callback;
        private readonly Type _targetType;

        public FakeWriter(Action<Context, IFile, object> callback, Type targetType)
        {
            _callback = callback;
            _targetType = targetType;
        }


        public Type GetTargetType()
        {
            return _targetType;
        }

        public void Write(Context context, IFile target, object value)
        {
            _callback(context, target, value);
        }
    }

    public class FakeWriter<T> : Writer<T>
    {
        private readonly Action<Context, IFile, T> _action;

        public FakeWriter(Action<Context, IFile, T> action)
        {
            _action = action;
        }

        public override void Write(Context context, IFile target, T value)
        {
            _action(context, target, value);
        }

        public static IWriter Create(Action<Context, IFile, T> action)
        {
            var writer = Substitute.For<IWriter>();
            writer.GetTargetType().Returns(typeof(T));
            writer.When(x => x.Write(Arg.Any<Context>(), Arg.Any<IFile>(), Arg.Any<object>()))
                .Do(a => action(a.Arg<Context>(), a.Arg<IFile>(), (T)a.Arg<object>()));
            return writer;
        }
    }
}