using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Lake")]
[assembly: AssemblyDescription("")]

// Expose internals to the test projects.
[assembly: InternalsVisibleTo("Lake.Tests")]
[assembly: InternalsVisibleTo("Lake.Tests.Integration")]
[assembly: InternalsVisibleTo("Lunt.Testing")]

[assembly: ComVisible(false)]