using UnityEngine;
using System;

public class SaveGameTrigger : BoxTrigger2D
{
    private Action<Collider2D> saveOnceDelegate;

    protected override void Awake()
    {
        base.Awake();

        saveOnceDelegate = SaveOnce;

        onStay += saveOnceDelegate;
    }

    private void SaveOnce(Collider2D collider)
    {
        SaveLoadManager.GetInstance().SaveGame();
        gameObject.SetActive(false);
        onStay -= saveOnceDelegate;
    }
}
