using Paps.StateMachines;
using SJ.Tools;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public interface ITribalStateMachineElement
    {
        void InitializeWith(ITribal owner, IHierarchicalStateMachine<Tribal.State, Tribal.Trigger> stateMachine, IBlackboard blackboard);
    }
}