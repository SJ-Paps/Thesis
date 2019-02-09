﻿using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif


public abstract class HSMStateAsset<TConcreteAssetClass, THSMTransitionWrapper, TState, TTrigger> : ScriptableObject
                                                                        where TConcreteAssetClass : HSMStateAsset<TConcreteAssetClass, THSMTransitionWrapper, TState, TTrigger>
                                                                        where THSMTransitionWrapper : IHSMTransitionSerializationWrapper<TState, TTrigger>
                                                                        where TState : unmanaged
                                                                        where TTrigger : unmanaged
{
    [SerializeField]
    protected TextAsset script;

    [SerializeField]
    [ReadOnly]
    protected string stateClassFullName;

    [SerializeField]
    protected string debugName;

    [SerializeField]
    protected TState stateId;

    [SerializeField]
    protected int startStateIndex;

    [SerializeField]
    protected TConcreteAssetClass[] childs;

    [SerializeField]
    protected TConcreteAssetClass[] parallelChilds;

    [SerializeField]
    protected THSMTransitionWrapper[] transitions;
    

    public static HSMState<TState, TTrigger> BuildFromAsset(HSMStateAsset<TConcreteAssetClass, THSMTransitionWrapper, TState, TTrigger> rootAsset)
    {
        var relationDictionary = new Dictionary<HSMStateAsset<TConcreteAssetClass, THSMTransitionWrapper, TState, TTrigger>, HSMState<TState, TTrigger>>();

        GetUniqueStates(relationDictionary, rootAsset);

        BuildHierarchy(relationDictionary, rootAsset);

        BuildTransitions(relationDictionary, rootAsset);

        SetInitialStates(relationDictionary, rootAsset);

        HSMState<TState, TTrigger> rootState = relationDictionary[rootAsset];

        return rootState;
    }

    private static void GetUniqueStates(Dictionary<HSMStateAsset<TConcreteAssetClass, THSMTransitionWrapper, TState, TTrigger>, HSMState<TState, TTrigger>> relationDictionary, HSMStateAsset<TConcreteAssetClass, THSMTransitionWrapper, TState, TTrigger> baseAsset)
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

    private static void BuildHierarchy(Dictionary<HSMStateAsset<TConcreteAssetClass, THSMTransitionWrapper, TState, TTrigger>, HSMState<TState, TTrigger>> relationDictionary, HSMStateAsset<TConcreteAssetClass, THSMTransitionWrapper, TState, TTrigger> baseAsset)
    {
        HSMState<TState, TTrigger> currentState = relationDictionary[baseAsset];

        for(int i = 0; i < baseAsset.parallelChilds.Length; i++)
        {
            HSMStateAsset<TConcreteAssetClass, THSMTransitionWrapper, TState, TTrigger> currentAssetChild = baseAsset.parallelChilds[i];

            currentState.AddParallelChild(relationDictionary[currentAssetChild]);
            BuildHierarchy(relationDictionary, currentAssetChild);
        }

        for(int i = 0; i < baseAsset.childs.Length; i++)
        {
            HSMStateAsset<TConcreteAssetClass, THSMTransitionWrapper, TState, TTrigger> currentAssetChild = baseAsset.childs[i];

            currentState.AddChild(relationDictionary[currentAssetChild]);
            BuildHierarchy(relationDictionary, currentAssetChild);
        }
    }

    private static void BuildTransitions(Dictionary<HSMStateAsset<TConcreteAssetClass, THSMTransitionWrapper, TState, TTrigger>, HSMState<TState, TTrigger>> relationDictionary, HSMStateAsset<TConcreteAssetClass, THSMTransitionWrapper, TState, TTrigger> baseAsset)
    {
        HSMState<TState, TTrigger> currentState = relationDictionary[baseAsset];

        for (int i = 0; i < baseAsset.transitions.Length; i++)
        {
            HSMTransition<TState, TTrigger> transition = baseAsset.transitions[i].ToHSMTransition();

            currentState.MakeChildTransition(transition);
        }

        for (int i = 0; i < baseAsset.parallelChilds.Length; i++)
        {
            HSMStateAsset<TConcreteAssetClass, THSMTransitionWrapper, TState, TTrigger> currentAssetChild = baseAsset.parallelChilds[i];

            BuildTransitions(relationDictionary, currentAssetChild);
        }

        for (int i = 0; i < baseAsset.childs.Length; i++)
        {
            HSMStateAsset<TConcreteAssetClass, THSMTransitionWrapper, TState, TTrigger> currentAssetChild = baseAsset.childs[i];

            BuildTransitions(relationDictionary, currentAssetChild);
        }
    }

    private static void SetInitialStates(Dictionary<HSMStateAsset<TConcreteAssetClass, THSMTransitionWrapper, TState, TTrigger>, HSMState<TState, TTrigger>> relationDictionary, HSMStateAsset<TConcreteAssetClass, THSMTransitionWrapper, TState, TTrigger> baseAsset)
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
            MonoScript monoscript = (MonoScript)script;

            if (monoscript == null)
            {
                throw new InvalidOperationException("Text asset is not a valid Monoscript");
            }

            Type classType = monoscript.GetClass();

            if(classType == null)
            {
                throw new NullReferenceException("The name of the class on the script " + script.name.ToUpper() + " doesn't match the script's name");
            }

            stateClassFullName = classType.FullName;
        }
#endif
    }
}

public interface IHSMTransitionSerializationWrapper<TState, TTrigger> where TState : unmanaged where TTrigger : unmanaged
{
    HSMTransition<TState, TTrigger> ToHSMTransition();
}
