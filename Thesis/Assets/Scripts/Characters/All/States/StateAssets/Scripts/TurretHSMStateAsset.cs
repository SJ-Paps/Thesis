using System;
using UnityEngine;

[Serializable]
public class TurretHSMTransition : SJHSMTransition
{
    [SerializeField]
    public Turret.State stateFrom;
    [SerializeField]
    public Character.Order trigger;
    [SerializeField]
    public Turret.State stateTo;

    protected override HSMTransition<byte, byte> CreateConcreteTransition()
    {
        return new HSMTransition<byte, byte>((byte)stateFrom, (byte)trigger, (byte)stateTo);
    }
}

[CreateAssetMenu(menuName = "HSM/Character HSM State Assets/Turret HSM State Asset")]
public class TurretHSMStateAsset : CharacterHSMStateAsset
{
    [SerializeField]
    private Turret.State state;

    [SerializeField]
    private TurretHSMTransition[] transitions;

    protected override SJHSMTransition[] GetSJHSMTranstions()
    {
        return transitions;
    }

    protected override byte GetStateId()
    {
        return (byte)state;
    }
}
