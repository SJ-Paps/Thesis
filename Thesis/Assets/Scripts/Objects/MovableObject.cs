using UnityEngine;

public class MovableObject : ActivableObject<Character> {
    
    private new Rigidbody2D rigidbody2D;

    public Rigidbody2D Rigidbody2D
    {
        get
        {
            if(rigidbody2D == null)
            {
                rigidbody2D = GetComponent<Rigidbody2D>();
            }

            return rigidbody2D;
        }
    }

    public override bool Activate(Character user)
    {
        return true;
    }

}
