using UnityEngine;

public class CollectibleListener
{
    #region Singleton

    public static CollectibleListener Instance { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initial()
    {
        Instance ??= new CollectibleListener();
    }

    #endregion

    public event OnCollectibleTakenEvent OnCollectibleTakenEvent;

    public void OnCollectibleTaken() => OnCollectibleTakenEvent?.Invoke();

    public void OnGameStop() => OnCollectibleTakenEvent = null;
}