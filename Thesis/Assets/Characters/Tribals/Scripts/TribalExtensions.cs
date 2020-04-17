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

        public static bool IsTouchingWalkableWall(this ITribal tribal, HorizontalDirection faceDirection)
        {
            return IsTouchingWallLayer(tribal, faceDirection, Layers.Walkable);
        }

        private static bool IsTouchingWallLayer(ITribal tribal, HorizontalDirection faceDirection, int layerMask)
        {
            int direction = (int)faceDirection;

            Bounds bounds = tribal.Collider.bounds;
            float lineSeparation = 0.2f;

            var upperBeginPoint = new Vector2(bounds.center.x + (lineSeparation + bounds.extents.x) * direction, bounds.center.y + bounds.extents.y);
            var lowerEndPoint = new Vector2(upperBeginPoint.x, bounds.center.y);

            Debug.DrawLine(upperBeginPoint, lowerEndPoint);

            return Physics2D.Linecast(upperBeginPoint, lowerEndPoint, layerMask);
        }

        public static bool IsTouchingWalkableFloor(this ITribal tribal)
        {
            return IsTouchingCeilingOrFloorLayer(tribal, VerticalDirection.Down, Layers.Walkable, heightExtents: 0.02f, widthNegativeOffset: 0.3f);
        }

        public static bool IsTouchingWalkableCeiling(this ITribal tribal)
        {
            return IsTouchingCeilingOrFloorLayer(tribal, VerticalDirection.Up, Layers.Walkable);
        }

        private static bool IsTouchingCeilingOrFloorLayer(ITribal tribal, VerticalDirection direction,
            int layerMask, float heightExtents = 0.05f, float widthNegativeOffset = 0.1f)
        {
            Bounds bounds = tribal.Collider.bounds;

            var upperOrLowerPoint = new Vector2(bounds.center.x, bounds.center.y + (bounds.extents.y + heightExtents) * (int)direction);
            var size = new Vector2(bounds.size.x - widthNegativeOffset, heightExtents * 2);

            return Physics2D.OverlapBox(upperOrLowerPoint, size, tribal.transform.eulerAngles.z, layerMask) != null;
        }

        public static IMovableObject SearchMovableObjects(this ITribal tribal)
        {
            float checkMovableObjectDistanceX = 0.2f;

            int facingDirection = (int)tribal.FacingDirection;

            var bounds = tribal.Collider.bounds;

            var center = new Vector2(bounds.center.x + (bounds.extents.x * facingDirection), bounds.center.y);
            var size = new Vector2(checkMovableObjectDistanceX, bounds.extents.y / 2);

            return SJUtil.FindMovableObject(center, size, tribal.transform.eulerAngles.z);
        }

        public static bool IsInsideVelocityDeadZoneOnHorizontalAxis(this ITribal tribal, float velocityDeadZone)
        {
            var velocity = tribal.RigidBody2D.Velocity.x;

            return velocity > velocityDeadZone * -1 && velocity < velocityDeadZone;
        }

        public static bool IsInsideVelocityDeadZoneOnVerticalAxis(this ITribal tribal, float velocityDeadZone)
        {
            var velocity = tribal.RigidBody2D.Velocity.y;

            return velocity > velocityDeadZone * -1 && velocity < velocityDeadZone;
        }
    }
}