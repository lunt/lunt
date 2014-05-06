using System;
using Lunt.IO;
using Moq;

namespace Lunt.Testing
{
    public class FakeWriter : ILuntWriter
    {
        private readonly Action<LuntContext, IFile, object> _callback;
        private readonly Type _targetType;

        public FakeWriter(Action<LuntContext, IFile, object> callback, Type targetType)
        {
            _callback = callback;
            _targetType = targetType;
        }


        public Type GetTargetType()
        {
            return _targetType;
        }

        public void Write(LuntContext context, IFile target, object value)
        {
            _callback(context, target, value);
        }
    }

    public class FakeWriter<T> : LuntWriter<T>
    {
        private readonly Action<LuntContext, IFile, T> _action;

        public FakeWriter(Action<LuntContext, IFile, T> action)
        {
            _action = action;
        }

        public override void Write(LuntContext context, IFile target, T value)
        {
            _action(context, target, value);
        }

        public static ILuntWriter Mock(Action<LuntContext, IFile, T> action)
        {
            var mock = new Mock<ILuntWriter>();
            mock.Setup(w => w.GetTargetType()).Returns(typeof (T));
            mock.Setup(w => w.Write(It.IsAny<LuntContext>(), It.IsAny<IFile>(), It.IsAny<object>()))
                .Callback((LuntContext context, IFile file, object obj) => action(context, file, (T) obj));
            return mock.Object;
        }

        public static ILuntWriter MockWithoutCallback(Type targetType)
        {
            var mock = new Mock<ILuntWriter>();
            mock.Setup(w => w.GetTargetType()).Returns(targetType ?? typeof (T));
            mock.Setup(w => w.Write(It.IsAny<LuntContext>(), It.IsAny<IFile>(), It.IsAny<object>()))
                .Callback((LuntContext context, IFile file, object obj) => { });
            return mock.Object;
        }
    }
}