using System;
using UnityEngine;

[Serializable]
public class TribalHSMTransition : SJHSMTransition
{
    [SerializeField]
    public Tribal.State stateFrom;
    [SerializeField]
    public Character.Order trigger;
    [SerializeField]
    public Tribal.State stateTo;

    protected override HSMTransition<byte, byte> CreateConcreteTransition()
    {
        return new HSMTransition<byte, byte>((byte)stateFrom, (byte)trigger, (byte)stateTo);
    }
}

[CreateAssetMenu(menuName = "HSM/Character HSM State Assets/Tribal HSM State Asset")]
public class TribalHSMStateAsset : CharacterHSMStateAsset
{
    [SerializeField]
    private Tribal.State state;

    [SerializeField]
    private TribalHSMTransition[] transitions;

    protected override SJHSMTransition[] GetSJHSMTranstions()
    {
        return transitions;
    }

    protected override byte GetStateId()
    {
        return (byte)state;
    }
}

