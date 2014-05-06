using Lunt.IO;

namespace Lunt.Testing
{
    public class FakeHashComputer : IHashComputer
    {
        private readonly string _hash;

        public string Hash
        {
            get { return _hash; }
        }

        public FakeHashComputer(string hash)
        {
            _hash = hash;
        }

        public string Compute(IFile file)
        {
            return Hash;
        }

        public void Dispose()
        {
        }
    }
}