using UnityEngine;
using UnityEngine.Events;

public class Transition : MonoBehaviour
{
    private UnityAction endEvent;
    [SerializeField] private RectTransform image;

    public void ActivateTransition(UnityAction action)
    {
        Vector3 leftPosition = new Vector3(-Screen.width * 4, 0f, 0f);

        image.anchoredPosition = leftPosition;

        image.RecAnchorLerpPosX(Screen.width * 8, 1.2f, false, action);
    }

    public void AnimationEndEvent()
    {
        endEvent?.Invoke();
        endEvent = null;
    }
}