using UnityEngine;
using UnityEngine.AI;

public class WanderingAgent : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    public float wanderRadius = 20f;
    public Vector2 pauseDurationRange = new Vector2(0.1f, 0.5f); // min and max pause durations
    public float maxStuckTime = 5f; // reroute if stuck for this long

    private bool isPaused = false;
    private float pauseTimer = 0f;
    private float currentPauseDuration = 0f;

    private float stuckTimer = 0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        SetNewRandomDestination();
    }

    private void Update()
    {
        if (isPaused)
        {
            pauseTimer += Time.deltaTime;

            if (pauseTimer >= currentPauseDuration)
            {
                isPaused = false;
                SetNewRandomDestination();
            }

            stuckTimer = 0f; // reset stuck timer when paused
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    if (Random.value < 0.8f)
                    {
                        isPaused = true;
                        pauseTimer = 0f;
                        currentPauseDuration = Random.Range(pauseDurationRange.x, pauseDurationRange.y);
                    }
                    else
                    {
                        SetNewRandomDestination();
                    }

                    stuckTimer = 0f; // reset on reaching
                }
            }
            else
            {
                stuckTimer += Time.deltaTime;

                if (stuckTimer >= maxStuckTime)
                {
                    Debug.LogWarning($"[{name}] Rerouting due to being stuck.");
                    SetNewRandomDestination();
                    stuckTimer = 0f;
                }
            }
        }

        animator?.SetBool("IsWalking", agent.velocity.magnitude > 0.1f);
    }

    private void SetNewRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(navHit.position);
        }
    }
}
