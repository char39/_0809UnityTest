using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class EnemyAI : MonoBehaviour
{
    public enum State   // 열거형 상수
    { PTROL = 0, TRACE = 1, ATTACK = 2, DIE = 3 }
    public State state = State.PTROL;

    [SerializeField]private Transform playerTr;
    [SerializeField]private Transform enemyTr;
    private Animator ani;

    private float attackDist = 7.0f;
    private float traceDist = 14.0f;
    public bool isDie = false;
    private WaitForSeconds waitTime;
    [SerializeField]private EnemyMoveAgent enemyMoveAgent;
    [SerializeField]private EnemyFire enemyFire;

    void Awake()
    {
        enemyMoveAgent = GetComponent<EnemyMoveAgent>();
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTr = player.GetComponent<Transform>();
        enemyTr = GetComponent<Transform>();
        enemyFire = GetComponent<EnemyFire>();
        ani = GetComponent<Animator>();
        waitTime = new WaitForSeconds(0.3f);
    }

    void OnEnable()
    {
        state = State.PTROL;
        ani.SetBool("Live", true);
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }

    void Update()
    {

    }

    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(1.0f);

        while (!isDie)
        {
            if (state == State.DIE)
                yield break;
            float dist = (playerTr.position - enemyTr.position).magnitude;
            if (dist <= attackDist)
            {
                state = State.ATTACK;
            }
            else if (dist <= traceDist)
                state = State.TRACE;
            else
                state = State.PTROL;
            yield return waitTime;
        }
    }
    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return waitTime;
            switch (state)
            {
                case State.PTROL:
                    enemyFire.isFire = false;
                    enemyMoveAgent.patrolling = true;
                    ani.SetFloat("Speed", 0.5f);
                    break;
                case State.ATTACK:
                    if (!enemyFire.isFire)
                        enemyFire.isFire = true;
                    enemyMoveAgent.Stop();
                    ani.SetFloat("Speed", 0.0f);
                    break;
                case State.TRACE:
                    enemyFire.isFire = false;
                    enemyMoveAgent.traceTarget = playerTr.position;
                    ani.SetFloat("Speed", 1.0f);
                    break;
                case State.DIE:
                    ani.SetBool("Live", false);
                    ani.SetTrigger("Death");
                    enemyFire.isFire = false;
                    enemyMoveAgent.Stop();
                    GetComponent<CapsuleCollider>().enabled = false;
                    gameObject.tag = "Untagged";
                    StartCoroutine(ObjectPoolPush());
                    isDie = true;
                    break;
            }
        }
    }
    IEnumerator ObjectPoolPush()
    {
        yield return new WaitForSeconds(3.0f);
        isDie = false;
        GetComponent<CapsuleCollider>().enabled = true;
        gameObject.tag = "Enemy";
        gameObject.SetActive(false);
    }
}
