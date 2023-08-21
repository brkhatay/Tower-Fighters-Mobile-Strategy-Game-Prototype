using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Singleton

    public static CameraController Instance { get; private set; }

    #endregion

    private void Awake()
    {
        Instance ??= this;
    }

    public void Shake() => transform.ShakeRandom();
}