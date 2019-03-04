using UnityEngine;

public class TribalChoiceHangingLadderOrRopeOrLedge : TribalHSMState
{
    public TribalChoiceHangingLadderOrRopeOrLedge(byte stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        Redirect();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        Redirect();
    }

    private void Redirect()
    {
        Character.Order order;

        Debug.Log("EL LAS ENTERIN TIGUER " + LastEnteringTrigger);

        if(LastEnteringTrigger == Character.Order.HangLedge)
        {
            order = Character.Order.HangLedge;
        }
        else
        {
            if(Blackboard.activable is Ladder)
            {
                order = Character.Order.HangLadder;
            }
            else
            {
                order = Character.Order.HangRope;
            }
        }

        
        if(SendEvent(order) == false)
        {
            SendEvent(Character.Order.FinishAction);
        }
    }
}
