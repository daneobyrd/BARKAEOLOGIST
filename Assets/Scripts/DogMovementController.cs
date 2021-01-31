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

    [SerializeField] private float pantIdleCountdownTime = 5f;

    [SerializeField] private AudioSource barkSource;
    [SerializeField] private AudioSource diggingSource;
    [SerializeField] private AudioSource jumpingSource;
    [SerializeField] private AudioSource sniffingSource;
    [SerializeField] private AudioSource pantingSource;
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioList barkClips;
    [SerializeField] private AudioList diggingClips;
    [SerializeField] private AudioList sniffingClips;
    [SerializeField] private AudioList pantingClips;

    [SerializeField] private AudioTerrainMap footstepClips;
    [SerializeField] private AudioTerrainMap jumpingClips;
    [SerializeField] private AudioTerrainMap slowFootstepClips;

    private CharacterController _characterController;
    private DogActions _actions;
    private Camera _camera;
    private Vector3 _lastMoveDirection;
    private float _idleCountdown;
    private bool _panted = true;
    private bool _sniffed = false;
    private TerrainColor _lastTerrainColor = TerrainColor.Black;

    private static readonly int WalkSpeed = Animator.StringToHash("WalkSpeed");
    private static readonly int Sniffing = Animator.StringToHash("Sniffing");
    private static readonly int Dig = Animator.StringToHash("Dig");
    private static readonly int Jump = Animator.StringToHash("Jump");

    private void Start() {
        _camera = Camera.main;
        _lastMoveDirection = transform.forward;
        _idleCountdown = pantIdleCountdownTime;
    }

    public void OnEnable() {
        if (_actions == null) {
            _actions = new DogActions();
        }

        _actions.Player.Dig.performed += OnDig;
        _actions.Player.Jump.performed += OnJump;
        _actions.Player.Bark.performed += OnBark;

        _actions.Player.Enable();
    }

    private void OnDig(InputAction.CallbackContext obj) {
        if (sniffer.CanDig) {
            animator.SetTrigger(Dig);
            diggingSource.clip = diggingClips.GetRandomClip();
            diggingSource.Play();
        }
    }

    private void OnJump(InputAction.CallbackContext obj) {
        animator.SetTrigger(Jump);
        var terrainColor = GetUnderlyingTerrainColor();
        jumpingSource.clip = jumpingClips.GetMatchingClip(terrainColor);
        jumpingSource.Play();
    }

    private void OnBark(InputAction.CallbackContext obj) {
        barkSource.clip = barkClips.GetRandomClip();
        barkSource.Play();
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
        if (walkDirection.sqrMagnitude > 0f) {
            _lastMoveDirection = walkDirection.normalized;
        }

        var desiredDirection = Quaternion.LookRotation(_lastMoveDirection);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            desiredDirection,
            // Scale the rate of rotation by movement (i.e. only turn when walking)
            rotationRate * Time.deltaTime * walkSpeed
        );

        bool isSniffing = sniffer && sniffer.IsSniffing;

        float speedMultiplier = sniff > 0.5f ? sniffSpeedMultiplier : 1f;
        float speed = baseMovementSpeed * speedMultiplier;
        _characterController.SimpleMove(speed * walkDirection);

        animator.SetFloat(WalkSpeed, walkSpeed);
        animator.SetBool(Sniffing, isSniffing);

        if (!_panted) {
            _idleCountdown -= Time.deltaTime;
            if (_idleCountdown <= 0f) {
                pantingSource.clip = pantingClips.GetRandomClip();
                pantingSource.Play();
                _panted = true;
            }
        }

        if (isSniffing) {
            if (!_sniffed) {
                sniffingSource.clip = sniffingClips.GetRandomClip();
                sniffingSource.Play();
                _sniffed = true;
            }
        }
        else {
            _sniffed = false;
        }

        if (walkSpeed > 0.01f) {
            _idleCountdown = pantIdleCountdownTime;
            _panted = false;

            var terrainColor = GetUnderlyingTerrainColor();
            if (!footstepSource.isPlaying || terrainColor != _lastTerrainColor) {
                if (isSniffing) {
                    footstepSource.clip = slowFootstepClips.GetMatchingClip(terrainColor);
                }
                else {
                    footstepSource.clip = footstepClips.GetMatchingClip(terrainColor);
                }
                footstepSource.Play();
                _lastTerrainColor = terrainColor;
            }
        }
        else {
            if (footstepSource.isPlaying) {
                footstepSource.Stop();
            }
        }

        if (isSniffing) {
            if (!sniffingSource.isPlaying) {
                sniffingSource.clip = sniffingClips.GetRandomClip();
                sniffingSource.Play();
            }
        }
        else {
            if (sniffingSource.isPlaying) {
                sniffingSource.Stop();
            }
        }
    }

    private TerrainColor GetUnderlyingTerrainColor() {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (!Physics.Raycast(ray, out RaycastHit hit)) {
            return TerrainColor.Black;
        }

        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (!meshCollider || !meshCollider.sharedMesh) {
            return TerrainColor.Black;
        }

        Mesh mesh = meshCollider.sharedMesh;
        Color[] colors = mesh.colors;
        int[] triangles = mesh.triangles;

        // Extract local space normals of the triangle we hit
        Color c0 = colors[triangles[hit.triangleIndex * 3 + 0]];
        Color c1 = colors[triangles[hit.triangleIndex * 3 + 1]];
        Color c2 = colors[triangles[hit.triangleIndex * 3 + 2]];

        // interpolate using the barycentric coordinate of the hitpoint
        Vector3 baryCenter = hit.barycentricCoordinate;

        // Use barycentric coordinate to interpolate normal
        Color interpolatedColor = c0 * baryCenter.x + c1 * baryCenter.y + c2 * baryCenter.z;

        return AudioTerrainMap.GetTerrainColor(interpolatedColor);
    }
}
