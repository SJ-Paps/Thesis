using System;
using UnityEngine;

[Serializable]
public struct TribalHSMTransition : IHSMTransitionSerializationWrapper<Tribal.State, Character.Trigger>
{
    [SerializeField]
    public Tribal.State stateFrom;
    [SerializeField]
    public Character.Trigger trigger;
    [SerializeField]
    public Tribal.State stateTo;

    [SerializeField]
    public HSMGuardConditionAsset[] guardConditions;

    public HSMTransition<Tribal.State, Character.Trigger> ToHSMTransition()
    {
        HSMTransition<Tribal.State, Character.Trigger> transition = new HSMTransition<Tribal.State, Character.Trigger>(stateFrom, trigger, stateTo);

        for(int i = 0; i < guardConditions.Length; i++)
        {
            transition.AddGuardCondition(guardConditions[i].CreateConcreteGuardCondition());
        }

        return transition;
    }
}

[CreateAssetMenu(menuName = "HSM/Character HSM State Assets/Tribal HSM State Asset")]
public class TribalHSMStateAsset : SJHSMStateAsset<TribalHSMStateAsset, TribalHSMTransition, Tribal.State, Character.Trigger, Tribal, Tribal.Blackboard>
{
    
}
