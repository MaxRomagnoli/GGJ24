using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AiFollower : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;
    [SerializeField] private float minDistance = 2f;

    void FixedUpdate()
    {
        if(target == null) { return; }

        Vector3 direction = target.transform.position - this.transform.position;
        if(minDistance < 0 || direction.sqrMagnitude < minDistance) { return; }

        // NavMeshPath path = agent.CalculatePath(target.position, path);
        // agent.SetPath(path);
        agent.SetDestination(target.position - (direction.normalized * minDistance));
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }
}
