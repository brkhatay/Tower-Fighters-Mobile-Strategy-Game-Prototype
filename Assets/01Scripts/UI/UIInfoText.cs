using System.Collections;
using TMPro;
using UnityEngine;

public class UIInfoText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoTMP;

    private string mainText;
    private bool textAnimation;
    private Coroutine textCoroutine;

    public void OnConnecting()
    {
        mainText = "Connecting";
        textAnimation = true;
        textCoroutine = StartCoroutine(IEConnectingTMPAnim());
    }

    public void OnMatchMaking()
    {
        mainText = "Looking for challengers";
        textAnimation = true;
        textCoroutine = StartCoroutine(IEConnectingTMPAnim());
    }

    public void Clear()
    {
        StopCoroutine(textCoroutine);
        textAnimation = false;
        infoTMP.text = "";
    }

    public void OnPlayerLeftGame()
    {
        mainText = "Challenger left the game";
        textCoroutine = StartCoroutine(IETextDelayChabge(2f, ""));
    }

    private IEnumerator IEConnectingTMPAnim()
    {
        while (textAnimation)
        {
            infoTMP.text = mainText + ".";
            yield return new WaitForSeconds(.3f);
            infoTMP.text = mainText + "..";
            yield return new WaitForSeconds(.3f);
            infoTMP.text = mainText + "...";
            yield return new WaitForSeconds(.3f);
            infoTMP.text = mainText;
            yield return new WaitForSeconds(.3f);
        }
    }

    private IEnumerator IETextDelayChabge(float delay, string text)
    {
        yield return new WaitForSeconds(delay);
        infoTMP.text = text;
    }
}