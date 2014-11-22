namespace CommonDomain.Messaging
{
    public interface IQueryHandler<in TRequest, out TResponse>
    {
        TResponse Handle(TRequest queryToHandle);
    }
}
