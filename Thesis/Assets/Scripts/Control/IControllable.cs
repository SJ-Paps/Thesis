public interface IControllable
{
    bool Enslaved
    {
        get;
    }

    void GetEnslaved();
}

public interface IControllable<TOrder> : IControllable where TOrder : struct
{
    void SetOrder(TOrder order);
}
