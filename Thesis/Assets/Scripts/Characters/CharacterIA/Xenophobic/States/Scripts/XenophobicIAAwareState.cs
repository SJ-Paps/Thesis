using SAM.Timers;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class XenophobicIAAwareState : XenophobicIAState
{
    private float previousHiddenDetectionDistance;
    private float previousHiddenDetectionProbability;
    private float previousSeekingTime;

    public XenophobicIAAwareState(XenophobicIAController.State state, string debugName) : base(state, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        previousHiddenDetectionDistance = Owner.CurrentHiddenDetectionDistance;
        previousHiddenDetectionProbability = Owner.CurrentHiddenDetectionProbability;
        previousSeekingTime = Owner.CurrentSeekingTime;

        Owner.CurrentHiddenDetectionDistance = Owner.Configuration.HiddenDetectionDistanceAware;
        Owner.CurrentHiddenDetectionProbability = Owner.Configuration.HiddenDetectionProbabilityAware;
        Owner.CurrentSeekingTime = Owner.Configuration.AwareTime;

        EyeCollection eyes = Owner.Slave.GetEyes();

        for (int i = 0; i < eyes.Count; i++)
        {
            Eyes current = eyes[i];

            //a partir de ahora estos ojos --SI-- pueden detectar la hidden layer
            int newLayerMask = Physics2D.GetLayerCollisionMask(current.Collider.gameObject.layer) | Reg.hiddenLayer;
            Physics2D.SetLayerCollisionMask(current.Collider.gameObject.layer, newLayerMask);
        }


    }

    protected override void OnExit()
    {
        base.OnExit();

        Owner.CurrentHiddenDetectionDistance = previousHiddenDetectionDistance;
        Owner.CurrentHiddenDetectionProbability = previousHiddenDetectionProbability;
        Owner.CurrentSeekingTime = previousSeekingTime;

        EyeCollection eyes = Owner.Slave.GetEyes();

        for (int i = 0; i < eyes.Count; i++)
        {
            Eyes current = eyes[i];

            //a partir de ahora estos ojos --NO-- pueden detectar la hidden layer
            int newLayerMask = Physics2D.GetLayerCollisionMask(current.Collider.gameObject.layer) ^ Reg.hiddenLayer;
            Physics2D.SetLayerCollisionMask(current.Collider.gameObject.layer, newLayerMask);
        }
    }
}
