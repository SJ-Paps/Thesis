using System;
using UnityEngine;

public class Xenophobic : Tribal
{

    public override void GetEnslaved()
    {

    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override void SendOrder(Order order)
    {
        base.SendOrder(order);
    }

    public override string ToString()
    {
        return base.ToString();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override void OnDetected()
    {
        base.OnDetected();
    }

    protected override void OnFacingChanged(bool facingLeft)
    {
        base.OnFacingChanged(facingLeft);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
