using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DroneMove : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] private float horizontalSpeed = 6f; // A/D
    [SerializeField] private float verticalSpeed = 4f;   // E/Q
    [SerializeField] private float forwardSpeed = 6f;    // W/S
    [SerializeField] private float mouseSensitivity = 3f;

    private Rigidbody rb;

    private float yaw;   // left/right (Y axis)
    private float pitch; // nose tilt (Z axis)

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Collect mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;      // Yaw: rotate left/right
        pitch -= mouseY;    // Pitch: tilt nose up/down
        pitch = Mathf.Clamp(pitch, -45f, 45f);
    }

    private void FixedUpdate()
    {
        // Build rotation for nose = +X
        // Yaw around world Y, Pitch around local Z
        Quaternion targetRotation = Quaternion.Euler(0f, yaw, pitch);
        rb.MoveRotation(targetRotation);

        HandleMovement();
    }

    private void HandleMovement()
    {
        float x = 0f, y = 0f, z = 0f;

        if (Input.GetKey(KeyCode.A)) x = -1f;
        if (Input.GetKey(KeyCode.D)) x = 1f;
        if (Input.GetKey(KeyCode.W)) z = 1f;
        if (Input.GetKey(KeyCode.S)) z = -1f;

        if (Input.GetKey(KeyCode.E)) y = 1f;
        if (Input.GetKey(KeyCode.Q)) y = -1f;

        // Because the drone’s nose is +X
        Vector3 forward = transform.right;       // forward/back
        Vector3 right = -transform.forward;      // strafe
        Vector3 up = Vector3.up;                 // vertical (world up)

        Vector3 move = (right * x * horizontalSpeed) +
                       (forward * z * forwardSpeed) +
                       (up * y * verticalSpeed);

        rb.MovePosition(rb.position + move * Time.fixedDeltaTime);
    }
}
