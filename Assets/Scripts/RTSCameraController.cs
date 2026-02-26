using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]private float moveSpeed = 30f;
    [SerializeField]private float edgeSize = 75f; // Distance from screen edge to start moving

    [Header("Map Limits")]
    [SerializeField]private Vector2 xLimits = new Vector2(-100, 100);
    [SerializeField]private Vector2 zLimits = new Vector2(-100, 100);

    [Header("Unit Selection")]
    [SerializeField]private LayerMask unitLayer;
    [SerializeField]private float UnitRotationSpeed = 100f;

    private Camera cam;
    private List<GameObject> selectedUnits = new List<GameObject>();
    private GameObject selectedUnit;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
       HandleMovement();
        SelectUnits();
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
            if(selectedUnit != null)
            {
                RotateAroundTarget();
            }
        }

        // Clamp inside map
        pos.x = Mathf.Clamp(pos.x, xLimits.x, xLimits.y);
        pos.z = Mathf.Clamp(pos.z, zLimits.x, zLimits.y);
        transform.position = pos;
    }

    void SelectUnits()
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach (var unit in selectedUnits)
            {
                //Deselect previously selected units
            }
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, unitLayer))
            {
                if (hit.collider)
                {
                    selectedUnit = hit.collider.gameObject;
                    if (!selectedUnits.Contains(hit.collider.gameObject))
                    {
                        selectedUnits.Add(hit.collider.gameObject);
                        Debug.Log("Selected unit: " + hit.collider.gameObject.name);
                    }
                }
            }
        }
    }
    void RotateAroundTarget()
    {
        transform.RotateAround(selectedUnit.transform.position, Vector3.up, 1f * UnitRotationSpeed * Time.deltaTime);
    }
}
