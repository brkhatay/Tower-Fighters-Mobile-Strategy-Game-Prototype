using UnityEngine.Events;

public class GenericEventHandler : IGenericEventHandler
{
    private UnityAction mainEvent;

    public UnityAction MainEvent
    {
        get => mainEvent;
        set => mainEvent = value;
    }

    #region IGenericEventHandler

    private UnityAction<bool> boolEventAction;

    public UnityAction<bool> BoolEventAction
    {
        get => boolEventAction;
    }

    public UnityAction<bool> SubscribeBoolEventAction
    {
        set => boolEventAction += value;
    }

    public UnityAction<bool> UnSubscribeBoolEventAction
    {
        set => boolEventAction -= value;
    }

    public UnityAction SubscribeMainEventAction
    {
        set => mainEvent += value;
    }

    #endregion
}

public interface IGenericEventHandler
{
    UnityAction<bool> SubscribeBoolEventAction { set; }
    UnityAction<bool> UnSubscribeBoolEventAction { set; }
    UnityAction SubscribeMainEventAction { set; }
}