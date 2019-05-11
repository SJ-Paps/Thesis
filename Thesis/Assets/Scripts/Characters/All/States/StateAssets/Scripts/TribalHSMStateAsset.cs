using System;
using UnityEngine;

[Serializable]
public class TribalHSMTransition : SJHSMTransition<Tribal.State, Character.Order>
{
}

[CreateAssetMenu(menuName = "HSM/Character HSM State Assets/Tribal HSM State Asset")]
public class TribalHSMStateAsset : SJHSMStateAsset<Tribal.State, Character.Order, TribalHSMTransition>
{

}

