using Paps.HierarchicalStateMachine_ToolsForUnity;
using Paps.StateMachines;
using SJ.GameEntities.Characters.Tribals.States;
using System;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals
{
    public abstract class Tribal : Character, IDamagable
    {
        public enum State
        {
            Alive,
            Dead,
            OnGround,
            OnAir,
            Jumping,
            Falling,
            Standing,
            Ducking,
            StandingIdle,
            DuckingIdle,
            StandingMoving,
            DuckingMoving,
            StandingTrotting,
            DuckingTrotting,
            StandingWalking,
            DuckingWalking,
            Running,
            Hidden,
            Pushing,
            Pulling,
            ChoiceOnAir
        }

        public enum Trigger
        {
            Die,
            Ground,
            Jump,
            Fall,
            GetUp,
            GetDown,
            Stop,
            Trot,
            Walk,
            Run,
            Hide,
            Push,
            Pull
        }

        public class TribalSaveData
        {
            public float x;
            public float y;
        }

        public class TribalStateEvent : IEvent<Order>
        {
            public Order eventData;

            public Order GetEventData()
            {
                return eventData;
            }

            object IEvent.GetEventData()
            {
                return eventData;
            }
        }

        public const string LastTriggerBlackboardKey = "LastTrigger";

        public static readonly AnimatorParameterId TrotAnimatorTrigger = new AnimatorParameterId("Move");
        public static readonly AnimatorParameterId RunAnimatorTrigger = new AnimatorParameterId("Move");
        public static readonly AnimatorParameterId WalkAnimatorTrigger = new AnimatorParameterId("Move");
        public static readonly AnimatorParameterId IdleAnimatorTrigger = new AnimatorParameterId("Idle");
        public static readonly AnimatorParameterId GroundAnimatorTrigger = new AnimatorParameterId("Ground");
        public static readonly AnimatorParameterId FallAnimatorTrigger = new AnimatorParameterId("Fall");
        public static readonly AnimatorParameterId JumpAnimatorTrigger = new AnimatorParameterId("Jump");
        public static readonly AnimatorParameterId HideAnimatorTrigger = new AnimatorParameterId("Idle");
        public static readonly AnimatorParameterId ClimbLedgeAnimatorTrigger = new AnimatorParameterId("ClimbLedge");

        public PercentageReversibleNumber MaxMovementVelocity { get; protected set; }
        public PercentageReversibleNumber MovementAcceleration { get; protected set; }
        public PercentageReversibleNumber JumpMaxHeight { get; protected set; }
        public PercentageReversibleNumber JumpAcceleration { get; protected set; }

        public Vector2 CurrentVelocity => RigidBody2D.velocity;

        public Animator Animator { get; protected set; }
        public Rigidbody2D RigidBody2D { get; protected set; }
        public SJCapsuleCollider2D Collider { get; protected set; }

        public event Action OnDead;

        [SerializeField]
        private float maxMovementVelocity, movementAcceleration, jumpMaxHeight, jumpAcceleration;

        [SerializeField]
        private HierarchicalStateMachineBuilder hsmBuilder;

        private HierarchicalStateMachine<State, Trigger> hsm;
        private Blackboard blackboard = new Blackboard();
        private TribalStateEvent stateEvent = new TribalStateEvent();

        protected override void SJAwake()
        {
            Animator = GetComponent<Animator>();
            RigidBody2D = GetComponent<Rigidbody2D>();
            Collider = GetComponent<SJCapsuleCollider2D>();

            MaxMovementVelocity = new PercentageReversibleNumber(maxMovementVelocity);
            MovementAcceleration = new PercentageReversibleNumber(movementAcceleration);
            JumpMaxHeight = new PercentageReversibleNumber(jumpMaxHeight);
            JumpAcceleration = new PercentageReversibleNumber(jumpAcceleration);

            hsm = (HierarchicalStateMachine<State, Trigger>)hsmBuilder.Build();

            InitializeStates();

            hsm.Start();

            base.SJAwake();
        }

        private void InitializeStates()
        {
            var states = hsm.GetStates();

            foreach (var state in states)
            {
                var stateObject = hsm.GetStateById(state);

                if (stateObject is TribalState tribalState)
                    tribalState.InitializeWith(this, hsm, blackboard);
            }

            hsm.OnBeforeActiveHierarchyPathChanges += SaveLastTrigger;
        }

        private void SaveLastTrigger(Trigger trigger)
        {
            blackboard.SetItem(LastTriggerBlackboardKey, trigger);
        }

        protected override void SJLateUpdate()
        {
            hsm.Update();
        }

        protected override void OnSendOrder(Order orderEvent)
        {
            stateEvent.eventData = orderEvent;
            hsm.SendEvent(stateEvent);
        }

        public virtual void TakeDamage(float damage, DamageType damageType)
        {
            hsm.Trigger(Trigger.Die);

            OnDead?.Invoke();
        }

        protected override object GetSaveData()
        {
            TribalSaveData saveData = new TribalSaveData()
            {
                x = transform.position.x,
                y = transform.position.y
            };

            return saveData;
        }

        protected override void LoadSaveData(object data)
        {
            TribalSaveData saveData = (TribalSaveData)data;

            transform.position = new Vector2(saveData.x, saveData.y);
        }

        public void Move(FaceDirection direction, float extraForceMultiplier = 1)
        {
            ApplyForceOnDirection(direction, MovementAcceleration * extraForceMultiplier);

            if (IsOverMaximumVelocity())
                ClampVelocity(direction == FaceDirection.Left ? FaceDirection.Right : FaceDirection.Left, extraForceMultiplier);
        }

        private bool IsOverMaximumVelocity()
        {
            return RigidBody2D.velocity.x > MaxMovementVelocity || RigidBody2D.velocity.x < MaxMovementVelocity * -1;
        }

        private void ApplyForceOnDirection(FaceDirection direction, float force)
        {
            RigidBody2D.AddForce(new Vector2((int)direction * force, 0), ForceMode2D.Impulse);
        }

        private void ClampVelocity(FaceDirection oppositeDirection, float extraForceMultiplier)
        {
            ApplyForceOnDirection(oppositeDirection, MovementAcceleration * extraForceMultiplier);
        }
    }
}