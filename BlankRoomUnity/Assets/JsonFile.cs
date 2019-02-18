﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFile : MonoBehaviour
{

	public Image RoomData;
	string dataPath;

    // Start is called before the first frame update
    void Start()
    {
        dataPath = Path.Combine(Application.persistentDataPath, "RoomData.txt");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown (KeyCode.S))
            SaveRoom (RoomData, dataPath);

        if (Input.GetKeyDown (KeyCode.L))
            RoomData = LoadRoom (dataPath);
    }

	public static void SaveRoom(Image RoomData, string path)
	{
		
		//Convert RoomData Image to String
		String RoomDataAsString = ImageToString(RoomData);

		string jsonString = JsonUtility.ToJson (RoomDataAsString);

		using (StreamWriter streamWriter = File.CreateText (path))
        {
            streamWriter.Write (jsonString);
        }
	}

	public static Image LoadRoom(string path)
	{
		using (StreamReader streamReader = File.OpenText (path))
        {
            string jsonString = streamReader.ReadToEnd ();
            return StringToImage(JsonUtility.FromJson<string> (jsonString));

        }
	}

	string ImageToString(Image im)
	{
		MemoryStream ms = new MemoryStream();

		im.Save(ms, im.RawFormat);

		byte[] array = ms.ToArray();

		return Convert.ToBase64String(array);
	}

	public Image StringToImage(string imageString){
		if (imageString == null)

		throw new ArgumentNullException("imageString");

		byte[] array = Convert.FromBase64String(imageString);

		Image image = Image.FromStream(new MemoryStream(array));

		return image;
	}
}