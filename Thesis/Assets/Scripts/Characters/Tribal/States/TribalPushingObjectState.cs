public class TribalPushingObjectState : TribalHSMState
{

    /*[SerializeField]
    private float pushCheckDistance = 0.4f;

    private FixedJoint2D objectFixedJoint2D;
    private Rigidbody2D characterRigidbody;

    private Character.Trigger expectedOrder;

    private int objectLayerMask;*/

    public TribalPushingObjectState(Character.State state, string debugName) : base(state, debugName)
    {
        /*characterRigidbody = character.RigidBody2D;

        objectLayerMask = 1 << Reg.movableObject;*/
    }

    /*protected override void OnEnter() {
        base.OnEnter();

        blackboard.isPushing = true;

        RaycastHit2D raycastHit2D = Physics2D.Raycast(character.transform.position, (Vector2)character.transform.right, pushCheckDistance, objectLayerMask);

        objectFixedJoint2D = raycastHit2D.transform.GetComponent<FixedJoint2D>();
        objectFixedJoint2D.enabled = true;
        objectFixedJoint2D.connectedBody = characterRigidbody;

        character.blockFacing = true;

        if (character.transform.right.x < 0)
        {
            expectedOrder = Character.Order.OrderMoveLeft;
        }
        else
        {
            expectedOrder = Character.Order.OrderMoveRight;
        }

        EditorDebug.Log("PUSHINGOBJECT ENTER");
    }

    protected override void OnExit() {
        base.OnExit();

        character.blockFacing = false;
        objectFixedJoint2D.enabled = false;
        objectFixedJoint2D.connectedBody = null;
        objectFixedJoint2D = null;

        EditorDebug.Log("PUSHINGOBJECT EXIT");
    }

    protected override void OnUpdate() {
        
        if(character.IsGrounded)
        {
            for (int i = 0; i < orders.Count; i++)
            {
                Character.Order ev = orders[i];

                if (ev == Character.Order.OrderPush || ev == expectedOrder)
                {
                    return;
                }
            }
        }

        stateMachine.Trigger(Character.Trigger.StopPushing);
    }*/
}
