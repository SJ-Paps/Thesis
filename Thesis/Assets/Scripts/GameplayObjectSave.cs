namespace SJ.Save
{
    public class GameplayObjectSave
    {
        public string instanceGuid;
        public string prefabName;

        public object save;

        public GameplayObjectSave(string instanceGuid, string prefabName, object save)
        {
            this.instanceGuid = instanceGuid;
            this.prefabName = prefabName;
            this.save = save;
        }

        public GameplayObjectSave()
        {

        }
    }
}