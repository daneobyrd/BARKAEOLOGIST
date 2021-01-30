using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class DogMovementController : MonoBehaviour {
    [SerializeField]
    private float baseMovementSpeed = 10f;
    [SerializeField]
    private float sniffSpeedMultiplier = 0.5f;

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
        var sniff = _actions.Player.Sniff.ReadValue<float>();
        var camera = Camera.main;

        var forward = camera.transform.forward;
        var right = camera.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        var speedMultiplier = sniff > 0.5f ? sniffSpeedMultiplier : 1f;
        var speed = baseMovementSpeed * speedMultiplier;
        _characterController.SimpleMove(speed * (forward * move.y + right * move.x));
    }
}
