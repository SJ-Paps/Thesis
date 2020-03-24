using System;

public interface IControllable<TOrder>
{
    event Action<TOrder> OnOrderReceived;

    void SendOrder(TOrder order);
}
