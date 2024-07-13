using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Deer : MonoBehaviour
{

    public NavMeshAgent agent;
    public Animator animator;

    public float walkRadius;
    public float maxWaitTime;

    [Space]
    public float currentWaitTime;
    public Vector3 currentTargetPosition;
    public Vector3 randomDirection;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentWaitTime = Random.Range(0, maxWaitTime);
        transform.rotation = Quaternion.Euler(0,Random.Range(0,180),0);
    }

    private void Update()
    {
        if(currentWaitTime > 0)
        {
            currentWaitTime -= Time.deltaTime;
            if(currentWaitTime < 0)
            {
                Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
                randomDirection += transform.position;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
                currentTargetPosition = hit.position;

                agent.SetDestination(currentTargetPosition);

                animator.SetBool("isRunning", true);
            }
        }
        else
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        currentWaitTime = Random.Range(0, maxWaitTime);
                        animator.SetBool("isRunning", false);
                    }
                }
            }
            else
            {
                animator.SetBool("isRunning", true);
            }
        }
    }
}
