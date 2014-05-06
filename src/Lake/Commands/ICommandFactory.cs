namespace Lake.Commands
{
    public interface ICommandFactory
    {
        ICommand CreateHelpCommand(LakeOptions options);
        ICommand CreateVersionCommand(LakeOptions options);
        ICommand CreateBuildCommand(LakeOptions options);
    }
}