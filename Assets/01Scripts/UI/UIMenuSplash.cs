using System.Collections;
using UnityEngine;

public class UIMenuSplash : MonoBehaviour
{
    [SerializeField] private Transform logoImage;
    [SerializeField] private UIInfoText infoText;

    void OnEnable()
    {
        StartCoroutine(IELerpScale(logoImage, Vector3.one));
        infoText.OnConnecting();
    }

    private void OnDisable() => infoText.Clear();

    #region Enumerators

    IEnumerator IELerpScale(Transform target, Vector3 end)
    {
        float lerpDuration = 0f, lerpEndValue = 4f;
        while (lerpDuration < lerpEndValue)
        {
            lerpDuration += Time.deltaTime;
            target.localScale = Vector3.Lerp(target.localScale, end, (lerpDuration / lerpEndValue));
            yield return null;
        }

        yield return null;
    }

    #endregion
}