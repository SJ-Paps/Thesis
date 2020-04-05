using Paps.HierarchicalStateMachine_ToolsForUnity;
using Paps.StateMachines;
using SJ.GameEntities.Characters.Tribals.States;
using SJ.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals
{
    [RequireComponent(typeof(Rigidbody2DWrapper), typeof(TransformWrapper), typeof(AnimatorWrapper))]
    [RequireComponent(typeof(CapsuleCollider2DWrapper), typeof(CompositeCollisionTrigger2DCallbackCaller))]
    [DisallowMultipleComponent]
    [SelectionBase]
    public abstract class Tribal : Character, ITribal
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
            IdleStanding,
            IdleDucking,
            MovingStanding,
            MovingDucking,
            TrottingStanding,
            TrottingDucking,
            WalkingStanding,
            WalkingDucking,
            Running,
            Hidden,
            Grappling,
            Pulling,
            ChoiceOnAir,
            Braking,
            ChoiceMoving,
            IdlePulling,
            MovingPulling,
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
            Move,
            Pull,
            Release
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

        public static class BlackboardKeys
        {
            public const string LastTrigger = "LastTrigger";
            public const string WalkMode = "WalkMode";
            public const string MovableObject = "MovableObject";
            public const string PullMoveDirection = "PullMoveDirection";
            public const string PullMode = "PullMode";
        }

        public static class AnimatorTriggers
        {
            public static readonly AnimatorParameterId Trot = new AnimatorParameterId("Move");
            public static readonly AnimatorParameterId Run = new AnimatorParameterId("Move");
            public static readonly AnimatorParameterId Walk = new AnimatorParameterId("Move");
            public static readonly AnimatorParameterId Idle = new AnimatorParameterId("Idle");
            public static readonly AnimatorParameterId Ground = new AnimatorParameterId("Ground");
            public static readonly AnimatorParameterId Fall = new AnimatorParameterId("Fall");
            public static readonly AnimatorParameterId Jump = new AnimatorParameterId("Jump");
            public static readonly AnimatorParameterId Hidden = new AnimatorParameterId("Idle");
            public static readonly AnimatorParameterId ClimbLedge = new AnimatorParameterId("ClimbLedge");
        }

        public IAnimator Animator { get; protected set; }
        public IRigidbody2D RigidBody2D { get; protected set; }
        public new ITransform transform { get; protected set; }
        public ICapsuleCollider2D Collider { get; protected set; }

        public PercentageReversibleNumber MaxMovementVelocity { get; protected set; }
        public PercentageReversibleNumber MovementAcceleration { get; protected set; }
        public PercentageReversibleNumber JumpAcceleration { get; protected set; }
        public PercentageReversibleNumber JumpMaxForce { get; protected set; }

        public event Action OnDead;

        private CompositeCollisionTrigger2DCallbackCaller collisionCallbackCaller;

        [Header("Tribal Configuration")]
        [SerializeField]
        private float maxMovementVelocity;
        [SerializeField]
        private float movementAcceleration;
        [SerializeField]
        private float jumpAcceleration;
        [SerializeField]
        private float jumpMaxForce;

        [SerializeField]
        private HierarchicalStateMachineBuilder hsmBuilder;

        private HierarchicalStateMachine<State, Trigger> hsm;
        private Blackboard blackboard = new Blackboard();
        private TribalStateEvent stateEvent = new TribalStateEvent();

        private Queue<Trigger> pendingTriggerQueue = new Queue<Trigger>();
        private Queue<Trigger> frameTriggerQueue = new Queue<Trigger>();

        protected override void SJAwake()
        {
            CacheComponents();
            InitializeConfigurationProperties();
            InitializeStateMachine();

            base.SJAwake();
        }

        private void InitializeStateMachine()
        {
            hsm = (HierarchicalStateMachine<State, Trigger>)hsmBuilder.Build();

            InitializeStates();
            InitializeGuardConditions();
            hsm.OnBeforeActiveHierarchyPathChanges += SaveLastTrigger;

            hsm.Start();
        }

        private void InitializeConfigurationProperties()
        {
            MaxMovementVelocity = new PercentageReversibleNumber(maxMovementVelocity);
            MovementAcceleration = new PercentageReversibleNumber(movementAcceleration);
            JumpAcceleration = new PercentageReversibleNumber(jumpAcceleration);
            JumpMaxForce = new PercentageReversibleNumber(jumpMaxForce);
        }

        private void CacheComponents()
        {
            Animator = GetComponent<IAnimator>();
            RigidBody2D = GetComponent<IRigidbody2D>();
            transform = GetComponent<ITransform>();
            Collider = GetComponent<ICapsuleCollider2D>();

            collisionCallbackCaller = GetComponent<CompositeCollisionTrigger2DCallbackCaller>();
        }

        private void InitializeStates()
        {
            var states = hsm.GetStates();

            foreach (var state in states)
            {
                var stateObject = hsm.GetStateById(state);

                if (stateObject is ITribalStateMachineElement stateMachineElement)
                    stateMachineElement.InitializeWith(this, hsm, blackboard);
            }
        }

        private void InitializeGuardConditions()
        {
            var transitions = hsm.GetTransitions();

            foreach (var transition in transitions)
            {
                var guardConditions = hsm.GetGuardConditionsOf(transition);

                if(guardConditions != null)
                {
                    foreach (var guardCondition in guardConditions)
                    {
                        if (guardCondition is ITribalStateMachineElement stateMachineElement)
                            stateMachineElement.InitializeWith(this, hsm, blackboard);
                    }
                }
            }
        }

        private void SaveLastTrigger(Trigger trigger)
        {
            blackboard.SetItem(BlackboardKeys.LastTrigger, trigger);
        }

        protected override void SJLateUpdate()
        {
            hsm.Update();
            ProcessTriggerQueue();
        }

        public void EnqueueTrigger(Trigger trigger)
        {
            pendingTriggerQueue.Enqueue(trigger);
        }

        private void ProcessTriggerQueue()
        {
            if(pendingTriggerQueue.Count > 0)
            {
                foreach (var trigger in pendingTriggerQueue)
                    frameTriggerQueue.Enqueue(trigger);

                pendingTriggerQueue.Clear();

                foreach (var trigger in frameTriggerQueue)
                    hsm.Trigger(trigger);

                frameTriggerQueue.Clear();
            }
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

        public void SubscribeToOnCollisionEnter(IOnCollisionEnter2DListener listener)
        {
            collisionCallbackCaller.SubscribeToOnCollisionEnter(listener);
        }

        public void UnsubscribeFromOnCollisionEnter(IOnCollisionEnter2DListener listener)
        {
            collisionCallbackCaller.UnsubscribeFromOnCollisionEnter(listener);
        }
    }
}