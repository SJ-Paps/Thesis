using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu]
public class HSMGuardConditionAsset : ScriptableObject
{
    [SerializeField]
    protected TextAsset script;

    [SerializeField]
    [ReadOnly]
    protected string stateClassFullName;

    [SerializeField]
    protected bool invert;

    public GuardCondition CreateConcreteGuardCondition()
    {
        GuardCondition guardCondition = (GuardCondition)Activator.CreateInstance(Type.GetType(stateClassFullName));

        guardCondition.invert = invert;

        return guardCondition;
    }

    void OnValidate()
    {
#if UNITY_EDITOR
        if (script != null)
        {
            MonoScript monoscript = (MonoScript)script;

            if (monoscript == null)
            {
                throw new InvalidOperationException("Text asset is not a valid Monoscript");
            }

            Type classType = monoscript.GetClass();

            if (classType == null)
            {
                throw new NullReferenceException("The name of the class on the script " + script.name.ToUpper() + " doesn't match the script's name");
            }

            stateClassFullName = classType.FullName;
        }
#endif
    }
}
