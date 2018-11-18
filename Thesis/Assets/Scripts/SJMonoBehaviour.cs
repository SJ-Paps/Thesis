using UnityEngine;
using UnityEditor;

public abstract class SJMonoBehaviour : MonoBehaviour
{
    [SerializeField]
    public string prefabName;

    protected virtual void Awake()
    {

    }

#if UNITY_EDITOR

    private void OnValidate()
    {

    }

#endif
}
