public interface ICollectable<TOwner> : IOwnable<TOwner> where TOwner : class
{
    bool Collect(TOwner owner);

    bool Drop();
}
