using UnityEngine;
using Unity.Netcode;

public class NetworkMovement : NetworkBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float mouseSens;
    [SerializeField] private Transform cam;

    [SerializeField] private float jumpForce;
    private float gravityForce = 9.1f;
    private CharacterController charCon;

    private float xRotation = 0f;
    private Vector3 velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        charCon = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        cam.gameObject.SetActive(IsOwner);
    }



    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            float x = Input.GetAxis("Horizontal");   
            float z = Input.GetAxis("Vertical");     

            Vector3 move = transform.right * x + transform.forward * z;
            charCon.Move(move * moveSpeed * Time.deltaTime);



            float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

            // Vertical rotation (camera pitch)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // Horizontal rotation (player yaw)
            transform.Rotate(Vector3.up * mouseX);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                transform.Translate(Vector3.up * jumpForce);
            }
        }
    }
}
