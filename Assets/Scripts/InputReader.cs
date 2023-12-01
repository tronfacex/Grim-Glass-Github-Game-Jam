using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls;

    public Vector2 MovementValue { get; private set; }

    public event Action JumpEvent;
    public event Action JumpCutoffEvent;
    public event Action DodgeEvent;
    public event Action AttackEvent;
    public event Action SprintEvent;
    public event Action FloatEvent;
    public event Action FloatCancelledEvent;
    public event Action PauseEvent;

    private float pauseButtonCooldown = .3f;
    private bool pausingAllowed = true;

    private void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);

        controls.Player.Enable();
    }

    private void OnDestroy()
    {
        controls.Player.Disable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (GameDataReader.Instance.GameData.CurrentTimeScale < 1) { return; }
        if (context.started)
        {
            JumpEvent?.Invoke();
        }
        if (context.canceled)
        {
            //Debug.Log("Button Released!");
            JumpCutoffEvent?.Invoke();
        }

    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (GameDataReader.Instance.GameData.CurrentTimeScale < 1) { return; }
        if (!context.performed) { return; }
        DodgeEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {

    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (GameDataReader.Instance.GameData.CurrentTimeScale < 1) { return; }
        if (!GameDataReader.Instance.GameData.HasMetBobby) { return; }
        if (!context.performed) { return; }
        AttackEvent?.Invoke();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        SprintEvent?.Invoke();
    }

    public void OnFloat(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            FloatEvent?.Invoke();
        }
        if (context.canceled)
        {
            FloatCancelledEvent?.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (GameDataReader.Instance.GameData.InGameOver)
        {
            return;
        }
        if (!context.performed) { return; }
        PauseEvent?.Invoke();
        pauseButtonCooldown = .3f;
        pausingAllowed = false;
    }

    private void Update()
    {
        pauseButtonCooldown = Mathf.Max(pauseButtonCooldown - Time.deltaTime, 0f);
        if (pauseButtonCooldown == 0 && !pausingAllowed)
        {
            pausingAllowed = true;
        }
    }

    public void AllowPausing()
    {
        pausingAllowed = true;
    }
    public void DisallowPausing()
    {
        pauseButtonCooldown = .6f;
        pausingAllowed = false;
    }
}
