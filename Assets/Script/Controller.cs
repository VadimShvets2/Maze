using UnityEngine;

public class Controller : MonoBehaviour 
{
    [Header("Movement")]
    public float speed = 5f;
    public float jumpHeight = 8f;
    public float gravity = -20f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public Camera cam;

    [Header("Ground Check")]
    public float groundCheckDistance = 0.3f;
    public LayerMask groundMask = ~0; // Default to everything

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleJump();
    }

    void HandleMouseLook()
    {
        // Get mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        // Rotate player body left and right
        transform.Rotate(Vector3.up * mouseX);
        
        // Rotate camera up and down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void HandleMovement()
    {
        // Get WASD input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        // Move relative to where player is facing
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
    }

    void HandleJump()
    {
        // Custom ground check - more reliable than controller.isGrounded
        bool grounded = IsGrounded();
        
        // DEBUG: Remove this after fixing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"SPACE PRESSED! Grounded: {grounded}, Velocity.y: {velocity.y}");
        }
        
        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        
        // Reset velocity when grounded
        if (grounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small negative value to keep grounded
        }
        
        // Jump when Space is pressed and grounded - using KeyCode instead of Input.GetButtonDown
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Debug.Log($"JUMPING! New velocity.y: {velocity.y}"); // DEBUG
        }
        
        // Apply vertical movement
        controller.Move(velocity * Time.deltaTime);
    }

    bool IsGrounded()
    {
        // Cast a sphere from the bottom of the CharacterController
        Vector3 spherePosition = new Vector3(
            transform.position.x,
            transform.position.y - (controller.height / 2f) - controller.center.y,
            transform.position.z
        );
        return Physics.CheckSphere(spherePosition, groundCheckDistance, groundMask);
    }

    // Optional: Visualize the ground check in the editor
    void OnDrawGizmosSelected()
    {
        if (controller == null) return;
        
        Vector3 spherePosition = new Vector3(
            transform.position.x,
            transform.position.y - (controller.height / 2f) - controller.center.y,
            transform.position.z
        );
        
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(spherePosition, groundCheckDistance);
    }
}