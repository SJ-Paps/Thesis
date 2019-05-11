using System.Collections.Generic;
using UnityEngine;
using System;
using Paps.StateMachines.HSM;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class HSMStateAsset<TState, TTrigger> : ScriptableObject where TState : unmanaged where TTrigger : unmanaged
{
    [SerializeField]
    protected TextAsset script;

    [SerializeField]
    [ReadOnly]
    protected string stateClassFullName;

    [SerializeField]
    protected string debugName;

    [SerializeField]
    protected bool copy;

    [SerializeField]
    [HideInInspector]
    protected TState stateId;

    [SerializeField]
    protected int startStateIndex;
    

    public static HSMState<TState, TTrigger> BuildFromAsset(HSMStateAsset<TState, TTrigger> rootAsset)
    {
        var relationDictionary = new Dictionary<HSMStateAsset<TState, TTrigger>, HSMState<TState, TTrigger>>();

        GetUniqueStates(relationDictionary, rootAsset);

        HSMState<TState, TTrigger> rootState = BuildHierarchy(relationDictionary, rootAsset);

        ReplaceCopiables(rootState, rootAsset);

        BuildTransitions(relationDictionary, rootAsset);

        SetInitialStates(relationDictionary, rootAsset);

        return rootState;
    }

    private static void GetUniqueStates(Dictionary<HSMStateAsset<TState, TTrigger>, HSMState<TState, TTrigger>> relationDictionary, HSMStateAsset<TState, TTrigger> baseAsset)
    {
        HSMStateAsset<TState, TTrigger>[] parallelChilds = baseAsset.GetParallelChilds();
        HSMStateAsset<TState, TTrigger>[] childs = baseAsset.GetNonParallelChilds();

        if(relationDictionary.ContainsKey(baseAsset) == false)
        {
            relationDictionary.Add(baseAsset, baseAsset.CreateConcreteHSMState());
        }

        for(int i = 0; i < parallelChilds.Length; i++)
        {
            GetUniqueStates(relationDictionary, parallelChilds[i]);
        }

        for(int i = 0; i < childs.Length; i++)
        {
            GetUniqueStates(relationDictionary, childs[i]);
        }
    }

    private static HSMState<TState, TTrigger> BuildHierarchy(Dictionary<HSMStateAsset<TState, TTrigger>, HSMState<TState, TTrigger>> relationDictionary, HSMStateAsset<TState, TTrigger> baseAsset)
    {
        HSMState<TState, TTrigger> currentState = relationDictionary[baseAsset];
        HSMStateAsset<TState, TTrigger>[] parallelChilds = baseAsset.GetParallelChilds();
        HSMStateAsset<TState, TTrigger>[] childs = baseAsset.GetNonParallelChilds();

        for (int i = 0; i < parallelChilds.Length; i++)
        {
            HSMStateAsset<TState, TTrigger> currentAssetChild = parallelChilds[i];

            currentState.AddParallelChild(relationDictionary[currentAssetChild]);
            BuildHierarchy(relationDictionary, currentAssetChild);
        }

        for(int i = 0; i < childs.Length; i++)
        {
            HSMStateAsset<TState, TTrigger> currentAssetChild = childs[i];

            currentState.AddChild(relationDictionary[currentAssetChild]);
            BuildHierarchy(relationDictionary, currentAssetChild);
        }

        return currentState;
    }

    private static void BuildTransitions(Dictionary<HSMStateAsset<TState, TTrigger>, HSMState<TState, TTrigger>> relationDictionary, HSMStateAsset<TState, TTrigger> baseAsset)
    {
        HSMState<TState, TTrigger> currentState = relationDictionary[baseAsset];

        HSMTransition<TState, TTrigger>[] transitions = baseAsset.GetTransitions();
        HSMStateAsset<TState, TTrigger>[] parallelChilds = baseAsset.GetParallelChilds();
        HSMStateAsset<TState, TTrigger>[] childs = baseAsset.GetNonParallelChilds();

        for (int i = 0; i < transitions.Length; i++)
        {
            HSMTransition<TState, TTrigger> transition = transitions[i];

            currentState.MakeChildTransition(transition);
        }

        for (int i = 0; i < parallelChilds.Length; i++)
        {
            HSMStateAsset<TState, TTrigger> currentAssetChild = parallelChilds[i];

            BuildTransitions(relationDictionary, currentAssetChild);
        }

        for (int i = 0; i < childs.Length; i++)
        {
            HSMStateAsset<TState, TTrigger> currentAssetChild = childs[i];

            BuildTransitions(relationDictionary, currentAssetChild);
        }
    }

    private static void SetInitialStates(Dictionary<HSMStateAsset<TState, TTrigger>, HSMState<TState, TTrigger>> relationDictionary, HSMStateAsset<TState, TTrigger> baseAsset)
    {
        HSMState<TState, TTrigger> currentState = relationDictionary[baseAsset];
        
        HSMStateAsset<TState, TTrigger>[] childs = baseAsset.GetNonParallelChilds();

        if (childs.Length > 0)
        {
            currentState.SetInitialState(childs[baseAsset.startStateIndex].stateId);

            for (int i = 0; i < childs.Length; i++)
            {
                SetInitialStates(relationDictionary, childs[i]);
            }
        }
    }

    private static void ReplaceCopiables(HSMState<TState, TTrigger> baseState, HSMStateAsset<TState, TTrigger> baseAsset)
    {
        HSMStateAsset<TState, TTrigger>[] parallelChilds = baseAsset.GetParallelChilds();
        HSMStateAsset<TState, TTrigger>[] childs = baseAsset.GetNonParallelChilds();

        for (int i = 0; i < parallelChilds.Length; i++)
        {
            var currentAsset = parallelChilds[i];
            var currentState = baseState.GetImmediateChildState(currentAsset.stateId);

            ReplaceCopiables(currentState, currentAsset);

            if (currentAsset.copy)
            {
                baseState.ReplaceChild(currentAsset.CreateConcreteHSMState());
            }
        }

        for(int i = 0; i < childs.Length; i++)
        {
            var currentAsset = childs[i];
            var currentState = baseState.GetImmediateChildState(currentAsset.stateId);

            ReplaceCopiables(currentState, currentAsset);

            if(currentAsset.copy)
            {
                baseState.ReplaceChild(currentAsset.CreateConcreteHSMState());
            }
        }
    }

    protected abstract HSMTransition<TState, TTrigger>[] GetTransitions();

    protected abstract HSMStateAsset<TState, TTrigger>[] GetNonParallelChilds();

    protected abstract HSMStateAsset<TState, TTrigger>[] GetParallelChilds();

    protected abstract TState GetStateId();

    protected virtual HSMState<TState, TTrigger> CreateConcreteHSMState()
    {
        HSMState<TState, TTrigger> state = (HSMState<TState, TTrigger>)Activator.CreateInstance(Type.GetType(stateClassFullName), stateId, debugName);

        return state;
    }

    void OnValidate()
    {
#if UNITY_EDITOR
        stateId = GetStateId();

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

