using SAM.FSM;
using System;
using UnityEngine;

public class XenophobicAlertlessState : XenophobicIAState {

    private Action<Vector2> onSomethingDetectedDelegate;

	public XenophobicAlertlessState(FSM<XenophobicIAController.State, XenophobicIAController.Trigger> fsm, XenophobicIAController.State state, Xenophobic controller, XenophobicIAController.Blackboard blackboard) : base(fsm, state, controller, blackboard)
    {
        onSomethingDetectedDelegate = GetAware;
    }

    protected override void OnEnter()
    {
        character.onSomethingDetected += onSomethingDetectedDelegate;
    }

    protected override void OnUpdate()
    {

    }

    protected override void OnExit()
    {
        character.onSomethingDetected -= onSomethingDetectedDelegate;
    }

    private void GetAware(Vector2 position)
    {
        blackboard.seekedLastPosition = position;
        stateMachine.Trigger(XenophobicIAController.Trigger.GetAware);
    }
}
