using System;
using UnityEngine;

[Serializable]
public struct TurretIAControllerHSMTransition : IHSMTransitionSerializationWrapper<TurretIAController.State, TurretIAController.Trigger>
{
    [SerializeField]
    public TurretIAController.State stateFrom;
    [SerializeField]
    public TurretIAController.Trigger trigger;
    [SerializeField]
    public TurretIAController.State stateTo;

    [SerializeField]
    public HSMGuardConditionAsset[] ANDGuardConditions, ORGuardConditions;

    public HSMTransition<TurretIAController.State, TurretIAController.Trigger> ToHSMTransition()
    {
        HSMTransition<TurretIAController.State, TurretIAController.Trigger> transition = new HSMTransition<TurretIAController.State, TurretIAController.Trigger>(stateFrom, trigger, stateTo);

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

[CreateAssetMenu(menuName = "HSM/IA Controller HSM State Assets/Turret IA State Asset")]
public class TurretIAControllerHSMStateAsset : SJHSMStateAsset<TurretIAControllerHSMStateAsset, TurretIAControllerHSMTransition, TurretIAController.State, TurretIAController.Trigger, TurretIAController, TurretIAController.Blackboard>
{

}
