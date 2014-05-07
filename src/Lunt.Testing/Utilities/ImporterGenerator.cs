using System;
using System.Diagnostics;
using System.Reflection.Emit;
using Castle.DynamicProxy;
using Lunt.IO;

namespace Lunt.Testing
{
    public class ImporterGenerator
    {
        public static IImporter Create<T>(Type defaultProcessor, Type sourceType, Func<Context, IFile, T> func, params string[] extensions)
        {
            // Get attribute builder.
            Type[] ctorTypes = {typeof (string[]), typeof (Type)};
            var ctor = typeof (ImporterAttribute).GetConstructor(ctorTypes);
            Debug.Assert(ctor != null, "Could not get constructor for content importer attribute.");
            object[] arguments = {extensions, defaultProcessor};
            var builder = new CustomAttributeBuilder(ctor, arguments);

            // Create the procy generatin options.
            var proxyOptions = new ProxyGenerationOptions();
            proxyOptions.AdditionalAttributes.Add(builder);

            // Create the proxy generator and create the proxy.
            var proxyGenerator = new ProxyGenerator();
            return (IImporter) proxyGenerator.CreateClassProxy(typeof (FakeImporter<T>), proxyOptions, new object[] {func});
        }
    }
}