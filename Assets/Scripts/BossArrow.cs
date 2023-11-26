using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossArrow : Arrow
{
    public Transform target;
    NavMeshAgent nav;
    
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(target.position); // 타겟(플레이어)추척함.
    }
}
