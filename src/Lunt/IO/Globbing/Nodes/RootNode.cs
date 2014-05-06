﻿///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

namespace Lunt.IO.Globbing
{
    internal abstract class RootNode : Node
    {
        public override bool IsWildcard
        {
            get { return false; }
        }
    }
}