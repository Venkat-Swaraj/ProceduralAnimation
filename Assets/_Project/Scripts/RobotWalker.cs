using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class RobotWalker : MonoBehaviour
{
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(new Vector3(Random.Range(25f, 50f), 0, Random.Range(25f, 50f)));
    }

    private void Update()
    {
        if (agent.remainingDistance == 0)
        {
            agent.SetDestination(new Vector3(Random.Range(25f, 50f), 0, Random.Range(25f, 50f)));
        }
    }
}
