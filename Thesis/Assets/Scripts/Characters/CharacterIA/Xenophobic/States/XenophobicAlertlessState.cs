using SAM.FSM;
using System;
using UnityEngine;
using System.Collections.ObjectModel;

public class XenophobicAlertlessState : XenophobicIAState {

    private Vector2 eyesSize = new Vector2(6, 1);

    private Action<Collider2D> onSomethingDetectedDelegate;
    private Eyes characterEyes;

    private int[] shouldCollideLayers;

	public XenophobicAlertlessState(FSM<XenophobicIAController.AlertState, XenophobicIAController.AlertTrigger> fsm, XenophobicIAController.AlertState state, XenophobicIAController controller, XenophobicIAController.Blackboard blackboard) : base(fsm, state, controller, blackboard)
    {
        onSomethingDetectedDelegate += GetAware;

        shouldCollideLayers = new int[3];

        shouldCollideLayers[0] = Reg.floorLayer;
        shouldCollideLayers[1] = Reg.playerLayer;
        shouldCollideLayers[2] = Reg.objectLayer;

        characterEyes = controller.SlaveEyes;
    }

    protected override void OnEnter()
    {
        if(characterEyes != null)
        {
            characterEyes.Trigger2D.ChangeSize(eyesSize);

            characterEyes.Trigger2D.onStay += onSomethingDetectedDelegate;
        }
    }

    protected override void OnUpdate()
    {

    }

    protected override void OnExit()
    {
        if (characterEyes != null)
        {
            characterEyes.Trigger2D.onStay -= onSomethingDetectedDelegate;
        }
    }

    private void GetAware(Collider2D collider)
    {
        if(characterEyes.IsVisible(collider, shouldCollideLayers))
        {
            stateMachine.Trigger(XenophobicIAController.AlertTrigger.GetAware);
        }
    }
}
