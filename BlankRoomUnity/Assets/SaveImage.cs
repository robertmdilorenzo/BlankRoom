using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SaveImage : MonoBehaviour
{

    //mycanvas.transform.Find("name").gameObject.SetActive(true);

    public GameObject wall1, wall2, wall3, wall4, player, MainPanel, CreateRoomPanel, SaveCopyPanel, LoadRoomPanel, DeleteRoomPanel;
    public string currentRoomName;
    public GameObject canvas, loadButton, deleteButton, loadButtonText, deleteButtonText;
    public string[] currentRoomNameList;
    private int RoomListIter;

    // Start is called before the first frame update
    void Start()
    {
        currentRoomName = "Room_1";

        wall1 = GameObject.Find("Plane") as GameObject;
        wall2 = GameObject.Find("Plane2") as GameObject;
        wall3 = GameObject.Find("Plane3") as GameObject;
        wall4 = GameObject.Find("Plane4") as GameObject;
        player = GameObject.Find("player") as GameObject;
        MainPanel = canvas.transform.Find("MainPanel").gameObject;
        CreateRoomPanel = canvas.transform.Find("Create Room Panel").gameObject;
        SaveCopyPanel = canvas.transform.Find("Save Copy Panel").gameObject;
        LoadRoomPanel = canvas.transform.Find("Load Room Panel").gameObject;
        DeleteRoomPanel = canvas.transform.Find("Delete Room Panel").gameObject;
        loadButton = LoadRoomPanel.transform.Find("LoadButton").gameObject;
        loadButtonText = loadButton.transform.Find("Text").gameObject;
        deleteButton = DeleteRoomPanel.transform.Find("DeleteButton").gameObject;
        deleteButtonText = deleteButton.transform.Find("Text").gameObject;
        LoadWallImages("Room_1");
        
        
        GetRoomNames();//testing
        
    }

    public void LoadCreateNewRoomPanel()
    {
        MainPanel.SetActive(false);
        CreateRoomPanel.SetActive(true);
    }

    public void LoadSaveCopyPanel()
    {
        MainPanel.SetActive(false);
        SaveCopyPanel.SetActive(true);
    }

    public void LoadDeleteRoomPanel()
    {
        currentRoomNameList = gameObject.GetComponent<SaveImage>().GetRoomNames();
        RoomListIter = 0;
        MainPanel.SetActive(false);
        DeleteRoomPanel.SetActive(true);
        if (currentRoomNameList.Length < 1)
        {
            deleteButtonText.GetComponent<Text>().text = "No Rooms";
        }
        else
        {
            deleteButtonText.GetComponent<Text>().text = currentRoomNameList[0];
        }
    }

    public void LoadLoadRoomPanel()
    {
        currentRoomNameList = gameObject.GetComponent<SaveImage>().GetRoomNames();
        RoomListIter = 0;
        MainPanel.SetActive(false);
        LoadRoomPanel.SetActive(true);
        if (currentRoomNameList.Length < 1)
        {
            loadButtonText.GetComponent<Text>().text = "No Rooms";
        }
        else
        {
            loadButtonText.GetComponent<Text>().text = currentRoomNameList[0];
        }

    }

    public void BackToMainPanel()
    {
        MainPanel.SetActive(true);
        CreateRoomPanel.SetActive(false);
        LoadRoomPanel.SetActive(false);
        DeleteRoomPanel.SetActive(false);
        SaveCopyPanel.SetActive(false);

    }

    public void StartMenu()
    {
        MainPanel.SetActive(true);
    }

    public void CreateRoomOnClick()
    {
        string input_string = CreateRoomPanel.transform.Find("Input").Find("Text").gameObject.GetComponent<Text>().text;
        if (input_string.Equals(""))
        {
            CreateRoomPanel.transform.Find("ErrorText").gameObject.GetComponent<Text>().text = "Input a Name";
            return;
        }
        else if (RoomNameExists(input_string))
        {
            CreateRoomPanel.transform.Find("ErrorText").gameObject.GetComponent<Text>().text = "Name Exists, Choose Another";
            return;
        }
        else
        {
            CreateNewRoom(input_string);
            gameObject.GetComponent<SaveImage>().BackToMainPanel();
            return;
        }
    }

    public void SaveAsOnClick()
    {
        string input_string = SaveCopyPanel.transform.Find("Input").Find("Text").gameObject.GetComponent<Text>().text;
        if (input_string.Equals(""))
        {
            SaveCopyPanel.transform.Find("ErrorText").gameObject.GetComponent<Text>().text = "Input a Name";
            return;
        }
        else if (RoomNameExists(input_string))
        {
            SaveCopyPanel.transform.Find("ErrorText").gameObject.GetComponent<Text>().text = "Name Exists, Choose Another";
            return;
        }
        else
        {
            SaveAllData(input_string);
            gameObject.GetComponent<SaveImage>().BackToMainPanel();
            return;
        }
    }

    public void SaveRoomOnClick()
    {
        gameObject.GetComponent<SaveImage>().SaveAllData(currentRoomName);
    }

    public void LoadButtonOnClick()
    {
        if (currentRoomNameList.Length < 1) return;

        gameObject.GetComponent<SaveImage>().LoadAllData(loadButtonText.GetComponent<Text>().text);
        gameObject.GetComponent<SaveImage>().BackToMainPanel();

    }

    public void LoadUpButtonOnClick()
    {
        if(RoomListIter == 0)
        {
            return;
        }
        else
        {
            loadButtonText.GetComponent<Text>().text = currentRoomNameList[--RoomListIter];
        }
    }

    public void LoadDownButtonOnClick()
    {
        if(RoomListIter == currentRoomNameList.Length - 1)
        {
            return;
        }
        else
        {
            loadButtonText.GetComponent<Text>().text = currentRoomNameList[++RoomListIter];
        }
    }

    public void DeleteButtonOnClick()
    {
        if (currentRoomNameList.Length < 1) return;

        gameObject.GetComponent<SaveImage>().DeleteRoom(deleteButtonText.GetComponent<Text>().text);
        gameObject.GetComponent<SaveImage>().BackToMainPanel();
    }

    public void DeleteUpButtonOnClick()
    {
        if (RoomListIter == 0)
        {
            return;
        }
        else
        {
            deleteButtonText.GetComponent<Text>().text = currentRoomNameList[--RoomListIter];
        }
    }

    public void DeleteDownButtonOnClick()
    {
        if(RoomListIter == currentRoomNameList.Length - 1)
        {
            return;
        }
        else
        {
            deleteButtonText.GetComponent<Text>().text = currentRoomNameList[++RoomListIter];
        }
    }

    /*
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     */

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateNewRoom(string RoomName)
    {
        gameObject.GetComponent<SaveImage>().MakeWallsBlank();
        gameObject.GetComponent<SaveImage>().SaveAllData(RoomName);
    }

    public void SaveAllData(string RoomName) {
        gameObject.GetComponent<SaveImage>().SavePlayerPosition(player, RoomName);
        gameObject.GetComponent<SaveImage>().SaveAllWalls(RoomName);
        currentRoomName = RoomName;
    }
    
    public void LoadAllData(string RoomName)
    {
        gameObject.GetComponent<SaveImage>().LoadPlayerPosition(player, RoomName);
        gameObject.GetComponent<SaveImage>().LoadWallImages(RoomName);
        currentRoomName = RoomName;
    }

    public string[] GetRoomNames()
    {
        string curDirectory = System.IO.Directory.GetCurrentDirectory() + "/Assets/Resources";
        string[] fileNames = Directory.GetFiles(curDirectory, "*_Player.txt");
        Regex pattern = new Regex(".*Resources.{1}");
        for (int i = 0; i < fileNames.Length; i++)
        {
            //Remove .Player from file names
            fileNames[i] = fileNames[i].Replace("_Player.txt", "");
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

        Array.Sort(fileNames);
        return fileNames; 
    }

    public bool RoomNameExists(string RoomName)
    {
        string[] names = gameObject.GetComponent<SaveImage>().GetRoomNames();
        foreach (string name in names)
        {
            if (RoomName.Equals(name)) return true;
        }
        return false;
    }

    public void DeleteRoom(string RoomName)
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
        wallName = curDirectory + RoomName + "_Player.txt";
        wallNameMeta = wallName + ".meta";
        System.IO.File.Delete(@wallNameMeta);
        System.IO.File.Delete(@wallName);
    }

    public void LoadPlayerPosition(GameObject player, string filename) {
        filename = System.IO.Directory.GetCurrentDirectory() + "/Assets/Resources/" + filename+"_Player.txt";
        StreamReader sr = new StreamReader(filename);

        Vector3 pos = new Vector3(float.Parse(sr.ReadLine()), float.Parse(sr.ReadLine()), float.Parse(sr.ReadLine()));
        player.transform.position = pos;
  
        Vector3 rot = new Vector3(float.Parse(sr.ReadLine()), float.Parse(sr.ReadLine()), float.Parse(sr.ReadLine()));
        player.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z); ;

        sr.Close();
    }

    public void SavePlayerPosition(GameObject player, string filename) {
        filename = System.IO.Directory.GetCurrentDirectory() + "/Assets/Resources/" + filename+"_Player.txt";
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

    public void SaveAllWalls(string filename) {
        gameObject.GetComponent<SaveImage>().SaveWallTextureAsPNG(wall1, filename+"_1.png");
        gameObject.GetComponent<SaveImage>().SaveWallTextureAsPNG(wall2, filename+"_2.png");
        gameObject.GetComponent<SaveImage>().SaveWallTextureAsPNG(wall3, filename+"_3.png");
        gameObject.GetComponent<SaveImage>().SaveWallTextureAsPNG(wall4, filename+"_4.png");
    }

    public void SaveWallTextureAsPNG(GameObject wall, string filename) {
        gameObject.GetComponent<SaveImage>().SaveTextureToFile(GetWallTexture(wall), filename);
    }

    public Texture2D GetWallTexture(GameObject wall) {
        return wall.GetComponent<Renderer>().material.mainTexture as Texture2D;
    }

    public void SaveTextureToFile(Texture2D texture, string filename)
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

    public void LoadWallImages(string RoomName) {

        wall1.GetComponent<Renderer>().material.mainTexture = gameObject.GetComponent<SaveImage>().LoadTextureFromFile(RoomName + "_1.png");
        wall2.GetComponent<Renderer>().material.mainTexture = gameObject.GetComponent<SaveImage>().LoadTextureFromFile(RoomName + "_2.png");
        wall3.GetComponent<Renderer>().material.mainTexture = gameObject.GetComponent<SaveImage>().LoadTextureFromFile(RoomName + "_3.png");
        wall4.GetComponent<Renderer>().material.mainTexture = gameObject.GetComponent<SaveImage>().LoadTextureFromFile(RoomName + "_4.png");
    }

    public void MakeWallsBlank()
    {
        wall1.GetComponent<Renderer>().material.mainTexture = gameObject.GetComponent<SaveImage>().LoadTextureFromFile("BlankWallTexture.png");
        wall2.GetComponent<Renderer>().material.mainTexture = gameObject.GetComponent<SaveImage>().LoadTextureFromFile("BlankWallTexture.png");
        wall3.GetComponent<Renderer>().material.mainTexture = gameObject.GetComponent<SaveImage>().LoadTextureFromFile("BlankWallTexture.png");
        wall4.GetComponent<Renderer>().material.mainTexture = gameObject.GetComponent<SaveImage>().LoadTextureFromFile("BlankWallTexture.png");
    }
}
