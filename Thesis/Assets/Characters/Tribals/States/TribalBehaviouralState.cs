using Paps.StateMachines.Extensions.BehaviouralStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class TribalBehaviouralState : TribalState, IBehaviouralState
    {
        [SerializeField]
        private TribalStateBehaviour[] behaviours;

        private BehaviouralState innerBehaviouralState = new BehaviouralState();

        public int BehaviourCount => innerBehaviouralState.BehaviourCount;

        protected override void Initialize()
        {
            AddInitialBehaviours();
            InitializeBehaviours();
        }

        protected override void OnEnter()
        {
            innerBehaviouralState.Enter();
        }

        private void AddInitialBehaviours()
        {
            for (int i = 0; i < behaviours.Length; i++)
                innerBehaviouralState.AddBehaviour(behaviours[i]);
        }

        private void InitializeBehaviours()
        {
            for (int i = 0; i < behaviours.Length; i++)
                behaviours[i].Initialize();
        }

        protected override void OnUpdate()
        {
            innerBehaviouralState.Update();
        }

        protected override void OnExit()
        {
            innerBehaviouralState.Exit();
        }

        public void AddBehaviour(IStateBehaviour stateBehaviour) => innerBehaviouralState.AddBehaviour(stateBehaviour);

        public bool ContainsBehaviour(IStateBehaviour stateBehaviour) => innerBehaviouralState.ContainsBehaviour(stateBehaviour);

        public T GetBehaviour<T>() => innerBehaviouralState.GetBehaviour<T>();

        public T[] GetBehaviours<T>() => innerBehaviouralState.GetBehaviours<T>();

        public IEnumerator<IStateBehaviour> GetEnumerator() => innerBehaviouralState.GetEnumerator();

        public bool RemoveBehaviour(IStateBehaviour stateBehaviour) => innerBehaviouralState.RemoveBehaviour(stateBehaviour);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return innerBehaviouralState.GetEnumerator();
        }
    }
}