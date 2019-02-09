
/*[Serializable]
public class TribalActionsIdleState : CharacterState 
{
    [SerializeField]
    private float pushCheckDistance = 0.4f, cooldownOfHiding = 0.7f;

    private bool canHide;
    private Animator animator;

    private List<IActivable> cachedActivables;

    public override void InitializeState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard)
    {
        base.InitializeState(fsm, state, character, orders, blackboard);

        animator = character.Animator;
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        
        Physics2D.queriesStartInColliders = false;
        EditorDebug.Log("ACTIONIDLE ENTER");
    }

    protected override void OnExit() 
    {
        base.OnExit();
        EditorDebug.Log("ACTIONIDLE EXIT");
    }

    protected override void OnUpdate()
    {
        EditorDebug.DrawLine(character.transform.position, (Vector2)character.transform.localPosition + (Vector2)character.transform.right * pushCheckDistance, Color.red);

        if (character.IsMovingHorizontal)
        {
            CheckPushable();
        }

        for (int i = 0; i < orders.Count; i++)
        {
            Character.Order ev = orders[i];

            if (ev == Character.Order.OrderHide && character.IsGrounded == true) 
            {
                Hide hide = FindActivable<Hide>();

                if (hide != null)
                {
                    hide.Activate(character);

                    stateMachine.Trigger(Character.Trigger.Hide);
                }
            }
            else if(ev == Character.Order.OrderPush)
            {
                CheckPushable();
            }
            else if(ev == Character.Order.OrderActivate)
            {
                IActivable activable = FindActivable<IActivable>();

                SwitchActivable(activable);
            }
            else if(ev == Character.Order.OrderAttack)
            {
                stateMachine.Trigger(Character.Trigger.Attack);
            }
        }
    }

    private T FindActivable<T>() where T : IActivable
    {
        Bounds bounds = character.Collider.bounds;

        float activableDetectionOffset = 0.2f;

        Vector2 detectionSize = new Vector2((bounds.extents.x * 2) + activableDetectionOffset, bounds.extents.y * 2);

        return SJUtil.FindActivable<T>(bounds.center, detectionSize, character.transform.eulerAngles.z);
    }

    private void SwitchActivable(IActivable activable)
    {
        if (activable != null)
        {
            if (activable is ContextualActivable)
            {
                if (character.IsGrounded)
                {
                    activable.Activate(character);
                }
            }
            else if (activable is Hide)
            {
                if (character.IsGrounded)
                {
                    activable.Activate(character);
                    stateMachine.Trigger(Character.Trigger.Hide);
                }
            }
        }
    }

    private void CheckPushable()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(character.transform.position, (Vector2)character.transform.right, pushCheckDistance, 1 << Reg.movableObject);

        if (raycastHit2D && character.IsGrounded)
        {
            stateMachine.Trigger(Character.Trigger.Push);
        }

    }
}*/
