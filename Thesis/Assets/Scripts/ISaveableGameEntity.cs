﻿using SJ.Management;

namespace SJ.GameEntities
{
    public interface ISaveableGameEntity : IGameEntity
    {
        GameplayObjectSave Save();

        void PostSaveCallback();

        void Load(GameplayObjectSave save);

        void PostLoadCallback(GameplayObjectSave save);
    }
}