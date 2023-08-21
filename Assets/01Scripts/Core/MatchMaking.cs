using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MatchMaking
{
    public async void FindMatch()
    {
        await NetworkManager.Instance.roomListUpdateTaskCompletionSource.Task;
        List<RoomInfo> currentRoomList = NetworkManager.Instance.currentRoomList;

        if (currentRoomList.Count > 0)
        {
            foreach (RoomInfo roomInfo in currentRoomList)
            {
                Debug.Log("Testing Room Name: " + roomInfo.Name + " " + roomInfo.IsOpen);
                
                if (roomInfo.PlayerCount < 2 && roomInfo.IsOpen && roomInfo.CustomProperties["PlayerLevel"] != null)
                {
                    if ((int) roomInfo.CustomProperties["PlayerLevel"] >= GameData.Instance.PlayerLevel + 3 ||
                        (int) roomInfo.CustomProperties["PlayerLevel"] <= GameData.Instance.PlayerLevel + 3)
                    {
                        PhotonNetwork.JoinRoom(roomInfo.Name);
                        Debug.Log("Finded Rival Joined Room: " + roomInfo.Name);
                        return;
                    }
                }
                else
                {
                    Debug.Log("No Empty Room Found");
                    NetworkManager.Instance.CreatRoom();
                    return;
                }
            }

            Debug.Log("Not Find Rival");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.Log("Not Have Eny Room");
            NetworkManager.Instance.CreatRoom();
        }
    }
}