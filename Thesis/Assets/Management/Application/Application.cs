using UniRx;

namespace SJ.Management
{
    public class Application
    {
        private static Application instance;

        public static Application Instance
        {
            get
            {
                if (instance == null)
                    instance = new Application();

                return instance;
            }
        }

        private Application() { }

        public bool IsInitialized { get; private set; }

        public void Initialize(LoadAction[] loadActions) => ExecuteLoadActions(loadActions);

        private void ExecuteLoadActions(LoadAction[] loadActions)
        {
            Observable.Zip(loadActions)
                .ObserveOnMainThread()
                .Subscribe();
        }
    }
}