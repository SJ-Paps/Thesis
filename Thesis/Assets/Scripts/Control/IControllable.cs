using System;

public interface IControllable<TOrder>
{
    event Action<TOrder> OnOrderReceived;

    bool Enslaved
    {
        get;
    }

    void GetEnslaved();

    void SendOrder(TOrder order);
}
