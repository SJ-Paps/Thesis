using UnityEngine;
using System;
using SJ.Game;

public class SaveGameTrigger : SJBoxCollider2D
{
    private Action<Collider2D> saveOnceDelegate;

    protected override void SJAwake()
    {
        base.SJAwake();

        saveOnceDelegate = SaveOnce;

        onStayTrigger += saveOnceDelegate;
    }

    private void SaveOnce(Collider2D collider)
    {
        SJ.Application.GameManager.Save();
        gameObject.SetActive(false);
        onStayTrigger -= saveOnceDelegate;
    }
}
