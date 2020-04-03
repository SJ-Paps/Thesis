using Paps.HierarchicalStateMachine_ToolsForUnity;
using Paps.StateMachines;
using SJ.Tools;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public abstract class TribalGuardCondition : ScriptableGuardCondition, ITribalStateMachineElement
    {
        protected ITribal Owner { get; private set; }
        protected IHierarchicalStateMachine<Tribal.State, Tribal.Trigger> StateMachine { get; private set; }
        protected IBlackboard Blackboard { get; private set; }

        private bool isInitialized = false;

        public void InitializeWith(ITribal owner, IHierarchicalStateMachine<Tribal.State, Tribal.Trigger> stateMachine, IBlackboard blackboard)
        {
            if (isInitialized)
                return;

            Owner = owner;
            StateMachine = stateMachine;
            Blackboard = blackboard;

            isInitialized = true;
            Initialize();
        }

        protected virtual void Initialize() { }
    }
}