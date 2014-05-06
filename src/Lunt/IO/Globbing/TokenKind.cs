///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

namespace Lunt.IO.Globbing
{
    internal enum TokenKind
    {
        Wildcard,
        CharacterWildcard,
        DirectoryWildcard,
        PathSeparator,
        Identifier,
        WindowsRoot,
        EndOfText
    }
}