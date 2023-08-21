using Photon.Pun;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviourPunCallbacks
{
    private const int MINIMUM_ITEM_COUNT = 5;
    private int spawnedItemCount;

    public void OnPlayersReady()
    {
        CollectibleListener.Instance.OnCollectibleTakenEvent += OnCollectibleTaken;

        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < 15; i++)
                photonView.RPC("SpawnObject", RpcTarget.All);
        }
    }

    [PunRPC]
    private void SpawnObject()
    {
        PhotonNetwork.Instantiate(
            Constants.Prefabs.COLELCTIBLE_WOOD,
            Pathfinding.Instance.AvailableRandomPoint(),
            Quaternion.identity);

        spawnedItemCount++;
    }

    public void OnCollectibleTaken() => photonView.RPC("SpawnObject", RpcTarget.All);
}