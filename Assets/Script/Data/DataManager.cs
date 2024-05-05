using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    static private GameObject container;
    static private DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (!instance)
            {
                container = new GameObject();
                container.name = "Data Manager";
                instance = container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }

    string GameDataFileName = "GameData.json";
    public GameData data = new GameData();

    string SettingDataFileName = "SettingData.json";
    public SettingData settingData = new SettingData();
    public bool LoadGameData()
    {
        var filePath = Application.persistentDataPath + "/" + GameDataFileName;
        if (File.Exists(filePath))
        {
            var FromJsonData = File.ReadAllText(filePath);
            try
            {
                data = JsonUtility.FromJson<GameData>(FromJsonData);
                Debug.Log("Data Loaded From : " + filePath);
            }
            catch (System.Exception e)
            {
                Debug.Log("bug : " + e);
            }
            return true;
        }
        else
        {
            //SaveManager.Instance.InitData();
            return false;
        }
    }

    public void DeleteGameData()
    {
        var filePath = Application.persistentDataPath + "/" + GameDataFileName;
        if (File.Exists(filePath))
            File.Delete(filePath);
        data = null;
    }

    public void SaveGameData()
    {
        var filePath = Application.persistentDataPath + "/" + GameDataFileName;
        var ToJasonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, ToJasonData);
        Debug.Log("Data Saved");
    }

    public bool HasFile()
    {
        var filePath = Application.persistentDataPath + "/" + GameDataFileName;
        //Debug.Log("file°æ·Î: " + filePath);
        return File.Exists(filePath);
    }

    public bool LoadSettingData()
    {
        var filePath = Application.persistentDataPath + "/" + SettingDataFileName;
        if (File.Exists(filePath))
        {
            var FromJsonData = File.ReadAllText(filePath);
            try
            {
                settingData = JsonUtility.FromJson<SettingData>(FromJsonData);
                Debug.Log("Setting Data Loaded From : " + filePath);
            }
            catch (System.Exception e)
            {
                Debug.Log("bug : " + e);
            }
            return true;
        }
        else
        {
            //SaveManager.Instance.InitData();
            return false;
        }
    }
    public void SaveSettingData()
    {
        var filePath = Application.persistentDataPath + "/" + SettingDataFileName;
        var ToJasonData = JsonUtility.ToJson(settingData, true);
        File.WriteAllText(filePath, ToJasonData);
        Debug.Log("Setting Data Saved");
    }
}
