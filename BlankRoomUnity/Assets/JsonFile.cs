using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Drawing;

//https://unity3d.com/learn/tutorials/topics/scripting/introduction-saving-and-loading
//Class was based on info from Unity saving tutorial listed above
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
		string RoomDataAsString = ImageToString(RoomData);

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

	//https://social.msdn.microsoft.com/Forums/en-US/011c2c24-ffc5-4be5-883b-2430267b33c8/serialize-an-image-to-a-string?forum=Vsexpressvcs
	//Two functions below based on Solution posted on Forum listed
	public static string ImageToString(Image im)
	{
		MemoryStream ms = new MemoryStream();

		im.Save(ms, im.RawFormat);

		byte[] array = ms.ToArray();

		return System.Convert.ToBase64String(array);
	}

	//https://social.msdn.microsoft.com/Forums/en-US/011c2c24-ffc5-4be5-883b-2430267b33c8/serialize-an-image-to-a-string?forum=Vsexpressvcs
	public static Image StringToImage(string imageString){
		//if (imageString == null)

		//throw new ArgumentNullException("imageString");

		byte[] array = System.Convert.FromBase64String(imageString);

		System.Drawing.Image image = Image.FromStream(new MemoryStream(array));

		return image;
	}
}
