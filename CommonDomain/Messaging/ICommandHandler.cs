namespace CommonDomain.Messaging
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        void Handle(TCommand commandToHandle);
    }
}
