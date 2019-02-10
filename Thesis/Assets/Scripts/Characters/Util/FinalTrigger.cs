using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTrigger : SJBoxCollider2D {
    
    protected override void OnAwake()
    {
        base.OnAwake();

        onStayTrigger += Finish;
    }

    private void Finish(Collider2D collider)
    {
        GameManager.Instance.GoMenu();

        gameObject.SetActive(false);
    }

}
