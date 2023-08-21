/**
 * Copyright (c) 2023-present Burak Hatay. All rights reserved.
 */

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;

public class LocalClient : ISaveClient
{
    private const string FORMAT = ".bin";


    public async Task Save(string key, object obj)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/" + key + FORMAT;
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

        formatter.Serialize(stream, obj);
        stream.Close();
    }

    public async Task<T> Load<T>(string key)
    {
        string path = Application.persistentDataPath + "/" + key + FORMAT;

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            T data = (T) formatter.Deserialize(stream);
            stream.Close();

            return data;
        }

        return default(T);
    }

    public async Task Delete(string key)
    {
        string path = Application.persistentDataPath + "/" + key + FORMAT;

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static T Save<T>(T data, string key)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/" + key + FORMAT;
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

        formatter.Serialize(stream, data);
        stream.Close();

        return data;
    }
}