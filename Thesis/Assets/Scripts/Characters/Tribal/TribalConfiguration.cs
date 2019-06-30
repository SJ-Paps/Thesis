using UnityEngine;

public class TribalConfiguration : CharacterConfiguration
{
    [SerializeField] private Transform hand;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Equipment equipment;
    [SerializeField] private Animator animator;
    [SerializeField] private new Rigidbody2D rigidbody2D;
    [SerializeField] private new SJCapsuleCollider2D collider;

    [SerializeField]
    private float maxMovementVelocity, acceleration, jumpMaxHeight, jumpAcceleration, jumpForceFromLadder, climbForce;

    public Inventory Inventory { get => inventory; }
    public Equipment Equipment { get => equipment; }
    public Transform Hand { get => hand; }
    public Animator Animator { get => animator; }
    public Rigidbody2D RigidBody2D { get => rigidbody2D; }
    public SJCapsuleCollider2D Collider { get => collider; }

    public float MaxMovementVelocity { get => maxMovementVelocity; }
    public float Acceleration { get => acceleration; }
    public float JumpMaxHeight { get => jumpMaxHeight; }
    public float JumpAcceleration { get => jumpAcceleration; }
    public float JumpForceFromLadder { get => jumpForceFromLadder; }
    public float ClimbForce { get => climbForce; }
    
}
