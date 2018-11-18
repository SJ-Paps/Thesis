using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
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
    private Color colorTransparency;

    [SerializeField]
    private float colorThreshold = 0.1f;

    [SerializeField]
    private bool flipX, flipY;

    [SerializeField]
    private float size = 2;

    private Texture2D prefabThumbnail;

    private float xPos, yPos;

    public void Spawn()
    {
        Instantiate(prefab, transform.position, prefab.transform.rotation);
    }

    void Update()
    {

        if (prefab != null)
        {
            prefabThumbnail = AssetPreview.GetAssetPreview(prefab.gameObject);
        }

        xPos = transform.position.x;
        yPos = transform.position.y;

    }

    void OnDrawGizmos()
    {
        if (prefabThumbnail != null)
        {
            if (prefabThumbnail != null)
            {
                float finalSizeX;
                float finalSizeY;

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

                Gizmos.DrawGUITexture(new Rect(xPos, yPos, finalSizeX, finalSizeY), prefabThumbnail);
                
            }
        }
    }
}