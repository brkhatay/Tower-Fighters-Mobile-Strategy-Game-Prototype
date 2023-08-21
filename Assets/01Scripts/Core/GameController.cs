using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameController : MonoBehaviourPunCallbacks
{
    #region Inspector

    [SerializeField] private CollectibleSpawner collectibleSpawner;
    [SerializeField] private Grid grid;
    [SerializeField] private MatchMaking matchMaking;

    #endregion

    #region Singleton

    public static GameController Instance { get; private set; }

    #endregion

    private bool baypassTransition;

    private void Awake()
    {
        Instance ??= this;
        matchMaking = new MatchMaking();
    }

    [PunRPC]
    public void OnPlayersReady(int side)
    {
        //Character spawn points
        List<Transform> spawnPoints;

        //Set player towers
        PlayerParties.Instance.PlayerPartiesInit(side, out spawnPoints);

        //Collectibles
        if (PhotonNetwork.IsMasterClient)
            collectibleSpawner.OnPlayersReady();

        //Spawn characters
        CharacterController.Instance.OnPlayersReady(side, spawnPoints);

        //Path grid
        grid.MakeGrid();

        //UI
        UIController.Instance.OnGameStart();

        //Controls
        InputManager.Instance.OnGameStart();
    }

    [PunRPC]
    public void OnPLayerWin()
    {
        OnGameStop();
        UIController.Instance.OnPLayerWin();
        GameData.Instance.OnPlayerLevelUp();
    }

    public void OnPlayerLose()
    {
        OnGameStop();
        UIController.Instance.OnPlayerLose();
        photonView.RPC(nameof(OnPLayerWin), RpcTarget.Others);
        GameData.Instance.OnPlayerLevelDown();
    }

    public void OnGameStop()
    {
        InputManager.Instance.OnGameStop();
        CollectibleListener.Instance.OnGameStop();
        ParticleController.Instance.OnGameStop();
    }

    public void OnPlayerJoinedLobby()
    {
        if (!baypassTransition)
            UIController.Instance.OnPlayerJoinedLobby();

        baypassTransition = false;
    }

    public void FindMatch() => matchMaking.FindMatch();

    public void ReturnLobby()
    {
        baypassTransition = true;
        NetworkManager.Instance.ReturnLobby();
    }

    public void ReloadScene()
    {
        UIController.Instance.OnReturnMenu();

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.DestroyAll();
        }

        NetworkManager.Instance.ReturnLobby();

        baypassTransition = false;
    }

    [PunRPC]
    public void ForceReloadScene()
    {
        OnGameStop();
        ReloadScene();
        UIController.Instance.OnPlayerLeftGame();
    }
}