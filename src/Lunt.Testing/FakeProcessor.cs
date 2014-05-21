using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection.Emit;
using Castle.DynamicProxy;

namespace Lunt.Testing
{
    public class FakeProcessor : IProcessor
    {
        private readonly Func<Context, object, object> _func;
        private readonly Type _sourceType;
        private readonly Type _targetType;

        public FakeProcessor(Func<Context, object, object> func, Type sourceType, Type targetType)
        {
            _func = func;
            _sourceType = sourceType;
            _targetType = targetType;
        }

        public Type GetSourceType()
        {
            return _sourceType;
        }

        public Type GetTargetType()
        {
            return _targetType;
        }

        public object Process(Context context, object source)
        {
            return _func(context, source);
        }
    }

    public class FakeProcessor<TSource, TTarget> : Processor<TSource, TTarget>
    {
        private readonly Func<Context, TSource, TTarget> _func;

        public FakeProcessor(Func<Context, TSource, TTarget> func)
        {
            _func = func;
        }

        public static IProcessor Mock(Func<Context, TSource, TTarget> func, string name)
        {
            // Get attribute builder.
            Type[] ctorTypes = {typeof (string)};
            var ctor = typeof (DisplayNameAttribute).GetConstructor(ctorTypes);
            Debug.Assert(ctor != null, "Could not get constructor for content importer attribute.");
            object[] arguments = {name};
            var builder = new CustomAttributeBuilder(ctor, arguments);

            // CreateCommand the procy generatin options.
            var proxyOptions = new ProxyGenerationOptions();
            proxyOptions.AdditionalAttributes.Add(builder);

            // CreateCommand the proxy generator and create the proxy.
            var proxyGenerator = new ProxyGenerator();
            return (IProcessor) proxyGenerator.CreateClassProxy(typeof (FakeProcessor<TSource, TTarget>),
                proxyOptions, new object[] {func});
        }

        public override TTarget Process(Context context, TSource source)
        {
            return _func(context, source);
        }
    }
}