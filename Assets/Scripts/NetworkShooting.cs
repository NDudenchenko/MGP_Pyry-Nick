using Unity.Netcode;
using UnityEngine;

public class NetworkShooting : NetworkBehaviour
{

    [SerializeField] private float range;
    [SerializeField] private float rateOfFire;

    [SerializeField] private int magazineSize;
    [SerializeField] private int currentAmmo;
    [SerializeField] private int reserveAmmo;
    [SerializeField] private int maxAmmo;

    [SerializeField] private Transform fpsCam;

    [SerializeField] private LayerMask hitLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && currentAmmo > 0)
            {
                if (Physics.Raycast(fpsCam.position, fpsCam.forward, out RaycastHit hit, range, hitLayer))
                {
                    Debug.Log("Hit Player!");
                }
                    //currentAmmo--;
            }
        }
    }
}
