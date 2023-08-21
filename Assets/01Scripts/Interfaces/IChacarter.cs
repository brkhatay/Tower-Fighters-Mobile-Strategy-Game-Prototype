using UnityEngine.Events;

public interface IChacarter
{
    public void SetCharacter(
        int side,
        UnityAction characterSelectAction,
        UnityAction characterDeSelectAction,
        IGenericEventHandler selectAction);
}