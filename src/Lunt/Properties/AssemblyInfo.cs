using System;
using System.Reflection;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
using System.Runtime.CompilerServices;

[assembly: AssemblyTitle("Lunt.Core")]
[assembly: AssemblyDescription("")]

// We're CLS compliant.
[assembly: CLSCompliant(true)]

// Expose internals to the test projects.
[assembly: InternalsVisibleTo("Lunt.Tests")]