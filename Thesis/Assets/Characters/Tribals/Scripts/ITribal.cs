using SJ.Tools;

namespace SJ.GameEntities.Characters.Tribals
{
    public interface ITribal : ICharacter, IDamagable, ICompositeCollisionEnter2DCallbackCaller
    {
        IAnimator Animator { get; }
        IRigidbody2D RigidBody2D { get; }
        ITransform transform { get; }
        ICapsuleCollider2D Collider { get; }
        PercentageReversibleNumber MaxMovementVelocity { get; }
        PercentageReversibleNumber MovementAcceleration { get; }
        PercentageReversibleNumber JumpMaxTime { get; }
        PercentageReversibleNumber JumpAcceleration { get; }

        void EnqueueTrigger(Tribal.Trigger trigger);
    }
}