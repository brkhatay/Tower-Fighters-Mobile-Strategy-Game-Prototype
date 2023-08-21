using Photon.Pun;
using UnityEngine;

public class CanonBallFire : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private ParticleSystem fireParticle;
    [SerializeField] private ObjectAnimator objectAnimator;


    [PunRPC]
    public void FireCanonBall()
    {
        fireParticle.Play();
        objectAnimator.Shake();

        CameraController.Instance.Shake();

        if (!photonView.IsMine) return;
        PhotonNetwork.Instantiate(Constants.Prefabs.CANON_BALL_BULLET, bulletSpawnPoint.position,
            Quaternion.identity).GetComponent<CanonBallBullet>().CanonBulletSendToEnemy(bulletSpawnPoint.position);
    }
}