using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lunt.Diagnostics;

namespace Lunt.Runtime
{
    internal sealed class AssemblyTypeScanner
    {
        private readonly IBuildLog _log;

        public AssemblyTypeScanner(IBuildLog log)
        {
            _log = log;
        }

        public IEnumerable<T> Scan<T>(Assembly assembly, bool log = false)
        {
            if (assembly.IsDynamic)
            {
                yield break;
            }

            var types = GetLoadableTypes(assembly);
            foreach (var type in types)
            {
                if (type.IsClass && !type.IsAbstract)
                {
                    if (typeof (T).IsAssignableFrom(type))
                    {
                        if (log)
                        {
                            _log.Verbose("Found component '{0}'", type.Name);
                        }

                        // Got an empty constructor?
                        var emptyConstructor = type.GetConstructor(Type.EmptyTypes);
                        if (emptyConstructor == null)
                        {
                            _log.Warning(Verbosity.Quiet, "Skipping component '{0}' (no parameterless constructor).", type.FullName);
                            continue;
                        }

                        yield return (T) Activator.CreateInstance(type);
                    }
                }
            }
        }

        private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}