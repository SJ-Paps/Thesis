using SJ.GameEntities.Characters;

public class Hide : ActivableObject<Character> {
    
    public override bool Activate(Character user)
    {
        return true;
    }
}
