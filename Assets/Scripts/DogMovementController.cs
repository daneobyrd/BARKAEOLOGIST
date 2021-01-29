using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class DogMovementController : MonoBehaviour {
    private CharacterController _characterController;
    private DogActions _actions;

    public void OnEnable() {
        if (_actions == null) {
            _actions = new DogActions();
        }

        _actions.Player.Enable();
    }

    private void Awake() {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update() {
        var move = _actions.Player.Move.ReadValue<Vector2>();
        var button = _actions.Player.Fire.ReadValue<float>();
        Debug.Log(button);
        _characterController.SimpleMove(10f * (Vector3.forward * move.y + Vector3.right * move.x));
    }

}
