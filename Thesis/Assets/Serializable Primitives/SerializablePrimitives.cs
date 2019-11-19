using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct SerializableInt
{
    [SerializeField]
    public string name;
    [SerializeField]
    public int value;
}

[Serializable]
public struct SerializableFloat
{
    [SerializeField]
    public string name;
    [SerializeField]
    public float value;
}

[Serializable]
public struct SerializableString
{
    [SerializeField]
    public string name;
    [SerializeField]
    public string value;
}

[Serializable]
public struct SerializableBool
{
    [SerializeField]
    public string name;
    [SerializeField]
    public bool value;
}

[Serializable]
public struct SerializableObject
{
    [SerializeField]
    public string name;
    [SerializeField]
    public UnityEngine.Object value;
}