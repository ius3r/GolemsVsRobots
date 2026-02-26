using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TowerBuilding : MonoBehaviour
{
    public InputActionAsset inputActions;
    public static float buildHeight;
    private InputAction clickAction, buildAction, rotateAction, mousePosition;
    [SerializeField] private LayerMask rayLayer;
    [SerializeField] private GameObject barraksTower;

    private TowerPlatform selectedPlatform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buildHeight = 4.5f;
        clickAction = inputActions.FindAction("MouseClick");
        clickAction.performed += CastMouseRay;
        mousePosition = inputActions.FindAction("MousePosition");
        buildAction = inputActions.FindAction("Build");
        buildAction.performed += TryBuild;
        rotateAction = inputActions.FindAction("Rotate");
        rotateAction.performed += TryRotate;
    }
    private void CastMouseRay(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector3 origin = mousePosition.ReadValue<Vector2>();
            origin.z = 100f;
            Ray mouseRay = Camera.main.ScreenPointToRay(origin);
            RaycastHit hit;
            if (Physics.Raycast(mouseRay, out hit, 900, rayLayer))
            {
                if (hit.collider.CompareTag("Platform"))
                {
                    Debug.Log(hit.point);
                    if (selectedPlatform != null) selectedPlatform.unselect();
                    selectedPlatform = hit.collider.gameObject.GetComponent<TowerPlatform>();
                    selectedPlatform.select();
                }
            }
            else if (selectedPlatform != null) { selectedPlatform.unselect(); selectedPlatform = null; }
            ;
        }
    }

    private void TryRotate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (selectedPlatform != null)
                selectedPlatform.rotateTower();
        }
    }

    private void TryBuild(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Build Performed");
            if (selectedPlatform != null)
                selectedPlatform.buildTower(barraksTower);
        }
    }

    /*
    private void FixedUpdate()
    {
        Vector3 mus = mousePosition.ReadValue<Vector2>();
        mus.z = 100f;
        mus = Camera.main.ScreenToWorldPoint(mus);
        Debug.DrawRay(Camera.main.transform.position, mus - Camera.main.transform.position, Color.cyan);
    }
    */
}
