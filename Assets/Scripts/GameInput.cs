using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;

public class GameInput : MonoBehaviour {
    public static GameInput Instance { get; private set; }
    
    private InputActions inputActions;

    public event EventHandler OnLeftMouse;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one GameInput instance.");
        } 
        else {
            Instance = this;
        }
        
        inputActions = new InputActions();
    }

    private void OnEnable() {
        inputActions.Player.Enable();

        inputActions.Player.Shoot.performed += LeftMouse_Pressed;
    }

    private void OnDisable() {
        inputActions.Player.Disable();
    }

    public float GetMovementInput() {
        return inputActions.Player.Movement.ReadValue<float>();
    }

    private void LeftMouse_Pressed(InputAction.CallbackContext ctx) {
        OnLeftMouse?.Invoke(this, EventArgs.Empty);
    }
    
}
