using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIMovement))]
public sealed class AICombat : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackRange = 2.0f;
    [SerializeField] private float attackCooldownSeconds = 1.0f;
    [SerializeField] private float damagePerAttack = 10f;
    [SerializeField] private bool stopToAttack = true;

    [Header("Audio")]
    [SerializeField] private AudioClip attackSfx;
    [SerializeField, Range(0f, 1f)] private float attackSfxVolume = 1f;

    private AIMovement _movement;
    private NavMeshAgent _agent;
    private float _nextAttackTime;

    public bool IsAttacking { get; private set; }

    private void Awake()
    {
        _movement = GetComponent<AIMovement>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Transform target = _movement.CurrentTarget;
        if (target == null)
        {
            IsAttacking = false;
            if (_agent != null && stopToAttack) _agent.isStopped = false;
            return;
        }

        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth == null || targetHealth.IsDead)
        {
            IsAttacking = false;
            if (_agent != null && stopToAttack) _agent.isStopped = false;
            return;
        }

        // Safety: never attack your own team.
        Team myTeam = _movement.Team;
        TeamAffiliation otherTeamAff = targetHealth.GetComponentInParent<TeamAffiliation>();
        Team otherTeam = otherTeamAff != null ? otherTeamAff.Team : Team.Neutral;
        if (myTeam != Team.Neutral && otherTeam == myTeam)
        {
            IsAttacking = false;
            if (_agent != null && stopToAttack) _agent.isStopped = false;
            return;
        }

        float range = Mathf.Max(0f, attackRange);
        float distSq = (target.position - transform.position).sqrMagnitude;
        bool inRange = distSq <= range * range;

        IsAttacking = inRange;

        if (stopToAttack && _agent != null)
        {
            _agent.isStopped = inRange;
        }

        if (!inRange)
        {
            return;
        }

        FaceTarget(target.position);

        if (Time.time < _nextAttackTime) return;
        _nextAttackTime = Time.time + Mathf.Max(0.01f, attackCooldownSeconds);

        if (attackSfx != null)
        {
            AudioController.Instance?.PlaySfxOneShot(attackSfx, attackSfxVolume);
        }

        targetHealth.TakeDamage(damagePerAttack);
    }

    private void FaceTarget(Vector3 targetWorldPos)
    {
        Vector3 dir = targetWorldPos - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 20f);
    }
}
