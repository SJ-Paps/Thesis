using System;
using System.IO;
using UnityEngine;

public static class Reg
{
    public static readonly string ASSETBUNDLE_DIRECTORY = Path.GetFullPath(Path.Combine(Application.dataPath, "../AssetBundles"));
    public static readonly string APPLICATION_INFO_ASSET_NAME = "ApplicationSettings";
    public static readonly string LANGUAGE_INFO_ASSET_NAME = "language_info";

#if UNITY_EDITOR
    public static readonly string buildPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Application.productName), Application.productName + ".exe");
    public static readonly string assetBundleDataPathBuild = Path.GetFullPath(Path.Combine(buildPath, "../AssetBundles"));
    
#endif

    public static readonly int playerLayer = LayerMask.NameToLayer("Player");
    public static readonly int hostileLayer = LayerMask.NameToLayer("Hostile");
    public static readonly int nonHostileLayer = LayerMask.NameToLayer("NonHostile");
    public static readonly int hostileDeadlyLayer = LayerMask.NameToLayer("HostileDeadly");
    public static readonly int generalDeadlyLayer = LayerMask.NameToLayer("GeneralDeadly");
	public static readonly int floorLayer = LayerMask.NameToLayer("Floor");
    public static readonly int activableObjectLayer = LayerMask.NameToLayer("ActivableObject");
    public static readonly int movableObjectLayer = LayerMask.NameToLayer("MovableObject");
    public static readonly int playerDetectionLayer = LayerMask.NameToLayer("PlayerDetection");
    public static readonly int itemLayer = LayerMask.NameToLayer("Item");
    public static readonly int hiddenLayer = LayerMask.NameToLayer("Hidden");

    public static readonly int walkableLayerMask = (1 << floorLayer) | (1 << movableObjectLayer);
    public static readonly int activableLayerMask = (1 << movableObjectLayer) | (1 << activableObjectLayer);

    
    
}