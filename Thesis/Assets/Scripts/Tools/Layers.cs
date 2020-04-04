using UnityEngine;

namespace SJ.Tools
{
    public static class Layers
    {
        public static readonly int Player = ToLayerMask("Player");
        public static readonly int Hostile = ToLayerMask("Hostile");
        public static readonly int NonHostile = ToLayerMask("NonHostile");
        public static readonly int HostileDeadly = ToLayerMask("HostileDeadly");
        public static readonly int GeneralDeadly = ToLayerMask("GeneralDeadly");
        public static readonly int Floor = ToLayerMask("Floor");
        public static readonly int ActivableObject = ToLayerMask("ActivableObject");
        public static readonly int MovableObject = ToLayerMask("MovableObject");
        public static readonly int PlayerDetection = ToLayerMask("PlayerDetection");
        public static readonly int Item = ToLayerMask("Item");
        public static readonly int Hidden = ToLayerMask("Hidden");

        public static readonly int Walkable = GroupLayers(Floor, MovableObject);

        private static int ToLayerMask(string name) => 1 << LayerMask.NameToLayer(name);
        private static int GroupLayers(params int[] layerMasks)
        {
            int resultLayerMask = layerMasks[0];

            for (int i = 1; i < layerMasks.Length; i++)
                resultLayerMask |= layerMasks[i];

            return resultLayerMask;
        }
    }
}