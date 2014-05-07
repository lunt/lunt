using System;
using Lunt.IO;

namespace Lunt.Testing
{
    public class FakeImporter<T> : IImporter
    {
        private readonly Func<Context, IFile, T> _func;

        public FakeImporter(Func<Context, IFile, T> func)
        {
            _func = func;
        }

        public static IImporter Mock(Func<Context, IFile, T> func, string extension, Type defaultProcessor = null)
        {
            return ImporterGenerator.Create(defaultProcessor, typeof (T), func, extension);
        }

        object IImporter.Import(Context context, IFile file)
        {
            return _func(context, file);
        }
    }
}