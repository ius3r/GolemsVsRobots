using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// WILL REPLACE WITH PROPER ANIMATION CONTROLLER LATER.
/// Minimal animation switching: swaps between a walking controller and an attacking controller.
/// Assign two RuntimeAnimatorController assets in the inspector.
/// </summary>
[RequireComponent(typeof(Animator))]
public sealed class AIAnimationController : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController idleController;
    [SerializeField] private RuntimeAnimatorController walkingController;
    [SerializeField] private RuntimeAnimatorController attackingController;

    private Animator _animator;
    private AICombat _combat;
    private NavMeshAgent _agent;

    private Vector3 _lastPosition;

    private RuntimeAnimatorController _lastController;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _combat = GetComponent<AICombat>();
        _agent = GetComponent<NavMeshAgent>();
        _lastPosition = transform.position;
    }

    private void Update()
    {
        RuntimeAnimatorController desired = idleController;

        bool isAttacking = _combat != null && _combat.IsAttacking;
        bool isMoving = IsMoving();

        if (isAttacking)
        {
            desired = attackingController != null
                ? attackingController
                : (walkingController != null ? walkingController : idleController);
        }
        else if (isMoving)
        {
            desired = walkingController != null ? walkingController : idleController;
        }

        if (desired != null && desired != _lastController)
        {
            _animator.runtimeAnimatorController = desired;
            _lastController = desired;
        }
    }

    private bool IsMoving()
    {
        // Prefer NavMeshAgent velocity when available.
        if (_agent != null && _agent.enabled)
        {
            Vector3 v = _agent.velocity;
            v.y = 0f;
            return v.sqrMagnitude > 0.01f;
        }

        Vector3 pos = transform.position;
        Vector3 delta = pos - _lastPosition;
        delta.y = 0f;
        _lastPosition = pos;
        return delta.sqrMagnitude > 0.0001f;
    }
}
