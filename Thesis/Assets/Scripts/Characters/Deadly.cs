﻿using UnityEngine;

public enum DamageType : byte
{
    Water,
    Sharp,
    Fall,
    Bullet
}

public class Deadly : MonoBehaviour {

    [SerializeField]
    private DamageType type;

    public DamageType Type { get { return type; } }
}
