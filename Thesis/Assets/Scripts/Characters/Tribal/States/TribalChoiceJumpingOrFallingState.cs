using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalChoiceJumpingOrFallingState : TribalHSMState
{
    public TribalChoiceJumpingOrFallingState(Character.State stateId, string debugName = null) : base(stateId, debugName)
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
            case Character.Trigger.Jump:

                SendEvent(Character.Trigger.Jump);

                break;

            default:

                SendEvent(Character.Trigger.Fall);

                break;
        }
    }


}