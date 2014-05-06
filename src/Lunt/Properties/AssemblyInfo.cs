using System;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyTitle("Lunt")]
[assembly: AssemblyDescription("")]

// Expose internals to the test projects.
[assembly: InternalsVisibleTo("Lunt.Tests")]

// We're CLS compliant.
[assembly: CLSCompliant(true)]