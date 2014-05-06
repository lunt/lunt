namespace Lake.Tests.Integration
{
    public static class IntegrationContextExtensions
    {
        public static IntegrationTestResult RunApplication(this IntegrationContext context, string[] args)
        {
            var application = IntegrationHelper.CreateApplication();
            var exitCode = application.Run(args);
            var manifest = IntegrationHelper.GetBuildManifest(context, args);
            return new IntegrationTestResult(exitCode, manifest);
        }

        public static IntegrationTestResult RunApplication(this IntegrationContext context, LakeOptions options)
        {
            var args = IntegrationHelper.CreateArgs(context, options);
            return RunApplication(context, args);
        }

        public static IntegrationTestResult ExecuteBuild(this IntegrationContext context, string configuration)
        {
            var options = IntegrationHelper.CreateOptions(context, configuration);
            return RunApplication(context, options);
        }
    }
}
