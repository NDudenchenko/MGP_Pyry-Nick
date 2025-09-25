using UnityEngine;

public class SpectatorMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float mouseSens;
    [SerializeField] private Transform cam;

    [SerializeField] private float flyForce;
    private CharacterController charCon;

    private float xRotation = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        charCon = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
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

        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(Vector3.up * flyForce * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.Translate(Vector3.down * flyForce * Time.deltaTime);
        }
    }
}
