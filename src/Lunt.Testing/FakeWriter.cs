using System;
using Lunt.IO;
using Moq;

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

        public static IWriter Mock(Action<Context, IFile, T> action)
        {
            var mock = new Mock<IWriter>();
            mock.Setup(w => w.GetTargetType()).Returns(typeof (T));
            mock.Setup(w => w.Write(It.IsAny<Context>(), It.IsAny<IFile>(), It.IsAny<object>()))
                .Callback((Context context, IFile file, object obj) => action(context, file, (T) obj));
            return mock.Object;
        }

        public static IWriter MockWithoutCallback(Type targetType)
        {
            var mock = new Mock<IWriter>();
            mock.Setup(w => w.GetTargetType()).Returns(targetType ?? typeof (T));
            mock.Setup(w => w.Write(It.IsAny<Context>(), It.IsAny<IFile>(), It.IsAny<object>()))
                .Callback((Context context, IFile file, object obj) => { });
            return mock.Object;
        }
    }
}