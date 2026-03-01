using UnityEngine;

public class MinimapController : MonoBehaviour
{
    [SerializeField] private Transform player;

    void Update()
    {
        if (player == null) return;
        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
    }
}