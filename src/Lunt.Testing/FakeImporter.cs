using System;
using Lunt.IO;

namespace Lunt.Testing
{
    public class FakeImporter<T> : ILuntImporter
    {
        private readonly Func<LuntContext, IFile, T> _func;

        public FakeImporter(Func<LuntContext, IFile, T> func)
        {
            _func = func;
        }

        public static ILuntImporter Mock(Func<LuntContext, IFile, T> func, string extension, Type defaultProcessor = null)
        {
            return ImporterGenerator.Create(defaultProcessor, typeof (T), func, extension);
        }

        object ILuntImporter.Import(LuntContext context, IFile file)
        {
            return _func(context, file);
        }
    }
}