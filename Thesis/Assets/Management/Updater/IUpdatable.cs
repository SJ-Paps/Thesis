namespace SJ.Updatables
{
    public interface IUpdatable
    {
        void DoUpdate();
        void DoLateUpdate();
        void DoFixedUpdate();
    }
}


