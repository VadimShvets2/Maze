using UnityEngine;

public class Controller : MonoBehaviour
{

    [SerializeField] private float Speed;
    [SerializeField] private float JumpForce;
    [SerializeField] private float Horizontal_Sensitive = 1;
    [SerializeField] private float Verticles_Sensitive = 1;
    private Rigidbody Human;
    [SerializeField] private float Taking_Distance; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Human = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
       Move();
       Rotate();
       Jump();
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Human.AddForce(transform.forward * Speed);  
        }
        if (Input.GetKey(KeyCode.A))
        {
            Human.AddForce(-transform.right * Speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Human.AddForce(transform.right * Speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Human.AddForce(-transform.forward * Speed); 
        }
    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Human.AddForce(transform.up * JumpForce);
        }
    }
    void Rotate()
    {
        Vector3 Rotation = transform.eulerAngles;
        float Horizontal = Input.GetAxis("Mouse X") * Horizontal_Sensitive;
        float Vertical = Input.GetAxis("Mouse Y") * Verticles_Sensitive;
        
        Rotation.y += Horizontal;
        transform.eulerAngles = Rotation;
    }
}

