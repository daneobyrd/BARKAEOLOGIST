using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class DogMovementController : MonoBehaviour {
    [SerializeField] private float baseMovementSpeed = 10f;
    [SerializeField] private float sniffSpeedMultiplier = 0.5f;

    [Tooltip("Desired rotation rate in degrees per second")] [SerializeField]
    private float rotationRate = 180f;

    [SerializeField] private Animator animator;

    [SerializeField] private Sniff sniffer;

    private CharacterController _characterController;
    private DogActions _actions;
    private Camera _camera;

    private static readonly int WalkSpeed = Animator.StringToHash("WalkSpeed");
    private static readonly int Sniffing = Animator.StringToHash("Sniffing");
    private static readonly int Dig = Animator.StringToHash("Dig");
    private static readonly int Jump = Animator.StringToHash("Jump");

    private void Start() {
        _camera = Camera.main;
    }

    public void OnEnable() {
        if (_actions == null) {
            _actions = new DogActions();
        }

        _actions.Player.Dig.performed += OnDig;
        _actions.Player.Jump.performed += OnJump;

        _actions.Player.Enable();
    }

    private void OnDig(InputAction.CallbackContext obj) {
        animator.SetTrigger(Dig);
    }

    private void OnJump(InputAction.CallbackContext obj) {
        animator.SetTrigger(Jump);
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

        var forward = _camera.transform.forward;
        var right = _camera.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        float walkSpeed = move.magnitude;
        var walkDirection = forward * move.y + right * move.x;

        var desiredDirection = Quaternion.LookRotation(walkDirection.normalized);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            desiredDirection,
            // Scale the rate of rotation by movement (i.e. only turn when walking)
            rotationRate * Time.deltaTime * walkSpeed
        );

        var speedMultiplier = sniff > 0.5f ? sniffSpeedMultiplier : 1f;
        var speed = baseMovementSpeed * speedMultiplier;
        _characterController.SimpleMove(speed * walkDirection);

        animator.SetFloat(WalkSpeed, walkSpeed);
        if (sniffer) {
            animator.SetBool(Sniffing, sniffer.IsSniffing);
        }
    }
}
