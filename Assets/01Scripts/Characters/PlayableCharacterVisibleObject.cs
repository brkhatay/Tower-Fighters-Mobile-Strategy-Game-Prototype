using Photon.Pun;
using UnityEngine;

public class PlayableCharacterVisibleObject : MonoBehaviourPunCallbacks
{
    #region Inspector

    [SerializeField] private PlayableCharacterAppearance playableCharacterAppearance;
    [SerializeField] private PlayableCharacterAnimationController characterAnimationController;

    #endregion

    public void Init(int sideVal)
    {
        gameObject.SetActive(true);
        playableCharacterAppearance.SetAppearance(sideVal);

        photonView.RPC("OnSpawned", RpcTarget.All);
    }

    public void OnCharacterSelected()
    {
        photonView.RPC("OnCharacterSelected", RpcTarget.All);
    }

    public void OnCharacterMoved()
    {
        photonView.RPC("OnCharacterMoved", RpcTarget.All);
    }

    public void OnCharacterStop()
    {
        photonView.RPC("OnCharacterStop", RpcTarget.All);
    }
}