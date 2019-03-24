﻿using System.Collections;
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
        LoadPlayerPosition(player, "Room_1_Player");
        //SavePlayerPosition(player, "Room_1_Player");
        //to be implemented, walls 2-4
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
