using UnityEngine;
using UnityEngine.AI;
[RequireComponent (typeof(NavMeshAgent))]

public class AgentMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }

    public void SetTarget(Vector3 _target)
    {
        agent.SetDestination(new Vector3(_target.x, _target.y, _target.z));
    }

    public void StopAgentMovement(bool state)
    {
        agent.isStopped = state;
    }


}
