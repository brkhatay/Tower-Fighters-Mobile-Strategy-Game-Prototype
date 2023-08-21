using Photon.Pun;
using UnityEngine;

public class CanonBallBullet : MonoBehaviourPunCallbacks
{
    [SerializeField] private Collider collider;
    private int fireDistance = 45;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Constants.Layers.PLAYER_TOWER && photonView.IsMine)
        {
            collider.enabled = false;

            other.GetComponent<PhotonView>().RPC("OnTakeDamage", RpcTarget.All);
            transform.PlayParticle_OnPoolTransform(Constants.Particles.COLLECTIBLE_TAKEN);

            gameObject.SetActive(false);
        }
    }

    public void CanonBulletSendToEnemy(Vector3 bulletSpawnPoint)
    {
        if (photonView.IsMine)
            photonView.RPC(nameof(SendCanonBall), RpcTarget.All, bulletSpawnPoint);
    }

    [PunRPC]
    private void SendCanonBall(Vector3 bulletSpawnPoint)
    {
        transform
            .LerpParabola_Task(
                bulletSpawnPoint + (bulletSpawnPoint.x > 0
                    ? Vector3.left
                    : Vector3.right) * fireDistance, 1f, 10f);
    }
}