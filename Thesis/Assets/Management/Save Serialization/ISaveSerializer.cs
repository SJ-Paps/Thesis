namespace SJ.Management
{
    public interface ISaveSerializer
    {
        string Serialize(params SaveData[] saves);
        SaveData[] Deserialize(string serializedSaves);
    }
}

