public interface IControllable<TOrder> where TOrder : struct
{
    bool Enslaved
    {
        get;
    }

    void GetEnslaved();

    void SetOrder(TOrder order);
}
