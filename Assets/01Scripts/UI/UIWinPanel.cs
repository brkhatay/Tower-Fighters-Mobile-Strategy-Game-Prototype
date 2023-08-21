using UnityEngine;
using UnityEngine.UI;

public class UIWinPanel : MonoBehaviour
{
    [SerializeField] private Button retunrButton;

    void Awake()
    {
        retunrButton.onClick.AddListener(() =>
        {
            GameController.Instance.ReloadScene();
            gameObject.SetActive(false);
        });
    }
}