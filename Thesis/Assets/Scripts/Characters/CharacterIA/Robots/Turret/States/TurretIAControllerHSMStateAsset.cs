using System;
using UnityEngine;

[Serializable]
public class TurretIAControllerHSMTransition : SJHSMTransition<TurretIAController.State, TurretIAController.Trigger>
{

}

[CreateAssetMenu(menuName = "HSM/IA Controller HSM State Assets/Turret IA State Asset")]
public class TurretIAControllerHSMStateAsset : SJHSMStateAsset<TurretIAController.State, TurretIAController.Trigger, TurretIAControllerHSMTransition>
{

}
