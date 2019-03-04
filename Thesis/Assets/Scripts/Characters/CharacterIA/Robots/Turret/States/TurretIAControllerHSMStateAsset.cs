using System;
using UnityEngine;

[Serializable]
public class TurretIAControllerHSMTransition : SJHSMTransition
{
    [SerializeField]
    public TurretIAController.State stateFrom;
    [SerializeField]
    public TurretIAController.Trigger trigger;
    [SerializeField]
    public TurretIAController.State stateTo;

    protected override HSMTransition<byte, byte> CreateConcreteTransition()
    {
        return new HSMTransition<byte, byte>((byte)stateFrom, (byte)trigger, (byte)stateTo);
    }
}

[CreateAssetMenu(menuName = "HSM/IA Controller HSM State Assets/Turret IA State Asset")]
public class TurretIAControllerHSMStateAsset : SJHSMStateAsset
{
    [SerializeField]
    private TurretIAController.State state;

    [SerializeField]
    private TurretIAControllerHSMTransition[] transitions;

    protected override SJHSMTransition[] GetSJHSMTranstions()
    {
        return transitions;
    }

    protected override byte GetStateId()
    {
        return (byte)state;
    }
}
