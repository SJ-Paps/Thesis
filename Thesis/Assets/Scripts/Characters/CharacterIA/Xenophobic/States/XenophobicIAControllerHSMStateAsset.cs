using System;
using UnityEngine;

[Serializable]
public class XenophobicIAControllerHSMTransition : SJHSMTransition<XenophobicIAController.State, XenophobicIAController.Trigger>
{

}

[CreateAssetMenu(menuName = "HSM/IA Controller HSM State Assets/Xenophobic IA State Asset")]
public class XenophobicIAControllerHSMStateAsset : SJHSMStateAsset<XenophobicIAController.State, XenophobicIAController.Trigger, XenophobicIAControllerHSMTransition>
{

}
