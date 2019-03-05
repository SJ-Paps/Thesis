using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif


public abstract class HSMStateAsset<TConcreteAssetClass, TState, TTrigger> : ScriptableObject
                                                                        where TConcreteAssetClass : HSMStateAsset<TConcreteAssetClass, TState, TTrigger>
                                                                        where TState : unmanaged
                                                                        where TTrigger : unmanaged
{
    [SerializeField]
    protected TextAsset script;

    [SerializeField]
    [ReadOnly]
    protected string stateClassFullName;

    [SerializeField]
    public string debugName;

    [SerializeField]
    protected bool copy;

    [SerializeField]
    public TState stateId;

    [SerializeField]
    public int startStateIndex;

    [SerializeField]
    public TConcreteAssetClass[] childs;

    [SerializeField]
    public TConcreteAssetClass[] parallelChilds;
    

    public static HSMState<TState, TTrigger> BuildFromAsset(HSMStateAsset<TConcreteAssetClass, TState, TTrigger> rootAsset)
    {
        var relationDictionary = new Dictionary<HSMStateAsset<TConcreteAssetClass, TState, TTrigger>, HSMState<TState, TTrigger>>();

        GetUniqueStates(relationDictionary, rootAsset);

        HSMState<TState, TTrigger> rootState = BuildHierarchy(relationDictionary, rootAsset);

        ReplaceCopiables(rootState, rootAsset);

        BuildTransitions(relationDictionary, rootAsset);

        SetInitialStates(relationDictionary, rootAsset);

        return rootState;
    }

    private static void GetUniqueStates(Dictionary<HSMStateAsset<TConcreteAssetClass, TState, TTrigger>, HSMState<TState, TTrigger>> relationDictionary, HSMStateAsset<TConcreteAssetClass, TState, TTrigger> baseAsset)
    {
        if(relationDictionary.ContainsKey(baseAsset) == false)
        {
            relationDictionary.Add(baseAsset, baseAsset.CreateConcreteHSMState());
        }

        for(int i = 0; i < baseAsset.parallelChilds.Length; i++)
        {
            GetUniqueStates(relationDictionary, baseAsset.parallelChilds[i]);
        }

        for(int i = 0; i < baseAsset.childs.Length; i++)
        {
            GetUniqueStates(relationDictionary, baseAsset.childs[i]);
        }
    }

    private static HSMState<TState, TTrigger> BuildHierarchy(Dictionary<HSMStateAsset<TConcreteAssetClass, TState, TTrigger>, HSMState<TState, TTrigger>> relationDictionary, HSMStateAsset<TConcreteAssetClass, TState, TTrigger> baseAsset)
    {
        HSMState<TState, TTrigger> currentState = relationDictionary[baseAsset];

        for(int i = 0; i < baseAsset.parallelChilds.Length; i++)
        {
            HSMStateAsset<TConcreteAssetClass, TState, TTrigger> currentAssetChild = baseAsset.parallelChilds[i];

            currentState.AddParallelChild(relationDictionary[currentAssetChild]);
            BuildHierarchy(relationDictionary, currentAssetChild);
        }

        for(int i = 0; i < baseAsset.childs.Length; i++)
        {
            HSMStateAsset<TConcreteAssetClass, TState, TTrigger> currentAssetChild = baseAsset.childs[i];

            currentState.AddChild(relationDictionary[currentAssetChild]);
            BuildHierarchy(relationDictionary, currentAssetChild);
        }

        return currentState;
    }

    private static void BuildTransitions(Dictionary<HSMStateAsset<TConcreteAssetClass, TState, TTrigger>, HSMState<TState, TTrigger>> relationDictionary, HSMStateAsset<TConcreteAssetClass, TState, TTrigger> baseAsset)
    {
        HSMState<TState, TTrigger> currentState = relationDictionary[baseAsset];
        
        HSMTransition<TState, TTrigger>[] transitions = baseAsset.GetTransitions();

        for (int i = 0; i < transitions.Length; i++)
        {
            HSMTransition<TState, TTrigger> transition = transitions[i];

            currentState.MakeChildTransition(transition);
        }

        for (int i = 0; i < baseAsset.parallelChilds.Length; i++)
        {
            HSMStateAsset<TConcreteAssetClass, TState, TTrigger> currentAssetChild = baseAsset.parallelChilds[i];

            BuildTransitions(relationDictionary, currentAssetChild);
        }

        for (int i = 0; i < baseAsset.childs.Length; i++)
        {
            HSMStateAsset<TConcreteAssetClass, TState, TTrigger> currentAssetChild = baseAsset.childs[i];

            BuildTransitions(relationDictionary, currentAssetChild);
        }
    }

    private static void SetInitialStates(Dictionary<HSMStateAsset<TConcreteAssetClass, TState, TTrigger>, HSMState<TState, TTrigger>> relationDictionary, HSMStateAsset<TConcreteAssetClass, TState, TTrigger> baseAsset)
    {
        HSMState<TState, TTrigger> currentState = relationDictionary[baseAsset];

        if (baseAsset.childs.Length > 0)
        {
            currentState.SetInitialState(baseAsset.childs[baseAsset.startStateIndex].stateId);

            for (int i = 0; i < baseAsset.childs.Length; i++)
            {
                SetInitialStates(relationDictionary, baseAsset.childs[i]);
            }
        }
    }

    private static void ReplaceCopiables(HSMState<TState, TTrigger> baseState, HSMStateAsset<TConcreteAssetClass, TState, TTrigger> baseAsset)
    { 
        for(int i = 0; i < baseAsset.parallelChilds.Length; i++)
        {
            var currentAsset = baseAsset.parallelChilds[i];
            var currentState = baseState.GetImmediateChildState(currentAsset.stateId);

            ReplaceCopiables(currentState, currentAsset);

            if (currentAsset.copy)
            {
                baseState.ReplaceChild(currentAsset.CreateConcreteHSMState());
            }
        }

        for(int i = 0; i < baseAsset.childs.Length; i++)
        {
            var currentAsset = baseAsset.childs[i];
            var currentState = baseState.GetImmediateChildState(currentAsset.stateId);

            ReplaceCopiables(currentState, currentAsset);

            if(currentAsset.copy)
            {
                baseState.ReplaceChild(currentAsset.CreateConcreteHSMState());
            }
        }
    }

    protected abstract HSMTransition<TState, TTrigger>[] GetTransitions();

    protected virtual HSMState<TState, TTrigger> CreateConcreteHSMState()
    {
        HSMState<TState, TTrigger> state = (HSMState<TState, TTrigger>)Activator.CreateInstance(Type.GetType(stateClassFullName), stateId, debugName);

        return state;
    }

    void OnValidate()
    {
#if UNITY_EDITOR
        if (script != null)
        {
            MonoScript monoscript = script as MonoScript;

            if (monoscript == null)
            {
                throw new InvalidOperationException("Text asset is not a valid Monoscript");
            }

            Type stateType = monoscript.GetClass();

            if(stateType == null)
            {
                throw new NullReferenceException("The name of the class on the script " + script.name.ToUpper() + " doesn't match the script's name");
            }

            stateClassFullName = stateType.FullName;
        }
#endif
    }
}

/*public interface IHSMTransitionSerializationWrapper<TState, TTrigger> where TState : unmanaged where TTrigger : unmanaged
{
    HSMTransition<TState, TTrigger> ToHSMTransition();
}*/

