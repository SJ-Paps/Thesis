using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalThrowingState : TribalHSMState
{
    protected override void OnEnter()
    {
        base.OnEnter();

        if(Owner.Equipment.HasEquippedObjectOfType<IThrowable>(out IThrowable throwable))
        {
            throwable.Throw();
        }

        SendEvent(Character.Order.FinishAction);
    }
}
