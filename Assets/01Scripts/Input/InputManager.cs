using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.EnhancedTouch;

public class InputManager
{
    #region Events

    public UnityAction OnTouchEnd;
    public UnityAction<Vector2> OnTouchPosition;

    #endregion

    #region Singleton

    public static InputManager Instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initial()
    {
        Instance ??= new InputManager();
        Instance.Init();
    }

    #endregion

    #region Parameters

    private InputControls controls = null;

    #endregion

    private void Init()
    {
        Input.multiTouchEnabled = false;

        if (controls == null)
        {
            controls = new InputControls();

            if (Application.platform == RuntimePlatform.OSXPlayer
                || Application.platform == RuntimePlatform.OSXEditor ||
                Application.platform == RuntimePlatform.WindowsPlayer
                || Application.platform == RuntimePlatform.WindowsEditor)
            {
                controls.Mouse.Click.performed += x =>
                    OnTouchPosition?.Invoke(x.ReadValue<Vector2>());
            }
            else
            {
                controls.Touch.TouchPosition.performed +=
                    x => OnTouchPosition?.Invoke(x.ReadValue<Vector2>());
            }

            controls.Touch.Touch.canceled += _ => OnTouchEnd?.Invoke();
        }

        EnhancedTouchSupport.Enable();
        controls.Enable();
    }

    public Vector2 CurrentTouchPos => controls.Touch.TouchPosition.ReadValue<Vector2>();

    public void OnGameStop()
    {
        OnTouchEnd = null;
        OnTouchPosition = null;
        controls.Disable();
    }

    public void OnGameStart() => controls.Enable();
}