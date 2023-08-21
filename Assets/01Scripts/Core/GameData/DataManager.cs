/**
 * Copyright (c) 2023-present Burak Hatay. All rights reserved.
 */

using System.Threading.Tasks;

public static class DataManager
{
    private static ISaveClient client = new LocalClient();

    public static void Save(object val, string key)
    {
        client.Save(key, val);
    }

    public static Task<T> LoadData<T>(string key)
    {
        return client.Load<T>(key);
    }

    public static void RemoveData(string key)
    {
        client.Delete(key);
    }
}