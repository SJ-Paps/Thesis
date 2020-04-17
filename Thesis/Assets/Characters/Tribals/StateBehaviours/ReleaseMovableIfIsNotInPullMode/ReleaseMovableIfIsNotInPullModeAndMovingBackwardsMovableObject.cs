using SJ.Tools;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class ReleaseMovableIfIsNotInPullModeAndMovingBackwardsMovableObject : TribalStateBehaviour
    {
        public override void OnEnter()
        {
            Blackboard.AddOnValueChangedListener(Tribal.BlackboardKeys.PullMode, CheckShouldRelease);
        }

        public override void OnExit()
        {
            Blackboard.RemoveOnValueChangedListener(Tribal.BlackboardKeys.PullMode, CheckShouldRelease);
        }
        
        private void CheckShouldRelease()
        {
            if(IsNotInPullMode() && IsMovingBackwardsMovableObject())
                Trigger(Tribal.Trigger.Release);
        }

        private bool IsNotInPullMode()
        {
            return Blackboard.GetItem<bool>(Tribal.BlackboardKeys.PullMode) == false;
        }

        private bool IsMovingBackwardsMovableObject()
        {
            return Blackboard.GetItem<HorizontalDirection>(Tribal.BlackboardKeys.MoveDirection) != Owner.FacingDirection;
        }
    }
}