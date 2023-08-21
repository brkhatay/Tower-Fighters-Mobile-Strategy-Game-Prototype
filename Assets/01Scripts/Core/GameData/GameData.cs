/**
 * Copyright (c) 2023-present Burak Hatay. All rights reserved.
 */

using UnityEngine;

public static class DataTags
{
    public const string PlayerData = "PlayerData";
}

public class GameData
{
    #region Singleton

    private static GameData instance;
    public static GameData Instance => instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void Initial()
    {
        instance ??= new GameData();
        instance.LoadData();
    }

    #endregion

    #region Parameters

    private PlayerData playerData = new PlayerData();

    #endregion

    private async void LoadData()
    {
        if (PlayerPrefs.GetInt("GameFirstPlay", 0) == 0)
        {
            playerData.playerLevel = 0;
            SavePlayerData();
            PlayerPrefs.SetInt("GameFirstPlay", 1);
        }

        playerData = await DataManager.LoadData<PlayerData>(DataTags.PlayerData);
    }

    #region Player Data

    public void OnPlayerLevelUp()
    {
        playerData.playerLevel++;
        SavePlayerData();
    }

    public void OnPlayerLevelDown()
    {
        playerData.playerLevel = Mathf.Clamp(playerData.playerLevel--, 0, playerData.playerLevel);
        SavePlayerData();
    }

    private void SavePlayerData() => DataManager.Save(playerData, DataTags.PlayerData);

    public int PlayerLevel => playerData.playerLevel;

    #endregion
}

#region Player

[System.Serializable]
public class PlayerData
{
    public int playerLevel;
}

#endregion