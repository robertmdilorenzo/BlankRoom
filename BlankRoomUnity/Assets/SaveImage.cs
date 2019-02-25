using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveImage : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static void SaveTextureToFile(Texture2D texture, string filename)
    {
        System.IO.File.WriteAllBytes(filename, texture.EncodeToPNG());
    }

    https://answers.unity.com/questions/858245/save-and-load-texture-with-systemio-filestream.html
    Texture2D load_s01_texture;
    void LoadTextureToFile(string filename)
    {
        load_s01_texture = System.IO.File.ReadAllBytes(Application.dataPath + "/Save/" + filename);
    }

    https://answers.unity.com/questions/858245/save-and-load-texture-with-systemio-filestream.html
    void LoadTextureToFile(string filename)
    {
        byte[] bytes;
        bytes = System.IO.File.ReadAllBytes(Application.dataPath + "/Save/" + filename);
        load_s01_texture = new Texture2D(1, 1);
        load_s01_texture.LoadImage(bytes);
    }

        // Update is called once per frame
        void Update()
    {
        
    }
}
