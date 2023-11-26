using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class EnemyFSM : MonoBehaviour
{
    public enum State
    {
        Idle,//정지
        Chase,//추적
        Attak,//공격
        Dead,//사망
        NoState//아무일도 없는 상태
    }

    public State currentState = State.Idle;

    //10.10 추가 
    private Animator animator;
    private SkinnedMeshRenderer meshRenderer;
    private Color originColor;

    public void TakeDamage(int damage)
    {
        // 체력이 감소되거나 피격 애니메이션이 재생되는 등의 코드를 작성
        Debug.Log(damage + "의 체력이 감소합니다.");
        // 피격 애니메이션 재생
        animator.SetTrigger("onHit");
        // 색상 변경
        StartCoroutine("OnHitColor");
    }

    private IEnumerator OnHitColor()
    {
        // 색을 빨간색으로 변경한 후 0.1초 후에 원래 색상으로 변경
        meshRenderer.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        meshRenderer.material.color = originColor;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        originColor = meshRenderer.material.color;
    }


    EnemyAni myAni;
    Transform player;

    float chaseDistance = 5f;//플레이어를 향해 몬스터가 추적을 시작할 거리
    float attakDistance = 2.5f;//플레이어가 왼쪽으로 들어오게 되면 공격을 시작
    float reChaseDistance = 3f;//플레이어가 도망 갈 경우 일정한 거리가 멀어져야 다시 추적

    float rotAngelePerSecond = 360f;
    float moveSpeed = 1.3f;

    float attackDelay = 2f;
    float attackTimer = 0f;

    public ParticleSystem hitEffect;



    // Start is called before the first frame update
    void Start()
    {
        myAni = GetComponent<EnemyAni>();
        ChangeState(State.Idle, EnemyAni.IDLE);

        player = GameObject.FindGameObjectWithTag("Player").transform;

        hitEffect.Stop();
    }

    public void ShowHitEffect()
    {
        hitEffect.Play();
    }

    void UpdateState()
    {
        switch(currentState)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Chase:
                ChaseState();
                break;
            case State.Attak:
                AttackState();
                break;
            case State.Dead:
                DeadState();
                break;
            case State.NoState:
                NoState();
                break;
        }
    }

    public void ChangeState(State newState, string aniName)
    {
        if(currentState ==  newState)
        {
            return;
        }

        currentState = newState;
        myAni.ChangeAni(aniName);



    }


    void IdleState()
    {
        if(GetDistanceFromPlayer() < chaseDistance)
        {
            ChangeState(State.Chase, EnemyAni.WALK);
        }
    }

    void ChaseState()
    {
        //몬스터가 공격 가능 거리 안으로 들어가면 공격 상태
        if(GetDistanceFromPlayer() < attakDistance)
        {
            ChangeState(State.Attak, EnemyAni.ATTACK);
        }
        else
        {
            TurnToDestination();
            MoveToDestination();

        }
    }

    void AttackState()
    {
        if(GetDistanceFromPlayer() > reChaseDistance)
        {
            attackTimer = 0f;
            ChangeState(State.Chase, EnemyAni.WALK);
        }
        else
        {
            if(attackTimer > attackDelay)
            {
                transform.LookAt(player.position);
                myAni.ChangeAni(EnemyAni.ATTACK);

                attackTimer = 0f;

     
            }

            attackTimer += Time.deltaTime;
        }
    }

    void DeadState()
    {

    }

    void NoState()
    {

    }

    void TurnToDestination()
    {
        Quaternion lookRotation = Quaternion.LookRotation(player.position - transform.position);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotAngelePerSecond);
    }


    void MoveToDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }


    float GetDistanceFromPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        return distance;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }
}
