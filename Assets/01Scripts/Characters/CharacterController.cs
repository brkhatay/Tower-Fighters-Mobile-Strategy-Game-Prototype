using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviourPunCallbacks
{
    #region Private Parameters

    private const byte CHARACTER_SPAWN_COUNT = 3;

    private UnityAction characterSelectedEvent;
    private GenericEventHandler CharacterSelectedEvent = new GenericEventHandler();

    #endregion

    #region Singleton

    public static CharacterController Instance { get; private set; }

    #endregion

    private void Awake() => Instance ??= this;

    public void OnPlayersReady(int side, List<Transform> spawnPoints)
    {
        CharacterSelectedEvent.MainEvent = () => { CharacterSelectedEvent.BoolEventAction?.Invoke(true); };

        for (byte i = 0; i < CHARACTER_SPAWN_COUNT; i++)
        {
            PhotonNetwork.Instantiate(Constants.Prefabs.PLAYABLE_CHARACTER,
                    spawnPoints[i].position,
                    Quaternion.Euler(0, 180, 0)).GetComponent<IChacarter>()
                .SetCharacter(
                    side,
                    () => CharacterSelectedEvent.MainEvent?.Invoke(),
                    () => CharacterSelectedEvent.BoolEventAction?.Invoke(false),
                    CharacterSelectedEvent);
        }
    }
}