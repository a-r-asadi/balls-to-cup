using System.IO;
using UnityEngine;

#if UNITY_ANDROID
using UnityEngine.Networking;
#endif

public static class FileUtils
{
    public static string GetStreamingDataPath(string path)
    {
        return Application.streamingAssetsPath + "/" + path;
    }

    public static bool StreamingAssetExists(string path)
    {
        string fullPath = GetStreamingDataPath(path);

#if UNITY_ANDROID && !UNITY_EDITOR
            bool exists = false;

            UnityWebRequest reader = UnityWebRequest.Get(fullPath);
            reader.SendWebRequest();
            while (!reader.isDone) { }
            if (string.IsNullOrEmpty(reader.error))
            {
                exists = true;
            }
            reader.Dispose();

            return exists;
#else
        return File.Exists(fullPath);
#endif
    }

    public static byte[] ReadStreamingAsset(string path)
    {
        string fullPath = GetStreamingDataPath(path);
        byte[] data = null;

#if UNITY_ANDROID && !UNITY_EDITOR
            UnityWebRequest reader = UnityWebRequest.Get(fullPath);
            reader.SendWebRequest();
            while (!reader.isDone) { }
            if (string.IsNullOrEmpty(reader.error))
            {
                data = reader.downloadHandler.data;
            }
            reader.Dispose();
#else
        data = File.ReadAllBytes(fullPath);
#endif
        return data;
    }

    public static string GetPersistentDataPath(string persistentPath)
    {
        return Application.persistentDataPath + "/" + persistentPath;
    }

    public static bool PersistentDataExists(string persistentPath)
    {
        string[] directoriesArray = persistentPath.Split('/');
        string currentPath = Application.persistentDataPath;
        for (int i = 0; i < directoriesArray.Length - 1; i++)
        {
            currentPath += "/" + directoriesArray[i];
            if (!Directory.Exists(currentPath))
            {
                Directory.CreateDirectory(currentPath);
            }
        }

        currentPath += "/" + directoriesArray[directoriesArray.Length - 1];
        return File.Exists(currentPath);
    }

    public static byte[] ReadPersistentData(string persistentPath)
    {
        return File.ReadAllBytes(GetPersistentDataPath(persistentPath));
    }

    public static void WritePersistentData(string persistentPath, byte[] data)
    {
        if (data == null)
        {
            return;
        }

        File.WriteAllBytes(GetPersistentDataPath(persistentPath), data);
    }

    public static void DeletePersistentData(string persistentPath)
    {
        File.Delete(GetPersistentDataPath(persistentPath));
    }

    public static void CopyFromStreamingToPersistent(string streamingPath, string persistentPath)
    {
        if (!PersistentDataExists(persistentPath))
        {
            WritePersistentData(persistentPath, ReadStreamingAsset(streamingPath));
        }
    }

    public static void CheckCreateDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}