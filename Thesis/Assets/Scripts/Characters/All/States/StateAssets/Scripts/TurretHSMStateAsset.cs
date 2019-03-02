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
    public HSMGuardConditionAsset[] guardConditions;

    public HSMTransition<Turret.State, Character.Order> ToHSMTransition()
    {
        HSMTransition<Turret.State, Character.Order> transition = new HSMTransition<Turret.State, Character.Order>(stateFrom, trigger, stateTo);

        for (int i = 0; i < guardConditions.Length; i++)
        {
            transition.AddGuardCondition(guardConditions[i].CreateConcreteGuardCondition());
        }

        return transition;
    }
}

[CreateAssetMenu(menuName = "HSM/Character HSM State Assets/Tribal HSM State Asset")]
public class TurretHSMStateAsset : SJHSMStateAsset<TurretHSMStateAsset, TurretHSMTransition, Turret.State, Character.Order, Turret, Turret.Blackboard>
{

}
