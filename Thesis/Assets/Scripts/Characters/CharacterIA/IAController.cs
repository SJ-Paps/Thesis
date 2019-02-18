using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAController<TSlave> : UnityController<TSlave, Character.Trigger> where TSlave : Character
{

}
