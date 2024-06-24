using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunnerController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    public void SetTarget(Vector3 target) => agent.SetDestination(target);
    public bool ReachedTarget() => agent.remainingDistance <= 0.5f || agent.pathStatus == NavMeshPathStatus.PathPartial;
    public void SetSpeed(float speed) => agent.speed = speed;
}
