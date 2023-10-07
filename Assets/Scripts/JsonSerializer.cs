using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;

public class JsonSerializer
{
    public bool SaveData<T>(string relativePath, T data)
    {
        string path = Application.persistentDataPath + relativePath;

        try
        {
            if (File.Exists(path))
            {
                Debug.Log("Data exists. Deleting old file and writing new one");
                File.Delete(path);
            }
            else
            {
                Debug.Log("Writing new file");
            }

            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonConvert.SerializeObject(data));
            return true;
        }
        catch(Exception e)
        {
            Debug.LogError($"Unable to save data: {e.Message} {e.StackTrace}");
            return false;
        }
    }

    public T LoadData<T>(string relativePath)
    {
        string path = Application.persistentDataPath + relativePath;
        if (!File.Exists(path))
        {
            Debug.LogError($"Can't load file at {path}");
            throw new FileNotFoundException();
        }

        try
        {
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return data;
        }
        catch(Exception e)
        {
            Debug.LogError($"Can't load data due to {e.Message} {e.StackTrace}");
            throw e;
        }
    }
}
