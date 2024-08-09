using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyMoveAgent : MonoBehaviour
{
    public List<Transform> wayPointList;
    public int nextIndex = 0;
    [SerializeField] private NavMeshAgent agent;
    private Rigidbody rb;
    private EnemyAI enemyAI;

    private readonly float patrollSpeed = 2.5f;
    private readonly float traceSpeed = 4.0f;
    private bool _patrolling;
    private Transform enemyTr;
    public bool patrolling
    {
        get { return _patrolling; }
        set
        {
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = patrollSpeed;

                MovewayPoint();
            }
        }
    }
    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;

            TraceTarget(_traceTarget);
        }
    }

    public float speed
    {
        get { return agent.velocity.magnitude; }
        
    }


    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        enemyTr = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        agent.autoBraking = false;
        agent.updateRotation = false;
        var group = GameObject.Find("Points");
        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPointList);
            wayPointList.RemoveAt(0);
        }
        nextIndex = Random.Range(0, wayPointList.Count);
        MovewayPoint();
    }

    void Update()
    {
        if (!agent.isStopped && !enemyAI.isDie)
        {
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * 7);
        }
        if (!_patrolling) return;
        FindWayPoint();
    }

    private void FindWayPoint()
    {
        if (agent.remainingDistance <= 0.5f)
        {
            nextIndex = Random.Range(0, wayPointList.Count);
            MovewayPoint();
        }
    }
    private void MovewayPoint()
    {
        if (agent.isPathStale)
            return;
        agent.destination = wayPointList[nextIndex].position;
        agent.isStopped = false;
        rb.isKinematic = false;
        agent.Move(agent.desiredVelocity * Time.deltaTime * 3);
    }
    private void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;
            agent.destination = pos;
        agent.isStopped = false;
        rb.isKinematic = false;
    }
    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.destination = Vector3.zero;
        rb.isKinematic = true;
        _patrolling = false;
    }
}
