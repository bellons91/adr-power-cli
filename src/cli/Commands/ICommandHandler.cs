namespace Commands
{
    internal interface ICommandHandler<T> where T : IConsoleCommand
    {
        int Execute(T command);
    }
}
