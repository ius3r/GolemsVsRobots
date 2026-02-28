using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private Camera mainCam;
    [SerializeField] private float panSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mainCam.transform.RotateAround(mainCam.transform.position, Vector3.up, panSpeed);
    }
}
