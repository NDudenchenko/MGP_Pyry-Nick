using UnityEngine;
using UnityEngine.UI;

public class NetworkButtonsDeactivator : MonoBehaviour
{
    [SerializeField]
    Button StartHostButton;
    [SerializeField]
    Button StartClientButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartHostButton.onClick.AddListener(DeactivateButton);
        StartClientButton.onClick.AddListener(DeactivateButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void DeactivateButton()
    {
        StartHostButton.gameObject.SetActive(false);
        StartClientButton.gameObject.SetActive(false);
    }

}
