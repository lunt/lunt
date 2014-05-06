using System;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyTitle("Lunt.Console")]
[assembly: AssemblyDescription("")]

// Expose internals to the test projects.
[assembly: InternalsVisibleTo("Lake.Tests")]
[assembly: InternalsVisibleTo("Lake.Tests.Integration")]
[assembly: InternalsVisibleTo("Lunt.Tests.Framework")]

// We're CLS compliant.
[assembly: CLSCompliant(true)]