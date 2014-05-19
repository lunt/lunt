using System.Collections.Generic;
using System.Linq;
using Lunt;
using Lunt.Bootstrapping;
using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Runtime;

namespace Lake
{
    internal class LakeInternalConfiguration : IInternalConfiguration
    {
        private readonly IBuildLog _log;
        private readonly IBuildEnvironment _environment;
        private readonly IPipelineScanner _scanner;

        public LakeInternalConfiguration(IBuildLog log, IBuildEnvironment environment, IPipelineScanner scanner)
        {
            _log = log;
            _environment = environment;
            _scanner = scanner;
        }

        public IEnumerable<ContainerRegistration> GetRegistrations()
        {
            var configuration = new InternalConfiguration();
            var registrations = configuration.GetRegistrations().ToList();

            ReplaceRegistration(registrations, new InstanceRegistration(typeof(IBuildLog), _log));
            ReplaceRegistration(registrations, new InstanceRegistration(typeof(IBuildEnvironment), _environment));
            ReplaceRegistration(registrations, new InstanceRegistration(typeof(IFileSystem), _environment.FileSystem));
            ReplaceRegistration(registrations, new InstanceRegistration(typeof(IPipelineScanner), _scanner));

            return registrations;
        }

        private static void ReplaceRegistration(List<ContainerRegistration> registrations, ContainerRegistration registration)
        {
            registrations.RemoveAll(x => x.RegistrationType == registration.RegistrationType);
            registrations.Add(registration);
        }
    }
}
