using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gvr;

public class WallWriting : MonoBehaviour
{
    // Start is called before the first frame update
    public RenderTexture canvasTexture;
    public Material baseMaterial;
    public GameObject penMarkContainer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GvrControllerInput.ClickButtonUp)
        {

            //StartCoroutine(SaveTextureFile(MergeTexture()));
            MergeTexture();
        }
    }

    Texture2D MergeTexture()
    {

        //Debug.Log("Merging...");
        RenderTexture.active = canvasTexture;
        int width = canvasTexture.width;
        int height = canvasTexture.height;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        Debug.Log(tex.ToString());
        RenderTexture.active = null;
        baseMaterial.mainTexture = tex;
        //Debug.Log("Merge Complete!");

        for(int i = 0; i < penMarkContainer.transform.childCount; i++)
        {
            Destroy(penMarkContainer.transform.GetChild(i).gameObject);
        }
        return tex;
        
    }

    IEnumerator SaveTextureFile(Texture2D savedTexture)
    {
        string fullPath = System.IO.Directory.GetCurrentDirectory();
        System.DateTime date = System.DateTime.Now;
        string fileName = "CanvasTexture.png";
        if (!System.IO.Directory.Exists(fullPath))
        {
            System.IO.Directory.CreateDirectory(fullPath);
        }
        var bytes = savedTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(fullPath + fileName, bytes);
        yield return null;
    }
}
