using System;
using UnityEngine;

[Serializable]
public class XenophobicIAControllerHSMTransition : SJHSMTransition
{
    [SerializeField]
    public XenophobicIAController.State stateFrom;
    [SerializeField]
    public XenophobicIAController.Trigger trigger;
    [SerializeField]
    public XenophobicIAController.State stateTo;

    protected override HSMTransition<byte, byte> CreateConcreteTransition()
    {
        return new HSMTransition<byte, byte>((byte)stateFrom, (byte)trigger, (byte)stateTo);
    }
}

[CreateAssetMenu(menuName = "HSM/IA Controller HSM State Assets/Xenophobic IA State Asset")]
public class XenophobicIAControllerHSMStateAsset : SJHSMStateAsset
{
    [SerializeField]
    private XenophobicIAController.State state;

    [SerializeField]
    private XenophobicIAControllerHSMTransition[] transitions;

    protected override SJHSMTransition[] GetSJHSMTranstions()
    {
        return transitions;
    }

    protected override byte GetStateId()
    {
        return (byte)state;
    }
}
