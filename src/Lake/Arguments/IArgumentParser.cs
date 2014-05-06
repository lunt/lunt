namespace Lake.Arguments
{
    public interface IArgumentParser
    {
        LakeOptions Parse(string[] args);
    }
}