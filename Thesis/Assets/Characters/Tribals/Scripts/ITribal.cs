using SJ.Tools;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals
{
    public interface ITribal : ICharacter, IDamagable
    {
        Animator Animator { get; }
        IRigidbody2D RigidBody2D { get; }
        PercentageReversibleNumber MaxMovementVelocity { get; }
        PercentageReversibleNumber MovementAcceleration { get; }
        PercentageReversibleNumber JumpMaxHeight { get; }
        PercentageReversibleNumber JumpAcceleration { get; }
    }
}