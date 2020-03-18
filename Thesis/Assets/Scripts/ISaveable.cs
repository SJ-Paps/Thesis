using SJ.Management;

namespace SJ.Save
{
    public interface ISaveable : IGameplayEntity
    {
        GameplayObjectSave Save();

        void PostSaveCallback();

        void Load(GameplayObjectSave save);

        void PostLoadCallback(GameplayObjectSave save);
    }
}