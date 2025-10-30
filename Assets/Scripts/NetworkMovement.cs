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

    private bool playerFrozen;

    private float xRotation = 0f;
    private float verticalVelocity;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private bool isGrouneded;

    [SerializeField] private LayerMask groundLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        charCon = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gravityForce = 0;
    }

    private void Start()
    {
        playerFrozen = true;
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

            if (Input.GetKeyDown(KeyCode.Space) && isGrouneded)
            {
                verticalVelocity = Mathf.Sqrt(jumpForce * -2.0f * gravityForce);
            }

            if (Input.GetKeyDown(KeyCode.W) && playerFrozen)
            {
                gravityForce = 9.1f;
                playerFrozen = false;
            }

            isGrouneded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);

            if (isGrouneded)
            {
                verticalVelocity = 0;
            }
            else
            {
                verticalVelocity += gravityForce;
            }

            transform.Translate(Vector3.down * verticalVelocity * Time.deltaTime);

        }
    }
    
    public override void OnNetworkSpawn()
    {
        SyncTransform_ServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SyncTransform_ServerRpc()
    {
        UpdateTransform_ClientRpc(transform.position, transform.rotation);
    }

    [ClientRpc]
    public void UpdateTransform_ClientRpc(Vector3 position, Quaternion rotation)
    {
        if (!IsOwner) return;
            
        transform.position = position;
        transform.rotation = rotation;
    }
}
