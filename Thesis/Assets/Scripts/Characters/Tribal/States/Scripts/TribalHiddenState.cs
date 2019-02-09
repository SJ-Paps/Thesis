public class TribalHiddenState : TribalHSMState
{
    /*private SyncTimer timerForComingOut;
    
    private float cooldownForComingOut = 0.7f;

    private Rigidbody2D rigidbody2D;

    private SortingGroup sortingGroup;*/

    public TribalHiddenState(Character.State state, string debugName) : base(state, debugName)
    {
        /*timerForComingOut = new SyncTimer();

        //timerForComingOut.onTick += StopTimerForComingOut;
        timerForComingOut.Interval = cooldownForComingOut;

        rigidbody2D = character.RigidBody2D;

        sortingGroup = character.GetComponentInChildren<SortingGroup>();*/
    }

    /*protected override void OnEnter() 
    {
        blackboard.isHidden = true;

        rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);

        sortingGroup.sortingOrder = 4;
        EditorDebug.Log("HIDDEN ENTER");
    }

    protected override void OnExit()
    {
        blackboard.isHidden = false;

        sortingGroup.sortingOrder = 6;
        EditorDebug.Log("HIDDEN EXIT");
    }

    protected override void OnUpdate() 
    {
        Hide();
    }

    private void Hide() 
    {
        timerForComingOut.Update(Time.deltaTime);

        for (int i = 0; i < orders.Count; i++)
        {
            Character.Order ev = orders[i];

            if (ev == Character.Order.OrderHide && !timerForComingOut.Active) 
            {
                EditorDebug.Log("LLAMADO AL TIMER HIDDEN");
                timerForComingOut.Start();
            }
        }
    }

    private void ComingOutOfTheHidingPlace() 
    {
        stateMachine.Trigger(Character.Trigger.StopHiding);
    }

    void StopTimerForComingOut(SyncTimer timer) 
    {
        ComingOutOfTheHidingPlace();
    }*/
}