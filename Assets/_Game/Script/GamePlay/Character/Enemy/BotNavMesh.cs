using UnityEngine;
using UnityEngine.AI;

public class BotNavMesh : MonoBehaviour
{
    private Transform tf;
    public Transform TF
    {
        get
        {
            if(tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }
    [SerializeField] protected NavMeshAgent agent;
    protected Vector3 destination;
    public bool IsDestionation => Vector3.Distance(TF.position, destination + (TF.position.y - destination.y) * Vector3.up) < 0.1f;
    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        agent.SetDestination(destination);
    }
    public void SyncMoveSpeed(float speed)
    {
        if (agent == null) return;
        agent.speed = speed;
    }
}