using UnityEngine;

public class TowerPlatform : MonoBehaviour
{
    [SerializeField] private Material normalMaterial, hoveredMaterial, selectedMaterial;
    private Renderer platformRenderer;
    private Tower currentTower;

    private bool isSelected;
    private bool isHovered;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        platformRenderer = GetComponent<Renderer>();
        ApplyMaterial();
    }

    private void OnMouseEnter()
    {
        hover();
    }

    private void OnMouseExit()
    {
        unhover();
    }

    public void select()
    {
        isSelected = true;
        ApplyMaterial();
    }

    public void unselect()
    {
        isSelected = false;
        ApplyMaterial();
    }

    public void hover()
    {
        isHovered = true;
        ApplyMaterial();
    }

    public void unhover()
    {
        isHovered = false;
        ApplyMaterial();
    }

    private void ApplyMaterial()
    {
        if (platformRenderer == null) return;

        if (isSelected && selectedMaterial != null)
        {
            platformRenderer.material = selectedMaterial;
            return;
        }

        if (isHovered && hoveredMaterial != null)
        {
            platformRenderer.material = hoveredMaterial;
            return;
        }

        if (normalMaterial != null)
        {
            platformRenderer.material = normalMaterial;
        }
    }

    public void buildTower(GameObject towerType)
    {
        Debug.Log("Building Barracks");
        if (currentTower != null) return;
        currentTower = GameObject.Instantiate(towerType, 
            new Vector3(transform.position.x, TowerBuilding.buildHeight, transform.position.z), 
            Quaternion.identity).GetComponent<Tower>();
        Debug.Log("Barracks at: " + new Vector3(transform.position.x, TowerBuilding.buildHeight, transform.position.z));
    }

    public void rotateTower()
    {
        if (currentTower == null) return;
        currentTower.gameObject.transform.Rotate(Vector3.up * 90f);
    }

}
