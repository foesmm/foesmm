namespace foesmm
{
    public interface IExecutable
    {
        string File { get; }
        ExecutableType Type { get; }
    }
}