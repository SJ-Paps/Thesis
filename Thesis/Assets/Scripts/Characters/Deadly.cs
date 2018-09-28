using UnityEngine;

public enum DeadlyType
{
    Water,
    Sharp,
    Fall
}

public class Deadly : MonoBehaviour {

    [SerializeField]
    private DeadlyType type;

    public DeadlyType Type { get { return type; } }
}
