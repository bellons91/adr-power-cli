namespace Commands
{
    internal interface ICommandHandler<T> where T : IConsoleCommand
    {
        Task<int> Execute(T command);
    }
}
