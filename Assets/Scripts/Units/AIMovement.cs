using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public sealed class AIMovement : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] private float acquireRadius = 12f;
    [SerializeField] private float retargetIntervalSeconds = 0.25f;
    [SerializeField] private bool includeTriggerColliders = true;

    private readonly Collider[] _overlapHits = new Collider[256];

    private NavMeshAgent _agent;
    private TeamAffiliation _teamAffiliation;
    private CoreTarget _core;

    private Transform _currentTarget;
    private float _nextRetargetTime;
    private Vector3 _guardPosition;

    public Transform CurrentTarget => _currentTarget;
    public Team Team => _teamAffiliation != null ? _teamAffiliation.Team : Team.Neutral;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _teamAffiliation = GetComponent<TeamAffiliation>();
        _guardPosition = transform.position;
    }

    private void Start()
    {
        _core = Object.FindFirstObjectByType<CoreTarget>();
        _nextRetargetTime = Time.time;
    }

    private void Update()
    {
        if (Time.time >= _nextRetargetTime)
        {
            _nextRetargetTime = Time.time + Mathf.Max(0.05f, retargetIntervalSeconds);
            _currentTarget = SelectTarget();
        }

        UpdateDestination();
    }

    private Transform SelectTarget()
    {
        if (Team == Team.Neutral)
        {
            return null;
        }

        // Golems: focus robots if encountered, otherwise path to core.
        if (Team == Team.Golems)
        {
            Transform robot = FindClosestEnemy(Team.Robots);
            if (robot != null) return robot;

            // No robots nearby, go for the core.
            return _core != null ? _core.transform : null;
        }

        // Robots: defend by seeking nearby golems. If none, hold position.
        if (Team == Team.Robots)
        {
            return FindClosestEnemy(Team.Golems);
        }

        return null;
    }

    private void UpdateDestination()
    {
        if (_agent == null || !_agent.enabled) return;

        if (_currentTarget != null)
        {
            _agent.SetDestination(_currentTarget.position);
            return;
        }

        // No target: robots hold their guard spot; golems without a core just stop.
        if (Team == Team.Robots)
        {
            _agent.SetDestination(_guardPosition);
        }
    }

    private Transform FindClosestEnemy(Team enemyTeam)
    {
        QueryTriggerInteraction triggerInteraction = includeTriggerColliders
            ? QueryTriggerInteraction.Collide
            : QueryTriggerInteraction.Ignore;

        int hitCount = Physics.OverlapSphereNonAlloc(
            transform.position,
            Mathf.Max(0f, acquireRadius),
            _overlapHits,
            ~0,
            triggerInteraction
        );

        Transform best = null;
        float bestDistSq = float.PositiveInfinity;

        for (int i = 0; i < hitCount; i++)
        {
            Collider col = _overlapHits[i];
            if (col == null) continue;
            if (col.transform.root == transform.root) continue;

            Health health = col.GetComponentInParent<Health>();
            if (health == null) health = col.GetComponentInChildren<Health>();
            if (health == null || health.IsDead) continue;

            TeamAffiliation otherTeam = health.GetComponentInParent<TeamAffiliation>();
            if (otherTeam == null) otherTeam = health.GetComponentInChildren<TeamAffiliation>();
            Team other = otherTeam != null ? otherTeam.Team : Team.Neutral;
            if (other != enemyTeam) continue;

            float distSq = (health.transform.position - transform.position).sqrMagnitude;
            if (distSq < bestDistSq)
            {
                bestDistSq = distSq;
                best = health.transform;
            }
        }

        return best;
    }
}
