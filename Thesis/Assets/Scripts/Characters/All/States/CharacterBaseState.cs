using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBaseState : CharacterHSMState
{
    public CharacterBaseState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }
}