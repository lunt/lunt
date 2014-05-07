namespace Lake.Commands
{
    public interface ICommand
    {
        int Execute(LakeOptions options);
    }
}