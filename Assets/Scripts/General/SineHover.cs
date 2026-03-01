using UnityEngine;

public class SineHover : MonoBehaviour
{
    [SerializeField] private float amplitude;
    [SerializeField] private float period;
    private float timeCount;
    private float startY;

    void Start()
    {
        period = (2f * Mathf.PI) / period;
        startY = transform.position.y;
    }


    void FixedUpdate()
    {
        timeCount += Time.deltaTime;
        transform.position = new Vector3(transform.position.x, amplitude * Mathf.Sin(period * timeCount) + startY, transform.position.z);
    }
}
