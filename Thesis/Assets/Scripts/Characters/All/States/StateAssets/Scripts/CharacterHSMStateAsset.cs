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

    public HSMTransition<Character.State, Character.Trigger> ToHSMTransition()
    {
        return new HSMTransition<Character.State, Character.Trigger>(stateFrom, trigger, stateTo);
    }
}

[CreateAssetMenu]
public class CharacterHSMStateAsset : HSMStateAsset<CharacterHSMStateAsset, CharacterHSMTransition, Character.State, Character.Trigger>
{

}
