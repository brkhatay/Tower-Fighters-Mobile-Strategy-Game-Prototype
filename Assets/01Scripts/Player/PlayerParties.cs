using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerParties : MonoBehaviour
{
    #region Singelton

    public static PlayerParties Instance { get; private set; }

    #endregion

    #region Inspector

    [SerializeField] private PartieSettings[] partieSettings;

    #endregion

    private void Awake() => Instance ??= this;


    private byte CustomManualInstantiationEventCode;

    public void PlayerPartiesInit(int side, out List<Transform> spawnPoints)
    {
        PhotonNetwork.Instantiate("PlayerSide", side == 1 ? Vector3.forward * 7 : Vector3.zero,
                side == 1 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity).GetComponent<PlayerSide>()
            .SetPlayerSide(side);

        spawnPoints = partieSettings[side].playebleCaharacterpoints;
    }
}

[System.Serializable]
public class PartieSettings
{
    public List<Transform> playebleCaharacterpoints = new List<Transform>();
}