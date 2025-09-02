using UnityEngine;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem; // New Input System
#endif

[RequireComponent(typeof(Rigidbody))]

public class DroneMove : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] private float horizontalSpeed = 6f; // WASD
    [SerializeField] private float verticalSpeed = 4f;   // E/Q
    [SerializeField] private bool moveInLocalSpace = true; // Move relative to drone facing

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;   // Drones don’t fall
        rb.isKinematic = true;   // We’ll move via MovePosition for smooth triggers
    }

    private void FixedUpdate()
    {
        Vector3 input = ReadInput(); // x = A/D, y = E/Q, z = W/S

        // Horizontal (XZ)
        Vector3 horizontal = new Vector3(input.x, 0f, input.z);
        if (moveInLocalSpace)
            horizontal = transform.TransformDirection(horizontal);

        // Normalize to keep consistent speed diagonally
        float horizontalMag = Mathf.Clamp01(new Vector2(input.x, input.z).magnitude);
        Vector3 horizontalMove = horizontal.normalized * (horizontalSpeed * horizontalMag);

        // Vertical (Y)
        Vector3 verticalMove = Vector3.up * input.y * verticalSpeed;

        // Final movement this physics step
        Vector3 delta = (horizontalMove + verticalMove) * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + delta);
    }

    private Vector3 ReadInput()
    {
        float x = 0f, y = 0f, z = 0f;

#if ENABLE_INPUT_SYSTEM
        var kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.aKey.isPressed) x -= 1f;
            if (kb.dKey.isPressed) x += 1f;
            if (kb.wKey.isPressed) z += 1f;
            if (kb.sKey.isPressed) z -= 1f;
            if (kb.eKey.isPressed) y += 1f;
            if (kb.qKey.isPressed) y -= 1f;
        }
#else
        x = Input.GetAxisRaw("Horizontal"); // A/D
        z = Input.GetAxisRaw("Vertical");   // W/S
        if (Input.GetKey(KeyCode.E)) y += 1f;
        if (Input.GetKey(KeyCode.Q)) y -= 1f;
#endif

        return new Vector3(x, y, z);
    }
}