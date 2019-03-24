using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveImage : MonoBehaviour
{
    public GameObject wall1, wall2, wall3, wall4, player;
    // Start is called before the first frame update
    void Start()
    {
        wall1 = GameObject.Find("Plane") as GameObject;
        LoadWallImages("Room_1");
        player = GameObject.Find("player") as GameObject;
        LoadPlayerPosition(player);
        //to be implemented, walls 2-4
    }

    public static void LoadPlayerPosition(GameObject player) {

    }

    public static void SavePlayerPosition(GameObject player, string filename) {
        filename = "Room_1_Player"; //Testing Line, name will not be hard coded in full version
        //FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
        filename = System.IO.Directory.GetCurrentDirectory() + "/Assets/Resources/" + filename;
        StreamWriter sw = new StreamWriter(filename);

    }
    
    public static void SaveWallTextureAsPNG(GameObject wall, string filename) {
        SaveTextureToFile(GetWallTexture(wall), filename);
        //must pass first argument as Texture2D
    }

    public static Texture2D GetWallTexture(GameObject wall) {
        return wall.GetComponent<Renderer>().material.mainTexture as Texture2D;
    }

    public static void SaveTextureToFile(Texture2D texture, string filename)
    {
        filename = System.IO.Directory.GetCurrentDirectory() + "/Assets/Resources/" + filename;
        System.IO.File.WriteAllBytes(filename, texture.EncodeToPNG());
    }

    Texture2D load_s01_texture;
   //https://answers.unity.com/questions/858245/save-and-load-texture-with-systemio-filestream.html
    public Texture2D LoadTextureFromFile(string filename)
    {
        Texture2D textureToBeReturned;
        byte[] bytes;
        bytes = System.IO.File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + "/Assets/Resources/" + filename);
        textureToBeReturned = new Texture2D(1, 1);
        textureToBeReturned.LoadImage(bytes);
        return textureToBeReturned;
    }

        // Update is called once per frame
        void Update()
    {
        LoadWallImages("Room_1");
    }

    void LoadWallImages(string RoomName) {

        wall1.GetComponent<Renderer>().material.mainTexture = LoadTextureFromFile(RoomName + "_1.png");
        //wall1.GetComponent<Renderer>().material.mainTexture = Resources.Load(RoomName + "_1.png") as Texture2D;
        //to be implemented: walls 2-4
    }
}
