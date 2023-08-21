using UnityEngine;

public class UIController : MonoBehaviour
{
    #region Singelton

    public static UIController Instance;

    #endregion

    #region Inspector

    [SerializeField] private Transition transition;
    [SerializeField] private UIInfoText infoText;

    [SerializeField] private GameObject inGameMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject splahPanel;
    [SerializeField] private GameObject winPanel;

    #endregion

    private void Awake() => Instance ??= this;

    public void OnGameStart()
    {
        transition.ActivateTransition((() => { mainMenu.SetActive(false); }));
    }

    public void OnReturnMenu()
    {
        mainMenu.SetActive(true);
        splahPanel.SetActive(true);
        inGameMenu.SetActive(false);
    }

    public void OnPLayerWin() => winPanel.SetActive(true);

    public void OnPlayerLose() => losePanel.SetActive(true);

    public void OnPlayerJoinedLobby()
    {
        transition.ActivateTransition((() =>
        {
            splahPanel.SetActive(false);
            inGameMenu.SetActive(true);
        }));
    }

    public void OnPlayerLeftGame()
    {
        infoText.OnPlayerLeftGame();
    }
}