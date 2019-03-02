using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalChoiceJumpingOrFallingState : TribalHSMState
{
    public TribalChoiceJumpingOrFallingState(Tribal.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        Redirect();
    }

    private void Redirect()
    {
        switch(LastEnteringTrigger)
        {
            case Character.Order.Jump:

                SendEvent(Character.Order.Jump);

                break;

            default:

                SendEvent(Character.Order.Fall);

                break;
        }
    }


}