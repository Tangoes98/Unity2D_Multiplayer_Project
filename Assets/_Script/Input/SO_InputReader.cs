using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/ Input Reader")]
public class SO_InputReader : ScriptableObject, IPlayerActions
{
    public event Action<bool> PrimaryFireEvent;
    public event Action<Vector2> PrimaryMoveEvent;

    public Vector2 AimPosition { get; private set; }

    Controls _controls;

    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
        }

        _controls.Player.Enable();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed) PrimaryFireEvent?.Invoke(true);
        else PrimaryFireEvent?.Invoke(false);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        PrimaryMoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        AimPosition = context.ReadValue<Vector2>();
    }
}
