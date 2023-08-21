using Photon.Pun;
using UnityEngine;

public class PlayerSide : MonoBehaviourPunCallbacks
{
    private const byte HEALT = 6;
    [SerializeField] private PlayerSideAppearance playerSideAppearance;
    [SerializeField] private PhotonView canonBallFire;

    private PlayerSide enemy;
    private byte collectCount;
    private byte damageCount;

    #region Appearance

    public void SetPlayerSide(int val)
    {
        photonView.RPC("SetPlayerColor", RpcTarget.All, val);

        if (!photonView.IsMine) return;
        CollectibleListener.Instance.OnCollectibleTakenEvent += OnResourceCollected;
        gameObject.name = gameObject.name + PhotonNetwork.LocalPlayer.UserId;
    }

    #endregion

    public void OnResourceCollected()
    {
        if (!photonView.IsMine) return;
        collectCount++;
        if (collectCount == 3)
        {
            OnAttack();
            collectCount = 0;
        }
    }

    private void OnAttack()
    {
        if (!photonView.IsMine) return;
        canonBallFire.GetComponent<PhotonView>().RPC("FireCanonBall", RpcTarget.All);
    }

    [PunRPC]
    private void OnTakeDamage()
    {
        if (!photonView.IsMine) return;

        damageCount++;

        if (damageCount == HEALT)
        {
            GameController.Instance.OnPlayerLose();
        }

        photonView.RPC("OnTakeDamageAppearance", RpcTarget.All);
    }
}