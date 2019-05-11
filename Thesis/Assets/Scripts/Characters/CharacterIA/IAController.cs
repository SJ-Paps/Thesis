using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAController<TSlave> : UnityController<TSlave, Character.Order> where TSlave : Character
{

}
