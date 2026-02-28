using UnityEngine;

public class TowerPlatform : MonoBehaviour
{
    [SerializeField] private Material normalMaterial, selectedMaterial;
    private Renderer platformRenderer;
    private Tower currentTower;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        platformRenderer = GetComponent<Renderer>();
    }

    public void select()
    {
        platformRenderer.material = selectedMaterial;
    }

    public void unselect()
    {
        platformRenderer.material = normalMaterial;
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
