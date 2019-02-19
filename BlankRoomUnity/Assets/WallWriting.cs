using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWriting : MonoBehaviour
{
    // Start is called before the first frame update
    public RenderTexture canvasTexture;
    public Material baseMaterial;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MergeTexture();
        }
    }
    private void OnMouseDown()
    {
        MergeTexture();
    }
    private void OnMouseUp()
    {
        MergeTexture();
    }

    void MergeTexture()
    {
        Debug.Log("Merging...");
        RenderTexture.active = canvasTexture;
        int width = canvasTexture.width;
        int height = canvasTexture.height;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;
        baseMaterial.mainTexture = tex;
        Debug.Log("Merge Complete!");
    }

    void SaveTexture()
    {

    }
}
