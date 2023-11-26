using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class PlayerFSM : MonoBehaviour
{
    public enum State
    {
        Idle,
        Move,
        Attack,
        AttckWait,
        Dead
    }
    //idle 상태를 기본 상태로 지정

    public State currentState = State.Idle;


    //마우스 클릭 지점, 플레리어가 이동할 목적지의 좌표를 저장할 예정
    Vector3 curTargetPos;

    GameObject curEnemy;

    public float rotAnglePerSecond = 360f;//1초에 플레이어의 방향을 360도 회전

    public float moveSpeed = 2f; // 초당 2미터의 속도로 이동

    float attackDelay = 2f;//공격을 한후 다시 할때까지의 지연시간

    float attackTimer = 0f;//공격 하고 난 뒤에 경과되는 시간을 계산하는 변수

    float attackDistance = 1.5f;//공격 사거리

    float chaseDistance = 2.5f;//전투 중 적이 도망가면 다시 추적을 시작하기 위한 거리



    PlayerAni myAni;

    // Start is called before the first frame update
    void Start()
    {
        myAni = GetComponent<PlayerAni>();
        //myAni.chageAni(PlayerAni.ANI_WALK)

        ChangeState(State.Idle, PlayerAni.ANI_IDLE);
        
    }

    //적을 공격하기 위한 함수
    public void AttakEnemy(GameObject enemy)
    {
        if(curEnemy != null && curEnemy == enemy)
        {
            return;
        }

        curEnemy = enemy;
        curTargetPos = curEnemy.transform.position;


        ChangeState(State.Move, PlayerAni.ANI_WALK);
    }

    void ChangeState(State newState, int aniNumber)
    {
        if (currentState == newState)
        {
            return;
        }

        myAni.ChangeAni(aniNumber);
        currentState = newState;
    }

    //캐릭터의 상태가 바뀌면 어떤 일이 일어날지를 미리 정의
    void UpdateState()
    {
        switch (currentState)
        {
            case State.Idle:
                IdleState();
               
                break;
            case State.Move:

                MoveState();

          
                break;
            case State.Attack:

                AtaackState();

                break;
            case State.AttckWait:

                AttackWaitState();

                break;
            case State.Dead:

                DeadState();

                break;
            default:
                break;
        }
    }

    void IdleState()
    {

    }

    void MoveState()
    {
        TurnToDestination();
        MoveToDestination();
    }

    void AtaackState()
    {
        attackTimer = 0f;

        //transform.LookAt(목표지점 위치)목표지점을 향해 오브젝트를 회전 시키는 함수
        transform.LookAt(curTargetPos);
        ChangeState(State.AttckWait, PlayerAni.ANI_ATKDLE);
            
    
    }

    void AttackWaitState()
    { 
       if(attackTimer>attackDelay)
        {
            ChangeState(State.Attack, PlayerAni.ANI_ATTACK);
        }

        attackTimer += Time.deltaTime;
    }


    void DeadState()
    { }


    //MoveTo(캐릭터가 이동할 목표 지점의 좌표)
    public void MoveTo(Vector3 tPos)
    {
        curEnemy = null;
        curTargetPos = tPos;
        ChangeState(State.Move, PlayerAni.ANI_WALK);
    }




    void TurnToDestination()
    {
        //Quaternion lookRotation(회전할 목표 방향): 목표 방향은 목적지 위치에서 자신의 위치를 뺌
        Quaternion lookRotation = Quaternion.LookRotation(curTargetPos - transform.position);

        //Quaternion.RotateTowards(현재의 rotation값, 최종목표 rotation 값 , 최대 회전각)
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotAnglePerSecond);

    }

    void MoveToDestination()
    {
        //Vector3.MoveTowards(시작지점, 목표지점, 최대이동거리)
        transform.position = Vector3.MoveTowards(transform.position, curTargetPos, moveSpeed * Time.deltaTime);


        if (curEnemy == null)
        {
            // 플레이어의 위치와 목표지점의 위치가 같으면, 상태를 idle상태로 바꾸라는 명령
            if (transform.position == curTargetPos)
            {
                ChangeState(State.Idle, PlayerAni.ANI_IDLE);
            }
        }
        else if(Vector3.Distance(transform.position,curTargetPos) < attackDistance)
        {
            ChangeState(State.Attack, PlayerAni.ANI_ATTACK);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }
}
