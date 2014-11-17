namespace CommonDomain.Mediator
{
    public interface ICommandHandler<in TCommand>/* where TCommand : Command*/
    {
        void Handle(TCommand commandToHandle);
    }
}
