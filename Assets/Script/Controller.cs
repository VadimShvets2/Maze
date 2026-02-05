using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Controller : MonoBehaviour 
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintSpeed = 15f;
    [SerializeField] private float jumpHeight = 8f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Mouse Look")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private Camera cam;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.3f;
    [SerializeField] private LayerMask groundMask = ~0; // Default to everything

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
        
        float currentSpeed = Input.GetKey(KeyCode.LeftControl) ? sprintSpeed : speed;
        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    void HandleJump()
    {
        // Custom ground check
        bool grounded = IsGrounded();
        
        // Apply gravity
        if (velocity.y < 0)
        {
            velocity.y += gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (velocity.y > 0 && !Input.GetButton("Jump"))
        {
            velocity.y += gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        
        velocity.y += gravity * Time.deltaTime;
        
        // Reset velocity when grounded
        if (grounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small negative value to keep grounded
        }
        
        // Jump when button is pressed and grounded
        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        
        // Apply vertical movement
        controller.Move(velocity * Time.deltaTime);
    }

    bool IsGrounded()
    {
        // Cast a sphere from the bottom of the CharacterController
        Vector3 spherePosition = new Vector3(
            transform.position.x,
            transform.position.y + controller.center.y - (controller.height / 2f),
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
            transform.position.y + controller.center.y - (controller.height / 2f),
            transform.position.z
        );
        
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(spherePosition, groundCheckDistance);
    }
}