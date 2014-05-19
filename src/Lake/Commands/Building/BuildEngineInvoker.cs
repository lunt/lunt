using Lunt;

namespace Lake.Commands.Building
{
    internal sealed class BuildEngineInvoker : IBuildEngineInvoker
    {
        public BuildManifest Build(BuildEngine engine, BuildEngineSettings settings)
        {
            return engine.Build(settings);
        }
    }
}