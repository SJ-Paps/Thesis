using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTrigger : BoxTrigger2D {

    protected override void Awake()
    {
        base.Awake();

        onStay += Finish;
    }

    private void Finish(Collider2D collider)
    {
        GameManager.Instance.GoMenu();

        gameObject.SetActive(false);
    }

}
