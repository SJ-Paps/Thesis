using System;
using UnityEngine;

[Serializable]
public struct CharacterHSMTransition : IHSMTransitionSerializationWrapper<Character.State, Character.Trigger>
{
    [SerializeField]
    public Character.State stateFrom;
    [SerializeField]
    public Character.Trigger trigger;
    [SerializeField]
    public Character.State stateTo;

    [SerializeField]
    public HSMGuardConditionAsset[] guardConditions;

    public HSMTransition<Character.State, Character.Trigger> ToHSMTransition()
    {
        HSMTransition<Character.State, Character.Trigger> transition = new HSMTransition<Character.State, Character.Trigger>(stateFrom, trigger, stateTo);

        for(int i = 0; i < guardConditions.Length; i++)
        {
            transition.AddGuardCondition(guardConditions[i].CreateConcreteGuardCondition());
        }

        return transition;
    }
}

[CreateAssetMenu]
public class CharacterHSMStateAsset : HSMStateAsset<CharacterHSMStateAsset, CharacterHSMTransition, Character.State, Character.Trigger>
{

}
