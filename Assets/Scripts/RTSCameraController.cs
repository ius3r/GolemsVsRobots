using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]private float moveSpeed = 25f;
    [SerializeField]private float edgeSize = 15f; // Distance from screen edge to start moving

    [Header("Map Limits")]
    [SerializeField]private Vector2 xLimits = new Vector2(-100, 100);
    [SerializeField]private Vector2 zLimits = new Vector2(-100, 100);

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        Vector3 pos = transform.position;

        // Mouse edge movement
        if (Input.mousePosition.x >= Screen.width - edgeSize)
        {
            pos.x += moveSpeed * Time.deltaTime;
        } 
        if (Input.mousePosition.x <= edgeSize)
        {
            pos.x -= moveSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y >= Screen.height - edgeSize)
        {
            pos.z += moveSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= edgeSize)
        {
            pos.z -= moveSpeed * Time.deltaTime;
        }
        if (Input.GetMouseButton(2))
        {
            float h = -Input.GetAxis("Mouse X") * moveSpeed;
            float v = -Input.GetAxis("Mouse Y") * moveSpeed;

            pos += new Vector3(h, 0, v) * Time.deltaTime;
        }

        // Clamp inside map
        pos.x = Mathf.Clamp(pos.x, xLimits.x, xLimits.y);
        pos.z = Mathf.Clamp(pos.z, zLimits.x, zLimits.y);
        Debug.Log($"Camera Position: {pos}");
        transform.position = pos;
    }
}
