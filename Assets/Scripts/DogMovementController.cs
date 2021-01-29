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

        // TODO put this someplace that makes more sense, like when clicking a start game button
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void Update() {
        var move = _actions.Player.Move.ReadValue<Vector2>();
        var button = _actions.Player.Fire.ReadValue<float>();
        var camera = Camera.main;

        var forward = camera.transform.forward;
        var right = camera.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        _characterController.SimpleMove(10f * (forward * move.y + right * move.x));
    }

}
