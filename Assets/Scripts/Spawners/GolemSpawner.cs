using System.Collections;
using UnityEngine;

public sealed class GolemSpawner : MonoBehaviour
{
    [Header("Prefab / Spawn")]
    [SerializeField] private GameObject golemPrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Difficulty (spawn more frequently over time)")]
    [SerializeField] private float baseSpawnIntervalSeconds = 6f;
    [SerializeField] private float minSpawnIntervalSeconds = 1.5f;
    [SerializeField] private float rampDurationSeconds = 180f;
    [SerializeField] private AnimationCurve rampCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private Coroutine _loop;
    private float _startTime;

    private void OnEnable()
    {
        _startTime = Time.time;
        _loop = StartCoroutine(SpawnLoop());
    }

    private void OnDisable()
    {
        if (_loop != null)
        {
            StopCoroutine(_loop);
            _loop = null;
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            float wait = GetCurrentSpawnIntervalSeconds();
            if (wait > 0f)
            {
                yield return new WaitForSeconds(wait);
            }
            else
            {
                yield return null;
            }

            SpawnOne();
        }
    }

    private float GetCurrentSpawnIntervalSeconds()
    {
        float baseInterval = Mathf.Max(0.05f, baseSpawnIntervalSeconds);
        float minInterval = Mathf.Clamp(minSpawnIntervalSeconds, 0.05f, baseInterval);

        if (rampDurationSeconds <= 0.01f) return minInterval;

        float elapsed = Time.time - _startTime;
        float t = Mathf.Clamp01(elapsed / rampDurationSeconds);
        float eased = Mathf.Clamp01(rampCurve != null ? rampCurve.Evaluate(t) : t);

        return Mathf.Lerp(baseInterval, minInterval, eased);
    }

    private void SpawnOne()
    {
        if (golemPrefab == null) return;

        Transform point = spawnPoint != null ? spawnPoint : transform;
        Instantiate(golemPrefab, point.position, point.rotation);
    }

    private void OnValidate()
    {
        baseSpawnIntervalSeconds = Mathf.Max(0.05f, baseSpawnIntervalSeconds);
        minSpawnIntervalSeconds = Mathf.Max(0.05f, minSpawnIntervalSeconds);
        rampDurationSeconds = Mathf.Max(0f, rampDurationSeconds);
    }
}
