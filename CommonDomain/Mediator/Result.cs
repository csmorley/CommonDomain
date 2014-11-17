namespace CommonDomain.Mediator
{
    using System;

    public sealed class UnitType
    {
        public static readonly UnitType Default = new UnitType();
        UnitType() { }
    }

    public sealed class Result : Result<UnitType>
    {
        public override UnitType Data
        {
            get { return UnitType.Default; }
            set { }
        }
    }

    public class Result<TResponseData>
    {
        public virtual TResponseData Data { get; set; }

        public virtual Exception Exception { get; set; }

        public virtual bool HasException()
        {
            return Exception != null;
        }
    }
}
