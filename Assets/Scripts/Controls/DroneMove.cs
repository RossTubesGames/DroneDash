using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class DroneMove : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] private float horizontalSpeed = 6f; // WASD
    [SerializeField] private float verticalSpeed = 4f;   // E/Q

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;   // Drone floats
        rb.isKinematic = true;   // We move it manually
    }

    private void FixedUpdate()
    {
        // Get input
        float x = 0f, y = 0f, z = 0f;

        if (Input.GetKey(KeyCode.A)) x = -1f;
        if (Input.GetKey(KeyCode.D)) x = 1f;
        if (Input.GetKey(KeyCode.W)) z = 1f;
        if (Input.GetKey(KeyCode.S)) z = -1f;

        if (Input.GetKey(KeyCode.E)) y = 1f;
        if (Input.GetKey(KeyCode.Q)) y = -1f;

        // Build movement vector
        Vector3 move = new Vector3(x * horizontalSpeed, y * verticalSpeed, z * horizontalSpeed);

        // Apply movement
        rb.MovePosition(rb.position + move * Time.fixedDeltaTime);
    }
}