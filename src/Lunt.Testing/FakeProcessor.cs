using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection.Emit;
using Castle.DynamicProxy;

namespace Lunt.Testing
{
    public class FakeProcessor : ILuntProcessor
    {
        private readonly Func<LuntContext, object, object> _func;
        private readonly Type _sourceType;
        private readonly Type _targetType;

        public FakeProcessor(Func<LuntContext, object, object> func, Type sourceType, Type targetType)
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

        public object Process(LuntContext context, object source)
        {
            return _func(context, source);
        }
    }

    public class FakeProcessor<TSource, TTarget> : LuntProcessor<TSource, TTarget>
    {
        private readonly Func<LuntContext, TSource, TTarget> _func;

        public FakeProcessor(Func<LuntContext, TSource, TTarget> func)
        {
            _func = func;
        }

        public static ILuntProcessor Mock(Func<LuntContext, TSource, TTarget> func, string name)
        {
            // Get attribute builder.
            Type[] ctorTypes = {typeof (string)};
            var ctor = typeof (DisplayNameAttribute).GetConstructor(ctorTypes);
            Debug.Assert(ctor != null, "Could not get constructor for content importer attribute.");
            object[] arguments = {name};
            var builder = new CustomAttributeBuilder(ctor, arguments);

            // Create the procy generatin options.
            var proxyOptions = new ProxyGenerationOptions();
            proxyOptions.AdditionalAttributes.Add(builder);

            // Create the proxy generator and create the proxy.
            var proxyGenerator = new ProxyGenerator();
            return (ILuntProcessor) proxyGenerator.CreateClassProxy(typeof (FakeProcessor<TSource, TTarget>),
                proxyOptions, new object[] {func});
        }

        public override TTarget Process(LuntContext context, TSource source)
        {
            return _func(context, source);
        }
    }
}