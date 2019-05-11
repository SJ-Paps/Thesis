using System;

public interface IControllable<TOrder> where TOrder : struct
{
    event Action<TOrder> onOrderReceived;

    bool Enslaved
    {
        get;
    }

    void GetEnslaved();

    void SendOrder(TOrder order);
}
