using UnityEngine;
using UnityEditor;

public class SpawnPoint : MonoBehaviour {

    [SerializeField]
    private SJMonoBehaviour prefab;

    public SJMonoBehaviour Prefab
    {
        get
        {
            return prefab;
        }
    }

    [SerializeField]
    private Material spriteDefault;

    [SerializeField]
    private Color colorTransparency;

    [SerializeField]
    private float colorTransparencyThreshold = 0.1f;

    [SerializeField]
    private bool flipX, flipY;

    [SerializeField]
    private int size = 2;

    private Texture2D prefabThumbnail;

    public void Spawn()
    {
        Instantiate(prefab, transform.position, prefab.transform.rotation);
    }

    void OnValidate()
    {
        prefabThumbnail = AssetPreview.GetAssetPreview(prefab.gameObject);
    }

    void OnDrawGizmos()
    {
        if(prefab != null)
        {
            if (prefabThumbnail != null)
            {
                int finalSizeX;
                int finalSizeY;

                if (flipX)
                {
                    finalSizeX = size;
                }
                else
                {
                    finalSizeX = -size;
                }

                if(flipY)
                {
                    finalSizeY = size;
                }
                else
                {
                    finalSizeY = -size;
                }

                if(spriteDefault == null)
                {
                    Gizmos.DrawGUITexture(new Rect(0, 0, finalSizeX, finalSizeY), prefabThumbnail);
                }
                else
                {
                    Gizmos.DrawGUITexture(new Rect(0, 0, finalSizeX, finalSizeY), prefabThumbnail, spriteDefault);
                }
                
            }
        }
    }
}