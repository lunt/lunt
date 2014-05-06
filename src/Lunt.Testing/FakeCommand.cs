using System;
using Lake.Commands;

namespace Lunt.Testing
{
    public class FakeCommand : ICommand
    {
        private readonly Func<int> _func;

        public FakeCommand(Func<int> func = null)
        {
            _func = func ?? (() => 0);
        }

        public int Execute()
        {
            return _func();
        }
    }
}