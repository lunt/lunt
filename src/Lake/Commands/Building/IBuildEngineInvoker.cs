using Lunt;

namespace Lake.Commands.Building
{
    public interface IBuildEngineInvoker
    {
        BuildManifest Build(BuildEngine engine, BuildEngineSettings settings);
    }
}
