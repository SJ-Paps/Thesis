using UnityEngine;

public enum DeadlyType
{
    Water,
    Sharp,
    Fall,
    Bullet
}

public class Deadly : MonoBehaviour {

    [SerializeField]
    private DeadlyType type;

    public DeadlyType Type { get { return type; } }
}
