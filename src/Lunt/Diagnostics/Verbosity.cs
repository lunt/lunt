using System.ComponentModel;

namespace Lunt.Diagnostics
{
    /// <summary>
    /// Specifies the amount of information to display in the build log.
    /// </summary>
    [TypeConverter(typeof (VerbosityTypeConverter))]
    public enum Verbosity
    {
        /// <summary>
        /// Quiet
        /// </summary>
        Quiet = 0,

        /// <summary>
        /// Minimal
        /// </summary>
        Minimal = 1,

        /// <summary>
        /// Normal
        /// </summary>
        Normal = 2,

        /// <summary>
        /// Verbose
        /// </summary>
        Verbose = 3,

        /// <summary>
        /// Diagnostic
        /// </summary>
        Diagnostic = 4
    }
}