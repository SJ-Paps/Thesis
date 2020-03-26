namespace SJ.Management
{
    public interface IUpdateListener
    {
        void DoUpdate();
    }

    public interface ILateUpdateListener
    {
        void DoLateUpdate();
    }

    public interface IFixedUpdateListener
    {
        void DoFixedUpdate();
    }
}
