using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalChoiceJumpingOrFallingState : TribalHSMState
{
    public TribalChoiceJumpingOrFallingState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        Redirect();
    }

    private void Redirect()
    {
        switch(LastTrigger)
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