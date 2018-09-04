using SAM.FSM;
using UnityEngine;
using System;

public class Xenophobic : Tribal, IAudibleListener
{
    public event Action<Character> onPlayerDetected;

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

    [SerializeField]
    protected VisionTrigger nearVisionTrigger, distantVisionTrigger;

    protected Character player;

    protected override void Awake()
    {
        base.Awake();

        attackFSM = new FSM<State, Trigger>();

        attackFSM.AddState(State.Idle);
        attackFSM.AddState(new XenophobicAttackState(attackFSM, State.Attacking, this, orders));

        attackFSM.MakeTransition(State.Idle, Trigger.Attack, State.Attacking);
        attackFSM.MakeTransition(State.Attacking, Trigger.StopAttacking, State.Idle);

        attackFSM.StartBy(State.Idle);

        if(nearVisionTrigger != null)
        {
            nearVisionTrigger.onPlayerDetected += OnPlayerDetected;
        }

        if(distantVisionTrigger != null)
        {
            distantVisionTrigger.onPlayerDetected += OnSomethingDetected;
        }
    }

    public override void GetEnslaved()
    {

    }

    public void Listen(ref AudibleData data)
    {
        
    }

    public override void SetOrder(Order order)
    {
        
    }

    protected virtual void OnPlayerDetected(Character player)
    {
        EditorDebug.Log("TE VI");

        this.player = player;

        if(onPlayerDetected != null)
        {
            onPlayerDetected(player);
        }
    }

    protected virtual void OnSomethingDetected(Character something)
    {
        EditorDebug.Log("QUE FUE ESO?");
    }
}
