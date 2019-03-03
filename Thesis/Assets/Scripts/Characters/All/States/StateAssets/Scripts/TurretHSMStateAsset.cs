using System;
using UnityEngine;

[Serializable]
public struct TurretHSMTransition : IHSMTransitionSerializationWrapper<Turret.State, Character.Order>
{
    [SerializeField]
    public Turret.State stateFrom;
    [SerializeField]
    public Character.Order trigger;
    [SerializeField]
    public Turret.State stateTo;

    [SerializeField]
    public HSMGuardConditionAsset[] ANDGuardConditions, ORGuardConditions;

    public HSMTransition<Turret.State, Character.Order> ToHSMTransition()
    {
        HSMTransition<Turret.State, Character.Order> transition = new HSMTransition<Turret.State, Character.Order>(stateFrom, trigger, stateTo);

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

[CreateAssetMenu(menuName = "HSM/Character HSM State Assets/Turret HSM State Asset")]
public class TurretHSMStateAsset : SJHSMStateAsset<TurretHSMStateAsset, TurretHSMTransition, Turret.State, Character.Order, Turret, Turret.Blackboard>
{

}
