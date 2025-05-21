using UnityEngine;
using UnityEngine.AI;

public class SquirrelWander : MonoBehaviour
{
    public float wanderRadius = 5f;
    public float waitTimeMin = 0.5f;
    public float waitTimeMax = 2f;

    private NavMeshAgent agent;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToNewPosition();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.2f && !isWaiting)
        {
            // Àá±ñ ¸ØÃß±â
            isWaiting = true;
            waitTimer = Random.Range(waitTimeMin, waitTimeMax);
            Invoke(nameof(MoveToNewPosition), waitTimer);
        }
    }

    void MoveToNewPosition()
    {
        Vector3 newPos = RandomNavmeshLocation(wanderRadius);
        agent.SetDestination(newPos);
        isWaiting = false;
    }

    Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return transform.position;
    }
}
