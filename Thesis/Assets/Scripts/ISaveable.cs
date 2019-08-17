using System;

public interface ISaveable
{
    string InstanceGUID { get; }

    object Save();

    void PostSaveCallback();

    void Load(object data);

    void PostLoadCallback(object dataSave);

}