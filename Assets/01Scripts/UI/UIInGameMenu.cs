using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInGameMenu : MonoBehaviour
{
    #region Inspector

    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI buttonTextTMP;
    [SerializeField] private UIInfoText infoText;

    #endregion

    void OnEnable() => SetButtonFindMatch();

    private void SetButtonReturn()
    {
        playButton.onClick.RemoveAllListeners();
        buttonTextTMP.text = "Return";
        playButton.onClick.AddListener((() =>
        {
            GameController.Instance.ReturnLobby();
            infoText.Clear();
            SetButtonFindMatch();
        }));
    }

    private void SetButtonFindMatch()
    {
        infoText.Clear();
        playButton.onClick.RemoveAllListeners();
        buttonTextTMP.text = "Find Game";
        playButton.onClick.AddListener((() =>
        {
            GameController.Instance.FindMatch();
            infoText.OnMatchMaking();
            SetButtonReturn();
        }));
    }
}