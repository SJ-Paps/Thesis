namespace SJ.GameEntities
{
    public class GameplayObjectSave
    {
        public string EntityGuid;
        public string PrefabName;

        public object Save;

        public GameplayObjectSave(string instanceGuid, string prefabName, object save)
        {
            EntityGuid = instanceGuid;
            PrefabName = prefabName;
            Save = save;
        }

        public GameplayObjectSave()
        {

        }
    }
}