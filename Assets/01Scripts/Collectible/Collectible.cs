using Photon.Pun;
using UnityEngine;

public class Collectible : MonoBehaviourPunCallbacks
{
    #region Inspector

    [SerializeField] private Collider collider;

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Constants.Layers.PLAYABLE_CHARACTER && other.GetComponent<PhotonView>().IsMine)
        {
            collider.enabled = false;

            CollectibleListener.Instance.OnCollectibleTaken();

            transform.PlayParticle_OnPoolTransform(Constants.Particles.COLLECTIBLE_TAKEN);

            photonView.RPC("DestroyCollectible", RpcTarget.All);

            //NOTE: Maybe add haptic.
        }
    }

    [PunRPC]
    private void DestroyCollectible() => Destroy(gameObject);
}