using System.Collections.Generic;

namespace Lake.Arguments
{
    public interface IArgumentParser
    {
        LakeOptions Parse(IEnumerable<string> args);
    }
}