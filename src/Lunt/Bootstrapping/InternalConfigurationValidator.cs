using System.Collections.Generic;
using System.Linq;
using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Runtime;

namespace Lunt.Bootstrapping
{
    internal static class InternalConfigurationValidator
    {
        public static void Validate(IEnumerable<ContainerRegistration> registrations)
        {
            var types = new[] {
                typeof(IBuildKernel), typeof(IBuildEnvironment), typeof(IFileSystem),
                typeof(IBuildLog), typeof(IHashComputer), typeof(IPipelineScanner), 
                typeof(IBuildConfigurationReader), typeof(IBuildManifestProvider)
            };

            var containerRegistrations = registrations as ContainerRegistration[] ?? registrations.ToArray();

            foreach (var type in types)
            {
                if (!containerRegistrations.Any(x => type.IsAssignableFrom(x.RegistrationType)))
                {
                    const string format = "No registration for type '{0}' could be found.";
                    throw new LuntException(string.Format(format, type.FullName));
                }
            }
        }
    }
}
