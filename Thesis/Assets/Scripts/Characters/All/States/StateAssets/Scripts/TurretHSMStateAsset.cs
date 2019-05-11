using System;
using UnityEngine;

[Serializable]
public class TurretHSMTransition : SJHSMTransition<Turret.State, Character.Order>
{

}

[CreateAssetMenu(menuName = "HSM/Character HSM State Assets/Turret HSM State Asset")]
public class TurretHSMStateAsset : SJHSMStateAsset<Turret.State, Character.Order, TurretHSMTransition>
{

}
