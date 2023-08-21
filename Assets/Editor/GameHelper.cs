#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GameHelper : EditorWindow
{
    [UnityEditor.MenuItem("/Game Helper/Delete Game Data", false, 0)]
    static void DeleteGameData()
    {
        DirectoryInfo dataDir = new DirectoryInfo(Application.persistentDataPath);
        dataDir.Delete(true);
        PlayerPrefs.DeleteAll();
        Debug.Log($"<color=magenta> All Data and Prefs Deleted </color>");
    }
}
#endif