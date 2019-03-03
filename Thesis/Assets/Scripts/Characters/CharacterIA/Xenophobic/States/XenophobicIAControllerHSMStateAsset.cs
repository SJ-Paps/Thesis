using System;
using UnityEngine;

[Serializable]
public struct XenophobicIAControllerHSMTransition : IHSMTransitionSerializationWrapper<XenophobicIAController.State, XenophobicIAController.Trigger>
{
    [SerializeField]
    public XenophobicIAController.State stateFrom;
    [SerializeField]
    public XenophobicIAController.Trigger trigger;
    [SerializeField]
    public XenophobicIAController.State stateTo;

    [SerializeField]
    public HSMGuardConditionAsset[] ANDGuardConditions, ORGuardConditions;

    public HSMTransition<XenophobicIAController.State, XenophobicIAController.Trigger> ToHSMTransition()
    {
        HSMTransition<XenophobicIAController.State, XenophobicIAController.Trigger> transition = new HSMTransition<XenophobicIAController.State, XenophobicIAController.Trigger>(stateFrom, trigger, stateTo);

        for (int i = 0; i < ANDGuardConditions.Length; i++)
        {
            transition.AddANDGuardCondition(ANDGuardConditions[i].CreateConcreteGuardCondition());
        }

        for (int i = 0; i < ORGuardConditions.Length; i++)
        {
            transition.AddORGuardCondition(ORGuardConditions[i].CreateConcreteGuardCondition());
        }

        return transition;
    }
}

[CreateAssetMenu(menuName = "HSM/IA Controller HSM State Assets/Xenophobic IA State Asset")]
public class XenophobicIAControllerHSMStateAsset : SJHSMStateAsset<XenophobicIAControllerHSMStateAsset, XenophobicIAControllerHSMTransition, XenophobicIAController.State, XenophobicIAController.Trigger, XenophobicIAController, XenophobicIAController.Blackboard>
{

}
