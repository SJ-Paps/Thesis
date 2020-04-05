using SJ.Tools;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals
{
    public static class TribalExtensions
    {
        public static bool IsTouchingWall(this ITribal tribal, HorizontalDirection faceDirection)
        {
            return IsTouchingWallLayer(tribal, faceDirection, Layers.Floor);
        }

        public static bool IsTouchingWallWalkable(this ITribal tribal, HorizontalDirection faceDirection)
        {
            return IsTouchingWallLayer(tribal, faceDirection, Layers.Walkable);
        }

        private static bool IsTouchingWallLayer(ITribal tribal, HorizontalDirection faceDirection, int layerMask)
        {
            int direction = (int)faceDirection;

            Bounds bounds = tribal.Collider.bounds;
            float widthExtents = 0.02f;
            float heightNegativeOffset = 0.1f;

            var frontPoint = new Vector2(bounds.center.x + ((bounds.extents.x + widthExtents) * direction), bounds.center.y);
            var size = new Vector2(widthExtents * 2, bounds.size.y - heightNegativeOffset);

            return Physics2D.OverlapBox(frontPoint, size, tribal.transform.eulerAngles.z, layerMask) != null;
        }

        public static bool IsTouchingFloorWalkable(this ITribal tribal)
        {
            return IsTouchingCeilingOrFloorLayer(tribal, VerticalDirection.Down, Layers.Walkable);
        }

        public static bool IsTouchingCeilingWalkable(this ITribal tribal)
        {
            return IsTouchingCeilingOrFloorLayer(tribal, VerticalDirection.Up, Layers.Walkable);
        }

        private static bool IsTouchingCeilingOrFloorLayer(ITribal tribal, VerticalDirection direction, int layerMask)
        {
            Bounds bounds = tribal.Collider.bounds;
            float heightExtents = 0.05f;
            float widthNegativeOffset = 0.1f;

            var upperOrLowerPoint = new Vector2(bounds.center.x, bounds.center.y + (bounds.extents.y + heightExtents) * (int)direction);
            var size = new Vector2(bounds.size.x - widthNegativeOffset, heightExtents * 2);

            return Physics2D.OverlapBox(upperOrLowerPoint, size, tribal.transform.eulerAngles.z, layerMask) != null;
        }

        public static IMovableObject SearchMovableObjects(this ITribal tribal)
        {
            float checkMovableObjectDistanceX = 0.2f;

            int facingDirection = (int)tribal.FacingDirection;

            var bounds = tribal.Collider.bounds;

            var center = new Vector2(bounds.center.x + (bounds.extents.x * facingDirection), bounds.center.y - bounds.extents.y / 3);
            var size = new Vector2(checkMovableObjectDistanceX, bounds.extents.y);

            return SJUtil.FindMovableObject(center, size, tribal.transform.eulerAngles.z);
        }

        public static bool IsInsideVelocityDeadZoneOnHorizontalAxis(this ITribal tribal, float velocityDeadZone)
        {
            var velocity = tribal.RigidBody2D.velocity.x;

            return velocity > velocityDeadZone * -1 && velocity < velocityDeadZone;
        }

        public static bool IsInsideVelocityDeadZoneOnVerticalAxis(this ITribal tribal, float velocityDeadZone)
        {
            var velocity = tribal.RigidBody2D.velocity.y;

            return velocity > velocityDeadZone * -1 && velocity < velocityDeadZone;
        }
    }
}