/**
 * Copyright (c) 2023-present Burak Hatay. All rights reserved.
 */

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager
{
    #region Singleton

    public static GameManager Instance { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initial()
    {
        Instance ??= new GameManager();
        Instance.SetUp();
    }

    #endregion

    #region Mono

    public ManagerMono managerMono;

    public class ManagerMono : MonoBehaviour
    {
        public UnityAction OnApllicationClosed;

        private void OnApplicationQuit()
        {
            OnApllicationClosed?.Invoke();
        }
    }

    #endregion

    private async void SetUp()
    {
        #region Main Settings

        // Time.maximumDeltaTime = 1f / 25f;
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        #endregion

        #region Mono

        managerMono = new GameObject().AddComponent<ManagerMono>();
        managerMono.name = "ManagerMono";

        #endregion
    }

    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    }
}

public static class ManagerConstants
{
}