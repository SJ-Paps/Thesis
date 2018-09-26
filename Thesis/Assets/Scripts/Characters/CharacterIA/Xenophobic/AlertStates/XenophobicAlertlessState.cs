using SAM.FSM;
using System;
using UnityEngine;

public class XenophobicAlertlessState : XenophobicIAState {

    private Vector2 eyesSize = new Vector2(9, 1);

    private Action<Collider2D> onSomethingDetectedDelegate;
    private Eyes characterEyes;

    private int visionLayers = (1 << Reg.floorLayer) | (1 << Reg.playerLayer) | (1 << Reg.objectLayer);


    public XenophobicAlertlessState(FSM<XenophobicIAController.State, XenophobicIAController.Trigger> fsm, XenophobicIAController.State state, XenophobicIAController controller, XenophobicIAController.Blackboard blackboard) : base(fsm, state, controller, blackboard)
    {
        onSomethingDetectedDelegate += AnalyzeDetection;

        characterEyes = controller.SlaveEyes;
    }

    protected override void OnEnter()
    {
        if(characterEyes != null)
        {
            characterEyes.Trigger2D.ChangeSize(eyesSize);

            characterEyes.Trigger2D.onEntered += onSomethingDetectedDelegate;
        }
    }

    protected override void OnUpdate()
    {

    }

    protected override void OnExit()
    {
        if (characterEyes != null)
        {
            characterEyes.Trigger2D.onEntered -= onSomethingDetectedDelegate;
        }
    }

    private void AnalyzeDetection(Collider2D collider)
    {
        if(characterEyes.IsVisible(collider, visionLayers))
        {
            if(collider.gameObject.layer == Reg.playerLayer && GameManager.Instance.GetPlayer().IsHidden == false)
            {
                stateMachine.Trigger(XenophobicIAController.Trigger.GetAware);
            }
        }
    }
}
