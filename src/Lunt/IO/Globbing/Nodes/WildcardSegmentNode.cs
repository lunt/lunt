///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

namespace Lunt.IO.Globbing
{
    internal sealed class WildcardSegmentNode : Node
    {
        public override bool IsWildcard
        {
            get { return true; }
        }

        public override string Render()
        {
            return ".*";
        }
    }
}