using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class PlayableCharacter : MonoBehaviourPunCallbacks, IChacarter
{
    #region Inspector

    [SerializeField] private PhotonView photonView;
    [SerializeField] private Outline outline;
    [SerializeField] private PlayableCharacterVisibleObject[] characters;

    #endregion

    #region Private Parametters

    //Objects
    private Camera mainCamera;
    private PlayableCharacterVisibleObject currentCaharacterObject;

    //Path
    private List<Node> nodeList = new List<Node>();
    private int currentNodePointIndex = 0;

    //Moving
    private float movingSpeed = 10f;

    //Touch
    private Vector2 touchStartPos;

    //Actions
    private UnityAction OnClickEvent;
    private UnityAction OnCharacterDeSelect;
    private UnityAction UpdateAction;

    #endregion

    #region Unity

    private void Awake() => mainCamera = Camera.main;

    private void OnMouseDown()
    {
        if (!photonView.IsMine) return;
        OnClickEvent?.Invoke();
    }

    void Update() => UpdateAction?.Invoke();

    #endregion

    #region Init

    #region Appearances

    [PunRPC]
    public void SetCharacterAppearances(int val)
    {
        currentCaharacterObject = characters[val];
        characters[val].Init(val);
        outline.Init();
    }

    #endregion

    public void SetCharacter(int side, UnityAction characterSelectAction,
        UnityAction characterDeSelectAction, IGenericEventHandler selectAction)
    {
        //Appearances
        photonView.RPC(nameof(SetCharacterAppearances), RpcTarget.All, side);

        //Events
        selectAction.SubscribeMainEventAction = () => { OnCharacterDeSelect?.Invoke(); };
        characterDeSelectAction += () => { InputManager.Instance.OnTouchPosition -= MoveCharacter; };

        //Click event main
        OnClickEvent += characterSelectAction;
        OnClickEvent += OnCharacterSelected;
        OnClickEvent += () => touchStartPos = InputManager.Instance.CurrentTouchPos;
        OnClickEvent += currentCaharacterObject.OnCharacterSelected;

        //For Path Find
        OnClickEvent += () => Pathfinding.Instance.SetStartPos(transform.position);

        InputManager.Instance.OnTouchEnd += OnReleased;

        //Outline
        OnClickEvent += () => outline.enabled = true;
        characterDeSelectAction += () => outline.enabled = false;

        OnCharacterDeSelect += characterDeSelectAction;
    }

    #endregion

    #region Controls

    #endregion

    private void OnReleased()
    {
        if (!photonView.IsMine) return;

        if (touchStartPos.normalized == InputManager.Instance.CurrentTouchPos.normalized) return;
        OnCharacterDeSelect?.Invoke();
    }

    private void OnCharacterSelected()
    {
        UpdateAction -= CharacterMoving;
        InputManager.Instance.OnTouchPosition += MoveCharacter;
    }

    private void MoveCharacter(Vector2 pos)
    {
        Ray ray = mainCamera.ScreenPointToRay(pos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            //Layer Mask
            if (hit.transform.gameObject.layer != 6) return;

            //Move points
            Pathfinding.Instance.SetStartPos(transform.position);
            nodeList = Pathfinding.Instance.FindPath(hit.point);

            //Animation end Effects
            currentCaharacterObject?.OnCharacterMoved();

            //Move point
            currentNodePointIndex = Mathf.Clamp(nodeList.Count - 3, 0, nodeList.Count);

            //Start moving
            UpdateAction ??= CharacterMoving;
        }
    }

    private void CharacterMoving()
    {
        if (nodeList.Count > 0)
        {
            MoveToNodePoint(nodeList[currentNodePointIndex].position);

            if ((transform.position - nodeList[currentNodePointIndex].position).magnitude < 2f)
            {
                currentNodePointIndex = Mathf.Abs((currentNodePointIndex - 1) % nodeList.Count);

                if (currentNodePointIndex == 0)
                {
                    if ((transform.position - nodeList[currentNodePointIndex].position).magnitude < 2f)
                    {
                        UpdateAction -= CharacterMoving;
                        currentCaharacterObject?.OnCharacterStop();
                    }
                }
            }
        }
    }

    private void MoveToNodePoint(Vector3 targetWaypoint)
    {
        //Set Look
        Vector3 lookDirection = targetWaypoint - transform.position;
        lookDirection.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

        //Set Rot
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, movingSpeed * Time.deltaTime);

        //Set Pos
        transform.Translate(Vector3.forward * movingSpeed * Time.deltaTime);
    }
}