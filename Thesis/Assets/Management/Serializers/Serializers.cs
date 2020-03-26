namespace SJ.Management
{
    public static class Serializers
    {
        private static ISaveSerializer saveSerializer;

        public static ISaveSerializer GetSaveSerializer()
        {
            if(saveSerializer == null)
                saveSerializer = new JsonSaveSerializer();

            return saveSerializer;
        }
    }
}
