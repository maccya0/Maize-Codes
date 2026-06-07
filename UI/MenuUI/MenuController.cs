using System;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private InputSystem_Actions inputActions;

    public Action<InputSystem_Actions> InitInputAction;
    public Action DestroyAction;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();
        inputActions.UI.Enable();
        inputActions.Player.Disable();
        Time.timeScale = 1.0f;
    }
    private void Start()
    {
        InitInputAction?.Invoke(inputActions);
    }

    private void OnDestroy()
    {
        DestroyAction?.Invoke();
        inputActions.Disable();
    }
}
