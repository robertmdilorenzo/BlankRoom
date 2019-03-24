using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class SaveImage : MonoBehaviour
{
    public static GameObject wall1, wall2, wall3, wall4, player;
    // Start is called before the first frame update
    void Start()
    {
        wall1 = GameObject.Find("Plane") as GameObject;
        wall2 = GameObject.Find("Plane2") as GameObject;
        wall3 = GameObject.Find("Plane3") as GameObject;
        wall4 = GameObject.Find("Plane4") as GameObject;
        player = GameObject.Find("player") as GameObject;
        LoadWallImages("Room_1");
        //GetRoomNames();//testing
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //filename argument should be name of room
    public static void SaveAllData(string RoomName) {
        SavePlayerPosition(player, RoomName);
        SaveAllWalls(RoomName);
    }

    //filename argument should be name of room
    public static void LoadAllData(string RoomName)
    {
        LoadPlayerPosition(player, RoomName);
        LoadWallImages(RoomName);
    }

    public static string[] GetRoomNames()
    {
        string curDirectory = System.IO.Directory.GetCurrentDirectory() + "/Assets/Resources";
        string[] fileNames = Directory.GetFiles(curDirectory, "*_Player");
        Regex pattern = new Regex(".*Resources.{1}");
        for (int i = 0; i < fileNames.Length; i++)
        {
            //Remove .Player from file names
            fileNames[i] = fileNames[i].Replace("_Player", "");
            fileNames[i] = pattern.Replace(fileNames[i], "");
        }

        //testing
        StreamWriter sw = new StreamWriter(curDirectory + "/TestFileNames.txt");
        for (int i = 0; i < fileNames.Length; i++)
        {
            sw.Write(fileNames[i]);
        }
        sw.Close();
        //end of testing

        return fileNames; 
    }

    public static void DeleteRoom(string RoomName)
    {
        string curDirectory = System.IO.Directory.GetCurrentDirectory() + "/Assets/Resources/";
        string wallName = curDirectory + RoomName + "_1.png";
        string wallNameMeta = wallName + ".meta";
        System.IO.File.Delete(@wallNameMeta);
        System.IO.File.Delete(@wallName);
        wallName = curDirectory + RoomName + "_2.png";
        wallNameMeta = wallName + ".meta";
        System.IO.File.Delete(@wallNameMeta);
        System.IO.File.Delete(@wallName);
        wallName = curDirectory + RoomName + "_3.png";
        wallNameMeta = wallName + ".meta";
        System.IO.File.Delete(@wallNameMeta);
        System.IO.File.Delete(@wallName);
        wallName = curDirectory + RoomName + "_4.png";
        wallNameMeta = wallName + ".meta";
        System.IO.File.Delete(@wallNameMeta);
        System.IO.File.Delete(@wallName);
        wallName = curDirectory + RoomName + "_Player";
        wallNameMeta = wallName + ".meta";
        System.IO.File.Delete(@wallNameMeta);
        System.IO.File.Delete(@wallName);
    }

    public static void LoadPlayerPosition(GameObject player, string filename) {
        filename = "Room_1_Player"; //name will not be hard coded in full version
        filename = System.IO.Directory.GetCurrentDirectory() + "/Assets/Resources/" + filename;
        StreamReader sr = new StreamReader(filename);

        Vector3 pos = new Vector3(float.Parse(sr.ReadLine()), float.Parse(sr.ReadLine()), float.Parse(sr.ReadLine()));
        player.transform.position = pos;
  
        Vector3 rot = new Vector3(float.Parse(sr.ReadLine()), float.Parse(sr.ReadLine()), float.Parse(sr.ReadLine()));
        player.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z); ;

        sr.Close();
    }

    public static void SavePlayerPosition(GameObject player, string filename) {
        filename = "Room_1_Player"; //Testing Line, name will not be hard coded in full version
        filename = System.IO.Directory.GetCurrentDirectory() + "/Assets/Resources/" + filename;
        StreamWriter sw = new StreamWriter(filename);
        Transform pt = player.transform;
        sw.Write(pt.position.x + "\n");
        sw.Write(pt.position.y + "\n");
        sw.Write(pt.position.z + "\n");
        sw.Write(pt.rotation.x + "\n");
        sw.Write(pt.rotation.y + "\n");
        sw.Write(pt.rotation.z + "\n");

        sw.Close();
    }

    public static void SaveAllWalls(string filename) {
        SaveWallTextureAsPNG(wall1, filename);
        SaveWallTextureAsPNG(wall2, filename);
        SaveWallTextureAsPNG(wall3, filename);
        SaveWallTextureAsPNG(wall4, filename);
    }

    public static void SaveWallTextureAsPNG(GameObject wall, string filename) {
        SaveTextureToFile(GetWallTexture(wall), filename+".png");
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
    public static Texture2D LoadTextureFromFile(string filename)
    {
        Texture2D textureToBeReturned;
        byte[] bytes;
        bytes = System.IO.File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + "/Assets/Resources/" + filename);
        textureToBeReturned = new Texture2D(1, 1);
        textureToBeReturned.LoadImage(bytes);
        return textureToBeReturned;
    }

    public static void LoadWallImages(string RoomName) {

        wall1.GetComponent<Renderer>().material.mainTexture = LoadTextureFromFile(RoomName + "_1.png");
        wall2.GetComponent<Renderer>().material.mainTexture = LoadTextureFromFile(RoomName + "_2.png");
        wall3.GetComponent<Renderer>().material.mainTexture = LoadTextureFromFile(RoomName + "_3.png");
        wall4.GetComponent<Renderer>().material.mainTexture = LoadTextureFromFile(RoomName + "_4.png");
    }

    public static void MakeWallsBlank()
    {
        wall1.GetComponent<Renderer>().material.mainTexture = LoadTextureFromFile("BlankWallTexture.png");
        wall2.GetComponent<Renderer>().material.mainTexture = LoadTextureFromFile("BlankWallTexture.png");
        wall3.GetComponent<Renderer>().material.mainTexture = LoadTextureFromFile("BlankWallTexture.png");
        wall4.GetComponent<Renderer>().material.mainTexture = LoadTextureFromFile("BlankWallTexture.png");
    }
}
