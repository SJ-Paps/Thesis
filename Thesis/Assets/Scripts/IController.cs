public delegate void ChangeControlDelegate<TControllable, TOrder>(TControllable previousControllable, TControllable newControllable) where TControllable : IControllable<TOrder>;

public interface IController<TControllable, TOrder> where TControllable : IControllable<TOrder>
{
    event ChangeControlDelegate<TControllable, TOrder> OnControllableChanged;
    void SetControllable(TControllable controllable);
}
