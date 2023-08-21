using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Singilten

    public static NetworkManager Instance { get; private set; }

    #endregion

    #region Private Properties

    private TypedLobby customLobby = new TypedLobby("customLobby", LobbyType.Default);

    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

    #endregion

    #region Public Properties

    public List<RoomInfo> currentRoomList = new List<RoomInfo>();

    public TaskCompletionSource<bool> roomListUpdateTaskCompletionSource = new TaskCompletionSource<bool>();

    #endregion

    private void Awake()
    {
        Instance ??= this;

        PhotonNetwork.SendRate = 20;
        PhotonNetwork.SerializationRate = 5;
        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.ConnectUsingSettings();
    }

    #region Photon CallBacks

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log(" ***PHOTON***  On Connected To Master");

        roomListUpdateTaskCompletionSource.TrySetResult(false);
        PhotonNetwork.JoinLobby(customLobby);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log(" ***PHOTON***  On Joined Lobby");

        GameController.Instance.OnPlayerJoinedLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("***PHOTON***  On Room List Update: " + roomList.Count);

        UpdateCachedRoomList(roomList);
        roomListUpdateTaskCompletionSource.TrySetResult(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message) => CreatRoom();

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log(" ***PHOTON***  On Joined Room");

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            int firstVal = Random.value > 0.5f ? 0 : 1;
            int secondVal = 1 - firstVal;


            if (!PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("OnPlayersReady", RpcTarget.MasterClient,
                    1);
            }

            GameController.Instance.OnPlayersReady(0);
        }
    }

    public override void OnLeftRoom() => PhotonNetwork.JoinLobby(customLobby);

    private void OnApplicationQuit() => ForceReloadScene();

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from Photon Server: " + cause);
        ForceReloadScene();
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);
        ForceReloadScene();
    }

    private void ForceReloadScene()
    {
        photonView.RPC("ForceReloadScene", RpcTarget.All);
    }

    #endregion

    #region Public Methods

    public void CreatRoom()
    {
        Debug.Log(" ***PHOTON*** Creat Room");
        RoomOptions ropts = new RoomOptions {IsOpen = true, IsVisible = true, MaxPlayers = 2};

        Hashtable roomProps = new Hashtable();

        roomProps.Add("inGame", 0);
        roomProps.Add("PlayerLevel", GameData.Instance.PlayerLevel);

        ropts.CustomRoomProperties = roomProps;
        ropts.CustomRoomPropertiesForLobby = new[] {"PlayerLevel"};
        PhotonNetwork.CreateRoom("Room" + Random.Range(0, 15), ropts, TypedLobby.Default);
    }

    [PunRPC]
    public void ReturnLobby()
    {
        PhotonNetwork.RemoveRPCs(PhotonNetwork.LocalPlayer);
        PhotonNetwork.Disconnect();
        PhotonNetwork.LeaveLobby();

        PhotonNetwork.ConnectUsingSettings();
    }

    #endregion

    #region Private Methods

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        cachedRoomList.Clear();
        currentRoomList = roomList;
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];

            if (info.RemovedFromList)
                cachedRoomList.Remove(info.Name);
            else
                cachedRoomList[info.Name] = info;
        }
    }

    #endregion
}