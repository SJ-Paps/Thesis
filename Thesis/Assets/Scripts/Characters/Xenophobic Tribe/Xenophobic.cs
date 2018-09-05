using SAM.FSM;
using UnityEngine;
using System;

public class Xenophobic : Tribal, IAudibleListener
{
    public event Action<Character> onPlayerDetected;
    public event Action<Vector2> onSomethingDetected;
    public event Action<Character> onPlayerReached;

    [SerializeField]
    protected Weapon weapon;

    public Weapon Weapon
    {
        get
        {
            return weapon;
        }

        protected set
        {
            weapon = value;
        }
    }

    protected FSM<State, Trigger> attackFSM;
    protected FSM<State, Trigger> movementFSM;
    protected FSM<State, Trigger> jumpingFSM;

    [SerializeField]
    protected VisionTrigger nearestVisionTrigger, nearVisionTrigger, distantVisionTrigger;

    protected override void Awake()
    {
        base.Awake();

        movementFSM = new FSM<State, Trigger>();

        movementFSM.AddState(new CharacterIdleState(movementFSM, State.Idle, this, orders));
        movementFSM.AddState(new MovingState(movementFSM, State.Moving, this, orders));

        movementFSM.MakeTransition(State.Idle, Trigger.Move, State.Moving);
        movementFSM.MakeTransition(State.Moving, Trigger.StopMoving, State.Idle);

        movementFSM.StartBy(State.Idle);
        

        jumpingFSM = new FSM<State, Trigger>();

        jumpingFSM.AddState(new GroundedState(jumpingFSM, State.Grounded, this, orders));
        jumpingFSM.AddState(new JumpingState(jumpingFSM, State.Jumping, this, orders));
        jumpingFSM.AddState(new FallingState(jumpingFSM, State.Falling, this, orders));

        jumpingFSM.MakeTransition(State.Grounded, Trigger.Jump, State.Jumping);
        jumpingFSM.MakeTransition(State.Grounded, Trigger.Fall, State.Falling);
        jumpingFSM.MakeTransition(State.Jumping, Trigger.Fall, State.Falling);
        jumpingFSM.MakeTransition(State.Falling, Trigger.Ground, State.Grounded);

        jumpingFSM.StartBy(State.Falling);


        attackFSM = new FSM<State, Trigger>();

        attackFSM.AddState(State.Idle);
        attackFSM.AddState(new XenophobicAttackState(attackFSM, State.Attacking, this, orders));

        attackFSM.MakeTransition(State.Idle, Trigger.Attack, State.Attacking);
        attackFSM.MakeTransition(State.Attacking, Trigger.StopAttacking, State.Idle);

        attackFSM.StartBy(State.Idle);

        AddStateMachineWhenAlive(movementFSM);
        AddStateMachineWhenAlive(jumpingFSM);
        AddStateMachineWhenAlive(attackFSM);

        nearestVisionTrigger.onSomethingDetected += OnPlayerReached;
        nearVisionTrigger.onSomethingDetected += OnPlayerDetected;
        distantVisionTrigger.onSomethingDetected += OnSomethingDetected;
    }

    public override void GetEnslaved()
    {

    }

    public void Listen(ref AudibleData data)
    {
        
    }

    protected virtual void OnPlayerDetected(Collider2D player)
    {
        EditorDebug.Log("TE VI");

        if(onPlayerDetected != null)
        {
            onPlayerDetected(GameManager.Instance.Player);
        }
    }

    protected virtual void OnSomethingDetected(Collider2D something)
    {
        EditorDebug.Log("QUE FUE ESO?");

        if(onSomethingDetected != null)
        {
            onSomethingDetected(something.transform.position);
        }
    }

    protected virtual void OnPlayerReached(Collider2D player)
    {
        EditorDebug.Log("TE TENGO");

        if(onPlayerReached != null)
        {
            onPlayerReached(GameManager.Instance.Player);
        }
    }
}
