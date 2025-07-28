using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("Sensitive")]
    [SerializeField] private float Horizontal_Sensitive = 1;
    [SerializeField] private float Verticles_Sensitive = 1;

    [Header("Movement")]
    [SerializeField] private float Speed;
    [SerializeField] private float JumpForce;
    [SerializeField] private float Taking_Distance;

    private Rigidbody Human;
    private Camera Character;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Human = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Character = Camera.main;
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

    private Vector3 _playerRotation;

    void Rotate()
    {
        float Horizontal = Input.GetAxis("Mouse X") * Horizontal_Sensitive;
        float Vertical = Input.GetAxis("Mouse Y") * Verticles_Sensitive;

        print(_playerRotation.x + Vertical * -1);

        _playerRotation.x -= Vertical;
        _playerRotation.x = Mathf.Clamp(_playerRotation.x, -90f, 90f);

        _playerRotation.y += Horizontal;

        transform.rotation = Quaternion.Euler(transform.rotation.x, _playerRotation.y, 0f);
        Character.transform.rotation = Quaternion.Euler(_playerRotation.x, Character.transform.rotation.y, 0f);
    }
}

