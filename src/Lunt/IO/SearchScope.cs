namespace Lunt.IO
{
    /// <summary>
    /// Specifies whether to search the current directory, or the current directory and all subdirectories.
    /// </summary>
    public enum SearchScope
    {
        /// <summary>
        /// The current directory.
        /// </summary>
        Current,

        /// <summary>
        /// The current directory and all subdirectories.
        /// </summary>
        Recursive
    }
}