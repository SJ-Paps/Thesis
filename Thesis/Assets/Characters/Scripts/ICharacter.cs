using SJ.Tools;

namespace SJ.GameEntities.Characters
{
    public interface ICharacter : ISaveableGameEntity, IControllable<Character.Order>
    {
        HorizontalDirection FacingDirection { get; }
        bool BlockFacing { get; set; }

        void Face(HorizontalDirection direction);
    }
}