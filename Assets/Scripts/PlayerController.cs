using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
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
    private List<GameObject> selectedUnits = new List<GameObject>();
    private GameObject selectedUnit;

    [Header("Towers")]
    [SerializeField] private List<GameObject> TowerPrefabs = new List<GameObject>();

    [Header("Crystals")]
    [SerializeField] private int Crystals = 100;
    [SerializeField] [Range(0, 100)] private int CrystalsPerSecond = 1;

    [Header("UI")]
    [SerializeField] private Canvas PlayerCanvas;
    [SerializeField] private Text CrystalText;

    private Camera cam;

    public void SetCrystals(int amount)
    {
        Crystals = amount;
        CrystalText.text = "Crystals: " + Crystals;
    }

    private void Awake()
    {
        if (TowerPrefabs.Count == 0)
        {
            Debug.LogWarning("No tower prefabs assigned in the inspector.");
        }
    }
  
    void Start()
    {
        cam = GetComponent<Camera>();
        PlayerCanvas = GetComponent<Canvas>();
        StartCoroutine(GenerateCrystalsOverTime());
    }

    void Update()
    {
        HandleMovement();
        SelectUnits();
        SpawnTowers();

    }

    void HandleMovement()
    {
        Vector3 pos = transform.position;
        if (Input.GetMouseButton(2))
        {
            if(selectedUnit != null)
            {
                RotateAroundTarget();
            }
            Vector3 forward = transform.forward;
            if (Input.mousePosition.y >= Screen.height - edgeSize)
            {
                pos += forward * moveSpeed * Time.deltaTime;
            }
            if (Input.mousePosition.y <= edgeSize)
            {
                pos -= forward * moveSpeed * Time.deltaTime;
            }
        }
        else
        {
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


    void SpawnTowers()
    {
        if (Input.GetMouseButtonDown(1) && TowerPrefabs.Count > 0)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.collider)
                {
                    Vector3 spawnPos = hit.point;
                    spawnPos.y += 0.5f; // Adjust height if needed
                    GameObject towerToSpawn = TowerPrefabs[Random.Range(0, TowerPrefabs.Count)];
                    Instantiate(towerToSpawn, spawnPos, Quaternion.identity);
                    Debug.Log("Spawned tower at: " + spawnPos);
                }
            }
        }
    }

   IEnumerator<WaitForSeconds> GenerateCrystalsOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Crystals += CrystalsPerSecond;
            CrystalText.text = "Crystals: " + Crystals;
            Debug.Log("Crystals: " + Crystals);
        }
    } 
}
