using System;
using UnityEngine;

[Serializable]
public struct TribalHSMTransition : IHSMTransitionSerializationWrapper<Tribal.State, Character.Order>
{
    [SerializeField]
    public Tribal.State stateFrom;
    [SerializeField]
    public Character.Order trigger;
    [SerializeField]
    public Tribal.State stateTo;

    [SerializeField]
    public HSMGuardConditionAsset[] guardConditions;

    public HSMTransition<Tribal.State, Character.Order> ToHSMTransition()
    {
        HSMTransition<Tribal.State, Character.Order> transition = new HSMTransition<Tribal.State, Character.Order>(stateFrom, trigger, stateTo);

        for(int i = 0; i < guardConditions.Length; i++)
        {
            transition.AddGuardCondition(guardConditions[i].CreateConcreteGuardCondition());
        }

        return transition;
    }
}

[CreateAssetMenu(menuName = "HSM/Character HSM State Assets/Tribal HSM State Asset")]
public class TribalHSMStateAsset : SJHSMStateAsset<TribalHSMStateAsset, TribalHSMTransition, Tribal.State, Character.Order, Tribal, Tribal.Blackboard>
{
    
}
