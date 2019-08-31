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

    protected override void OnDetected()
    {
        base.OnDetected();
    }

    protected override void OnFacingChanged(bool facingLeft)
    {
        base.OnFacingChanged(facingLeft);
    }
}
