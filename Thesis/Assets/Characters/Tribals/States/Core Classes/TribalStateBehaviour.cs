using Paps.HierarchicalStateMachine_ToolsForUnity;
using Paps.StateMachines;
using SJ.Tools;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public abstract class TribalStateBehaviour : ScriptableStateBehaviour, ITribalStateMachineElement
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

        protected virtual bool OnHandleEvent(Character.Order ev) => false;

        protected void Trigger(Tribal.Trigger trigger)
        {
            StateMachine.Trigger(trigger);
        }

        public override sealed bool HandleEvent(IEvent ev)
        {
            var orderEvent = ((Tribal.TribalStateEvent)ev).eventData;

            return OnHandleEvent(orderEvent);
        }
    }
}