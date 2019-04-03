using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SaveImage : MonoBehaviour
{
    public GameObject wall1, wall2, wall3, wall4, player, MainPanel, CreateRoomPanel, SaveCopyPanel, LoadRoomPanel, DeleteRoomPanel;
    public string currentRoomName;
    public GameObject canvas, loadButton, deleteButton, loadButtonText, deleteButtonText, saveCopyText, createText, UICamera, MainCamera;
    public string[] currentRoomNameList;
    private GameObject[] scLetters, createLetters;
    private int RoomListIter;
    private int UI_DISTANCE_FROM_WALL = 4;
    private Boolean shift;

    // Start is called before the first frame update
    void Start()
    {
        currentRoomName = "%%%%%%%%%%%%%%%%%%%%";
        RoomListIter = 0;
        shift = false;
        wall1 = GameObject.Find("Plane") as GameObject;
        //wall2 = GameObject.Find("Plane2") as GameObject;
        //wall3 = GameObject.Find("Plane3") as GameObject;
        //wall4 = GameObject.Find("Plane4") as GameObject;
        player = GameObject.Find("player") as GameObject;
        MainCamera = GameObject.Find("Main Camera") as GameObject;
        //UICamera = GameObject.Find("UI Camera") as GameObject;
        //UICamera.SetActive(false);
        MainPanel = canvas.transform.Find("MainPanel").gameObject;
        CreateRoomPanel = canvas.transform.Find("Create Room Panel").gameObject;
        createText = CreateRoomPanel.transform.Find("Input").gameObject;
        SaveCopyPanel = canvas.transform.Find("Save Copy Panel").gameObject;
        saveCopyText = SaveCopyPanel.transform.Find("Input").gameObject;
        LoadRoomPanel = canvas.transform.Find("Load Room Panel").gameObject;
        DeleteRoomPanel = canvas.transform.Find("Delete Room Panel").gameObject;
        loadButton = LoadRoomPanel.transform.Find("LoadButton").gameObject;
        loadButtonText = loadButton.transform.Find("Text").gameObject;
        deleteButton = DeleteRoomPanel.transform.Find("DeleteButton").gameObject;
        deleteButtonText = deleteButton.transform.Find("Text").gameObject;
        FindLettersForKeyboards();
        gameObject.GetComponent<SaveImage>().MakeWallsBlank();


    }

    public void LoadCreateNewRoomPanel()
    {
        MainPanel.SetActive(false);
        CreateRoomPanel.SetActive(true);
        CreateRoomPanel.transform.Find("ErrorText").gameObject.GetComponent<Text>().text = "";
    }

    public void LoadSaveCopyPanel()
    {
        MainPanel.SetActive(false);
        SaveCopyPanel.SetActive(true);
        SaveCopyPanel.transform.Find("ErrorText").gameObject.GetComponent<Text>().text = "";
    }

    public void LoadDeleteRoomPanel()
    {
        currentRoomNameList = gameObject.GetComponent<SaveImage>().GetRoomNames();
        gameObject.GetComponent<SaveImage>().ValidateRoomListIter();
        MainPanel.SetActive(false);
        DeleteRoomPanel.SetActive(true);
        if (currentRoomNameList.Length < 1)
        {
            deleteButtonText.GetComponent<Text>().text = "No Rooms";
        }
        else
        {
            deleteButtonText.GetComponent<Text>().text = currentRoomNameList[RoomListIter];
        }
    }

    public void LoadLoadRoomPanel()
    {
        currentRoomNameList = gameObject.GetComponent<SaveImage>().GetRoomNames();
        gameObject.GetComponent<SaveImage>().ValidateRoomListIter();
        MainPanel.SetActive(false);
        LoadRoomPanel.SetActive(true);
        if (currentRoomNameList.Length < 1)
        {
            loadButtonText.GetComponent<Text>().text = "No Rooms";
        }
        else
        {
            loadButtonText.GetComponent<Text>().text = currentRoomNameList[RoomListIter];
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
        if(canvas.activeSelf) //Menu is already open
        {
            BackToMainPanel();
            canvas.SetActive(false);
            
        }
        else //menu is closed
        {
            BackToMainPanel();
            canvas.SetActive(true);
            Vector3 rot = new Vector3(MainCamera.transform.rotation.x, MainCamera.transform.rotation.y, MainCamera.transform.rotation.z);

            canvas.GetComponent<RectTransform>().position = new Vector3(wall1.transform.position.x, wall1.transform.position.y, wall1.transform.position.z - UI_DISTANCE_FROM_WALL);
            canvas.GetComponent<RectTransform>().rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }
    }

    public void CreateRoomOnClick()
    {
        string input_string = createText.GetComponent<Text>().text;
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
        string input_string = saveCopyText.gameObject.GetComponent<Text>().text;
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
        if (currentRoomName.Equals("%%%%%%%%%%%%%%%%%%%%"))
        {
            gameObject.GetComponent<SaveImage>().LoadSaveCopyPanel();
            return;
        }
        gameObject.GetComponent<SaveImage>().SaveAllData(currentRoomName);
    }

    private void ValidateRoomListIter()
    {
        if (RoomListIter < 0 || RoomListIter >= currentRoomNameList.Length) RoomListIter = 0;
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
        if (Input.GetKeyDown("space"))
        {
            StartMenu();
        }
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
        /*StreamWriter sw = new StreamWriter(curDirectory + "/TestFileNames.txt");
        for (int i = 0; i < fileNames.Length; i++)
        {
            sw.Write(fileNames[i]);
        }
        sw.Close();*/
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
        player.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z); 

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
        //gameObject.GetComponent<SaveImage>().SaveWallTextureAsPNG(wall2, filename+"_2.png");
        //gameObject.GetComponent<SaveImage>().SaveWallTextureAsPNG(wall3, filename+"_3.png");
        //gameObject.GetComponent<SaveImage>().SaveWallTextureAsPNG(wall4, filename+"_4.png");
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
        //wall2.GetComponent<Renderer>().material.mainTexture = gameObject.GetComponent<SaveImage>().LoadTextureFromFile(RoomName + "_2.png");
        //wall3.GetComponent<Renderer>().material.mainTexture = gameObject.GetComponent<SaveImage>().LoadTextureFromFile(RoomName + "_3.png");
        //wall4.GetComponent<Renderer>().material.mainTexture = gameObject.GetComponent<SaveImage>().LoadTextureFromFile(RoomName + "_4.png");
    }

    public void MakeWallsBlank()
    {
        wall1.GetComponent<Renderer>().material.mainTexture = gameObject.GetComponent<SaveImage>().LoadTextureFromFile("BlankWallTexture.png");
        //wall2.GetComponent<Renderer>().material.mainTexture = gameObject.GetComponent<SaveImage>().LoadTextureFromFile("BlankWallTexture.png");
       // wall3.GetComponent<Renderer>().material.mainTexture = gameObject.GetComponent<SaveImage>().LoadTextureFromFile("BlankWallTexture.png");
        //wall4.GetComponent<Renderer>().material.mainTexture = gameObject.GetComponent<SaveImage>().LoadTextureFromFile("BlankWallTexture.png");
    }

    public void scBackspaceOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        else if (saveCopyText.GetComponent<Text>().text.Equals(""))
        {
            return;
        }
        else
        {
            string cur = saveCopyText.GetComponent<Text>().text;
            saveCopyText.GetComponent<Text>().text = cur.Substring(0, cur.Length - 1);
        }
    }

    public void scShiftOnClick()
    {
        if (shift) shift = false; else shift = true;
        if (scLetters[0].GetComponent<Text>().text.Equals("A"))
        {
            SetAllLowercase(scLetters);
        }
        else
        {
            SetAllUppercase(scLetters);
        }
    }

    public void scAOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "A";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "a";
        }
    }

    public void scBOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "B";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "b";
        }
    }

    public void scCOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "C";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "c";
        }
    }
    public void scDOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "D";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "d";
        }
    }
    public void scEOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "E";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "e";
        }
    }

    public void scFOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "F";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "f";
        }
    }

    public void scGOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "G";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "g";
        }
    }

    public void scHOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "H";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "h";
        }
    }

    public void scIOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "I";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "i";
        }
    }

    public void scJOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "J";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "j";
        }
    }

    public void scKOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "K";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "k";
        }
    }

    public void scLOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "L";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "l";
        }
    }

    public void scMOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "M";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "m";
        }
    }

    public void scNOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "N";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "n";
        }
    }

    public void scOOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "O";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "o";
        }
    }

    public void scPOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "P";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "p";
        }
    }

    public void scQOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "Q";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "q";
        }
    }

    public void scROnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "R";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "r";
        }
    }

    public void scSOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "S";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "s";
        }
    }

    public void scTOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "T";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "t";
        }
    }

    public void scUOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "U";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "u";
        }
    }

    public void scVOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "V";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "v";
        }
    }

    public void scWOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "W";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "w";
        }
    }

    public void scXOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "X";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "x";
        }
    }

    public void scYOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "Y";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "y";
        }
    }

    public void scZOnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "Z";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "z";
        }
    }

    public void sc1OnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "!";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "1";
        }
    }

    public void sc2OnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "@";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "2";
        }
    }

    public void sc3OnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "#";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "3";
        }
    }

    public void sc4OnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "$";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "4";
        }
    }

    public void sc5OnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "%";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "5";
        }
    }

    public void sc6OnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "^";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "6";
        }
    }

    public void sc7OnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "&";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "7";
        }
    }

    public void sc8OnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "*";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "8";
        }
    }

    public void sc9OnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += "(";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "9";
        }
    }

    public void sc0OnClick()
    {
        if (saveCopyText.GetComponent<Text>().text == null)
        {
            saveCopyText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            saveCopyText.GetComponent<Text>().text += ")";
        }
        else
        {
            saveCopyText.GetComponent<Text>().text += "0";
        }
    }

    public void CreateBackspaceOnClick()
    {
        if(createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        else if (createText.GetComponent<Text>().text.Equals(""))
        {
            return;
        }
        else
        {
            string cur = createText.GetComponent<Text>().text;
            createText.GetComponent<Text>().text = cur.Substring(0, cur.Length - 1);
        }
    }

    public void CreateShiftOnClick()
    {
        if (shift) shift = false; else shift = true;
        if (createLetters[0].GetComponent<Text>().text.Equals("A"))
        {
            SetAllLowercase(createLetters);
        }
        else
        {
            SetAllUppercase(createLetters);
        }
    }

    public void CreateAOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "A";
        }
        else
        {
            createText.GetComponent<Text>().text += "a";
        }
    }

    public void CreateBOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "B";
        }
        else
        {
            createText.GetComponent<Text>().text += "b";
        }
    }

    public void CreateCOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "C";
        }
        else
        {
            createText.GetComponent<Text>().text += "c";
        }
    }
    public void CreateDOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "D";
        }
        else
        {
            createText.GetComponent<Text>().text += "d";
        }
    }
    public void CreateEOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "E";
        }
        else
        {
            createText.GetComponent<Text>().text += "e";
        }
    }

    public void CreateFOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "F";
        }
        else
        {
            createText.GetComponent<Text>().text += "f";
        }
    }

    public void CreateGOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "G";
        }
        else
        {
            createText.GetComponent<Text>().text += "g";
        }
    }

    public void CreateHOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "H";
        }
        else
        {
            createText.GetComponent<Text>().text += "h";
        }
    }

    public void CreateIOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "I";
        }
        else
        {
            createText.GetComponent<Text>().text += "i";
        }
    }

    public void CreateJOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "J";
        }
        else
        {
            createText.GetComponent<Text>().text += "j";
        }
    }

    public void CreateKOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "K";
        }
        else
        {
            createText.GetComponent<Text>().text += "k";
        }
    }

    public void CreateLOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "L";
        }
        else
        {
            createText.GetComponent<Text>().text += "l";
        }
    }

    public void CreateMOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "M";
        }
        else
        {
            createText.GetComponent<Text>().text += "m";
        }
    }

    public void CreateNOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "N";
        }
        else
        {
            createText.GetComponent<Text>().text += "n";
        }
    }

    public void CreateOOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "O";
        }
        else
        {
            createText.GetComponent<Text>().text += "o";
        }
    }

    public void CreatePOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "P";
        }
        else
        {
            createText.GetComponent<Text>().text += "p";
        }
    }

    public void CreateQOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "Q";
        }
        else
        {
            createText.GetComponent<Text>().text += "q";
        }
    }

    public void CreateROnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "R";
        }
        else
        {
            createText.GetComponent<Text>().text += "r";
        }
    }

    public void CreateSOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "S";
        }
        else
        {
            createText.GetComponent<Text>().text += "s";
        }
    }

    public void CreateTOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "T";
        }
        else
        {
            createText.GetComponent<Text>().text += "t";
        }
    }

    public void CreateUOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "U";
        }
        else
        {
            createText.GetComponent<Text>().text += "u";
        }
    }

    public void CreateVOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "V";
        }
        else
        {
            createText.GetComponent<Text>().text += "v";
        }
    }

    public void CreateWOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "W";
        }
        else
        {
            createText.GetComponent<Text>().text += "w";
        }
    }

    public void CreateXOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "X";
        }
        else
        {
            createText.GetComponent<Text>().text += "x";
        }
    }

    public void CreateYOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "Y";
        }
        else
        {
            createText.GetComponent<Text>().text += "y";
        }
    }

    public void CreateZOnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "Z";
        }
        else
        {
            createText.GetComponent<Text>().text += "z";
        }
    }

    public void Create1OnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "!";
        }
        else
        {
            createText.GetComponent<Text>().text += "1";
        }
    }

    public void Create2OnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "@";
        }
        else
        {
            createText.GetComponent<Text>().text += "2";
        }
    }

    public void Create3OnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "#";
        }
        else
        {
            createText.GetComponent<Text>().text += "3";
        }
    }

    public void Create4OnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "$";
        }
        else
        {
            createText.GetComponent<Text>().text += "4";
        }
    }

    public void Create5OnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "%";
        }
        else
        {
            createText.GetComponent<Text>().text += "5";
        }
    }

    public void Create6OnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "^";
        }
        else
        {
            createText.GetComponent<Text>().text += "6";
        }
    }

    public void Create7OnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "&";
        }
        else
        {
            createText.GetComponent<Text>().text += "7";
        }
    }

    public void Create8OnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "*";
        }
        else
        {
            createText.GetComponent<Text>().text += "8";
        }
    }

    public void Create9OnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += "(";
        }
        else
        {
            createText.GetComponent<Text>().text += "9";
        }
    }

    public void Create0OnClick()
    {
        if (createText.GetComponent<Text>().text == null)
        {
            createText.GetComponent<Text>().text = "";
        }
        if (shift)
        {
            createText.GetComponent<Text>().text += ")";
        }
        else
        {
            createText.GetComponent<Text>().text += "0";
        }
    }

    private void FindLettersForKeyboards()
    {
        scLetters = new GameObject[36];
        scLetters[0] = SaveCopyPanel.transform.Find("A").Find("Text").gameObject;
        scLetters[1] = SaveCopyPanel.transform.Find("B").Find("Text").gameObject;
        scLetters[2] = SaveCopyPanel.transform.Find("C").Find("Text").gameObject;
        scLetters[3] = SaveCopyPanel.transform.Find("D").Find("Text").gameObject;
        scLetters[4] = SaveCopyPanel.transform.Find("E").Find("Text").gameObject;
        scLetters[5] = SaveCopyPanel.transform.Find("F").Find("Text").gameObject;
        scLetters[6] = SaveCopyPanel.transform.Find("G").Find("Text").gameObject;
        scLetters[7] = SaveCopyPanel.transform.Find("H").Find("Text").gameObject;
        scLetters[8] = SaveCopyPanel.transform.Find("I").Find("Text").gameObject;
        scLetters[9] = SaveCopyPanel.transform.Find("J").Find("Text").gameObject;
        scLetters[10] = SaveCopyPanel.transform.Find("K").Find("Text").gameObject;
        scLetters[11] = SaveCopyPanel.transform.Find("L").Find("Text").gameObject;
        scLetters[12] = SaveCopyPanel.transform.Find("M").Find("Text").gameObject;
        scLetters[13] = SaveCopyPanel.transform.Find("N").Find("Text").gameObject;
        scLetters[14] = SaveCopyPanel.transform.Find("O").Find("Text").gameObject;
        scLetters[15] = SaveCopyPanel.transform.Find("P").Find("Text").gameObject;
        scLetters[16] = SaveCopyPanel.transform.Find("Q").Find("Text").gameObject;
        scLetters[17] = SaveCopyPanel.transform.Find("R").Find("Text").gameObject;
        scLetters[18] = SaveCopyPanel.transform.Find("S").Find("Text").gameObject;
        scLetters[19] = SaveCopyPanel.transform.Find("T").Find("Text").gameObject;
        scLetters[20] = SaveCopyPanel.transform.Find("U").Find("Text").gameObject;
        scLetters[21] = SaveCopyPanel.transform.Find("V").Find("Text").gameObject;
        scLetters[22] = SaveCopyPanel.transform.Find("W").Find("Text").gameObject;
        scLetters[23] = SaveCopyPanel.transform.Find("X").Find("Text").gameObject;
        scLetters[24] = SaveCopyPanel.transform.Find("Y").Find("Text").gameObject;
        scLetters[25] = SaveCopyPanel.transform.Find("Z").Find("Text").gameObject;
        scLetters[26] = SaveCopyPanel.transform.Find("1").Find("Text").gameObject;
        scLetters[27] = SaveCopyPanel.transform.Find("2").Find("Text").gameObject;
        scLetters[28] = SaveCopyPanel.transform.Find("3").Find("Text").gameObject;
        scLetters[29] = SaveCopyPanel.transform.Find("4").Find("Text").gameObject;
        scLetters[30] = SaveCopyPanel.transform.Find("5").Find("Text").gameObject;
        scLetters[31] = SaveCopyPanel.transform.Find("6").Find("Text").gameObject;
        scLetters[32] = SaveCopyPanel.transform.Find("7").Find("Text").gameObject;
        scLetters[33] = SaveCopyPanel.transform.Find("8").Find("Text").gameObject;
        scLetters[34] = SaveCopyPanel.transform.Find("9").Find("Text").gameObject;
        scLetters[35] = SaveCopyPanel.transform.Find("0").Find("Text").gameObject;
        createLetters = new GameObject[36];
        createLetters[0] = CreateRoomPanel.transform.Find("A").Find("Text").gameObject;
        createLetters[1] = CreateRoomPanel.transform.Find("B").Find("Text").gameObject;
        createLetters[2] = CreateRoomPanel.transform.Find("C").Find("Text").gameObject;
        createLetters[3] = CreateRoomPanel.transform.Find("D").Find("Text").gameObject;
        createLetters[4] = CreateRoomPanel.transform.Find("E").Find("Text").gameObject;
        createLetters[5] = CreateRoomPanel.transform.Find("F").Find("Text").gameObject;
        createLetters[6] = CreateRoomPanel.transform.Find("G").Find("Text").gameObject;
        createLetters[7] = CreateRoomPanel.transform.Find("H").Find("Text").gameObject;
        createLetters[8] = CreateRoomPanel.transform.Find("I").Find("Text").gameObject;
        createLetters[9] = CreateRoomPanel.transform.Find("J").Find("Text").gameObject;
        createLetters[10] = CreateRoomPanel.transform.Find("K").Find("Text").gameObject;
        createLetters[11] = CreateRoomPanel.transform.Find("L").Find("Text").gameObject;
        createLetters[12] = CreateRoomPanel.transform.Find("M").Find("Text").gameObject;
        createLetters[13] = CreateRoomPanel.transform.Find("N").Find("Text").gameObject;
        createLetters[14] = CreateRoomPanel.transform.Find("O").Find("Text").gameObject;
        createLetters[15] = CreateRoomPanel.transform.Find("P").Find("Text").gameObject;
        createLetters[16] = CreateRoomPanel.transform.Find("Q").Find("Text").gameObject;
        createLetters[17] = CreateRoomPanel.transform.Find("R").Find("Text").gameObject;
        createLetters[18] = CreateRoomPanel.transform.Find("S").Find("Text").gameObject;
        createLetters[19] = CreateRoomPanel.transform.Find("T").Find("Text").gameObject;
        createLetters[20] = CreateRoomPanel.transform.Find("U").Find("Text").gameObject;
        createLetters[21] = CreateRoomPanel.transform.Find("V").Find("Text").gameObject;
        createLetters[22] = CreateRoomPanel.transform.Find("W").Find("Text").gameObject;
        createLetters[23] = CreateRoomPanel.transform.Find("X").Find("Text").gameObject;
        createLetters[24] = CreateRoomPanel.transform.Find("Y").Find("Text").gameObject;
        createLetters[25] = CreateRoomPanel.transform.Find("Z").Find("Text").gameObject;
        createLetters[26] = CreateRoomPanel.transform.Find("1").Find("Text").gameObject;
        createLetters[27] = CreateRoomPanel.transform.Find("2").Find("Text").gameObject;
        createLetters[28] = CreateRoomPanel.transform.Find("3").Find("Text").gameObject;
        createLetters[29] = CreateRoomPanel.transform.Find("4").Find("Text").gameObject;
        createLetters[30] = CreateRoomPanel.transform.Find("5").Find("Text").gameObject;
        createLetters[31] = CreateRoomPanel.transform.Find("6").Find("Text").gameObject;
        createLetters[32] = CreateRoomPanel.transform.Find("7").Find("Text").gameObject;
        createLetters[33] = CreateRoomPanel.transform.Find("8").Find("Text").gameObject;
        createLetters[34] = CreateRoomPanel.transform.Find("9").Find("Text").gameObject;
        createLetters[35] = CreateRoomPanel.transform.Find("0").Find("Text").gameObject;
    }

    private void SetAllLowercase(GameObject[] letters)
    {
        letters[0].GetComponent<Text>().text = "a";
        letters[1].GetComponent<Text>().text = "b";
        letters[2].GetComponent<Text>().text = "c";
        letters[3].GetComponent<Text>().text = "d";
        letters[4].GetComponent<Text>().text = "e";
        letters[5].GetComponent<Text>().text = "f";
        letters[6].GetComponent<Text>().text = "g";
        letters[7].GetComponent<Text>().text = "h";
        letters[8].GetComponent<Text>().text = "i";
        letters[9].GetComponent<Text>().text = "j";
        letters[10].GetComponent<Text>().text = "k";
        letters[11].GetComponent<Text>().text = "l";
        letters[12].GetComponent<Text>().text = "m";
        letters[13].GetComponent<Text>().text = "n";
        letters[14].GetComponent<Text>().text = "o";
        letters[15].GetComponent<Text>().text = "p";
        letters[16].GetComponent<Text>().text = "q";
        letters[17].GetComponent<Text>().text = "r";
        letters[18].GetComponent<Text>().text = "s";
        letters[19].GetComponent<Text>().text = "t";
        letters[20].GetComponent<Text>().text = "u";
        letters[21].GetComponent<Text>().text = "v";
        letters[22].GetComponent<Text>().text = "w";
        letters[23].GetComponent<Text>().text = "x";
        letters[24].GetComponent<Text>().text = "y";
        letters[25].GetComponent<Text>().text = "z";
        letters[26].GetComponent<Text>().text = "1";
        letters[27].GetComponent<Text>().text = "2";
        letters[28].GetComponent<Text>().text = "3";
        letters[29].GetComponent<Text>().text = "4";
        letters[30].GetComponent<Text>().text = "5";
        letters[31].GetComponent<Text>().text = "6";
        letters[32].GetComponent<Text>().text = "7";
        letters[33].GetComponent<Text>().text = "8";
        letters[34].GetComponent<Text>().text = "9";
        letters[35].GetComponent<Text>().text = "0";
    }

    private void SetAllUppercase(GameObject[] letters)
    {
        letters[0].GetComponent<Text>().text = "A";
        letters[1].GetComponent<Text>().text = "B";
        letters[2].GetComponent<Text>().text = "C";
        letters[3].GetComponent<Text>().text = "D";
        letters[4].GetComponent<Text>().text = "E";
        letters[5].GetComponent<Text>().text = "F";
        letters[6].GetComponent<Text>().text = "G";
        letters[7].GetComponent<Text>().text = "H";
        letters[8].GetComponent<Text>().text = "I";
        letters[9].GetComponent<Text>().text = "J";
        letters[10].GetComponent<Text>().text = "K";
        letters[11].GetComponent<Text>().text = "L";
        letters[12].GetComponent<Text>().text = "M";
        letters[13].GetComponent<Text>().text = "N";
        letters[14].GetComponent<Text>().text = "O";
        letters[15].GetComponent<Text>().text = "P";
        letters[16].GetComponent<Text>().text = "Q";
        letters[17].GetComponent<Text>().text = "R";
        letters[18].GetComponent<Text>().text = "S";
        letters[19].GetComponent<Text>().text = "T";
        letters[20].GetComponent<Text>().text = "U";
        letters[21].GetComponent<Text>().text = "V";
        letters[22].GetComponent<Text>().text = "W";
        letters[23].GetComponent<Text>().text = "X";
        letters[24].GetComponent<Text>().text = "Y";
        letters[25].GetComponent<Text>().text = "Z";
        letters[26].GetComponent<Text>().text = "!";
        letters[27].GetComponent<Text>().text = "@";
        letters[28].GetComponent<Text>().text = "#";
        letters[29].GetComponent<Text>().text = "$";
        letters[30].GetComponent<Text>().text = "%";
        letters[31].GetComponent<Text>().text = "^";
        letters[32].GetComponent<Text>().text = "&";
        letters[33].GetComponent<Text>().text = "*";
        letters[34].GetComponent<Text>().text = "(";
        letters[35].GetComponent<Text>().text = ")";
    }
}
