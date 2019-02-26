public class Hide : ActivableObject<Character> {
    
    public override bool Activate(Character user)
    {
        EditorDebug.Log("OBJETO HIDE " + name + " FUE ACTIVADO");

        return true;
    }
}
