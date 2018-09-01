using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xenophobic : Tribal, IAudibleListener
{
    [SerializeField]
    protected Gun gun;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void GetEnslaved()
    {

    }

    public void Listen(ref AudibleData data)
    {
        
    }

    public override void SetOrder(Order order)
    {
        
    }
}
