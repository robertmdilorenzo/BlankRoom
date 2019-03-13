using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveImage : MonoBehaviour
{
    public GameObject wall1, wall2, wall3, wall4;
    // Start is called before the first frame update
    void Start()
    {
        wall1 = GameObject.Find("Plane") as GameObject;
        //to be implemented, walls 2-4
    }

    public static void SaveTextureToFile(Texture2D texture, string filename)
    {
        filename = System.IO.Directory.GetCurrentDirectory() + "Resources/" + filename;
        System.IO.File.WriteAllBytes(filename, texture.EncodeToPNG());
    }

    Texture2D load_s01_texture;
   //https://answers.unity.com/questions/858245/save-and-load-texture-with-systemio-filestream.html
    public Texture2D LoadTextureFromFile(string filename)
    {
        Texture2D textureToBeReturned;
        byte[] bytes;
        bytes = System.IO.File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + "Resources/" + filename);
        textureToBeReturned = new Texture2D(1, 1);
        textureToBeReturned.LoadImage(bytes);
        return textureToBeReturned;
    }

        // Update is called once per frame
        void Update()
    {
        
    }

    void LoadWallImages(string RoomName) {
        wall1.GetComponent<Material>().mainTexture = Resources.Load(RoomName + "_1");
        //to be implemented: walls 2-4
    }
}
