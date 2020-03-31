namespace SJ.GameEntities.Characters
{
    public interface ICharacter : ISaveableGameEntity, IControllable<Character.Order>
    {
        FaceDirection FacingDirection { get; }
        bool BlockFacing { get; set; }

        void Face(FaceDirection direction);
    }
}