using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//보스 스크립트 Enemy스크립트를 상속받음
public class Boss : Enemy
{

    
    public GameObject arrow; //게임오브젝트 FireBallVariant 패턴1
    public Transform magicArrow1; //패턴1 발사 위치
    public Transform magicArrow2; //패턴2 발사 위치
    public GameObject exp;
    public GameObject emission;

  



    Vector3 lookVec; // 바라보는 방향 벡터
    Vector3 tauntVec; // 플레이어 쪽으로 순간 이동할 벡터
    public  bool isLook; //보고있는 상태 변수


    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        
        nav = GetComponent<NavMeshAgent>(); //네비 메쉬를 이용해 작성
        anim = GetComponent<Animator>();

        nav.isStopped = true; // 네비는 기본적으로 멈춤.

        StartCoroutine(Think());
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) //죽으면 모든 코루틴을 멈춤
        {
            StopAllCoroutines();
            return;
        }


        if (isLook) 
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 2;
            transform.LookAt(target.position + lookVec);
         }
        else
        {
            nav.SetDestination(tauntVec);
        }
    }

    IEnumerator Think() //패턴 코루틴 함수임
    {
        yield return new WaitForSeconds(0.1f);

        int rendAction = Random.Range(0, 5);
        switch (rendAction)
        {
            case 0:
                
            case 1:
                StartCoroutine(MagicShot()); // 2/7 확률로 매직샷 패턴을 사용함 제1패턴
                //특징 플레이어 방향으로 발사함.


                break;

            case 2:
                
            case 3:
                StartCoroutine(MagicArrowShot());// 2/7 확률로 매직애로우샷 패턴을 사용함 제2패턴
                //특징 플레이어를 따라다님. 특정시간까지

                break;
            case 4:
               
            case 5:
                StartCoroutine(Exp()); // 2/7 확률로 캐릭터 자리에 폭발

                break;


            case 6:
                StartCoroutine(Taunt()); // 1/7 확률로 캐릭터를 향해 뛰어듬
                break;
        }

    }

    IEnumerator MagicShot()
    {
        isLook = false;
        anim.SetTrigger("doBigShot");
        Instantiate(enemyArrow, transform.position, transform.rotation);

        yield return new WaitForSeconds(3f);
        isLook = true;
        StartCoroutine(Think()); // 패턴 발생후 반복함.
    }

    IEnumerator Exp()
    {
        isLook = false;
        anim.SetTrigger("doBigShot");
        Instantiate(emission, target.transform.position, transform.transform.rotation);

        yield return new WaitForSeconds(1f);

        Instantiate(exp, target.transform.position, target.transform.rotation);
       
        yield return new WaitForSeconds(3f);
        isLook = true;
        StartCoroutine(Think());
    }

    IEnumerator MagicArrowShot()
    {


        anim.SetTrigger("doShot");

        yield return new WaitForSeconds(0.2f);
        GameObject instantArrowA = Instantiate(arrow, magicArrow1.position, magicArrow1.rotation);
        BossArrow bossArrowA = instantArrowA.GetComponent<BossArrow>();
        bossArrowA.target = target;

        yield return new WaitForSeconds(0.3f);
        GameObject instantArrowB = Instantiate(arrow, magicArrow1.position, magicArrow1.rotation);
        BossArrow bossArrowB = instantArrowB.GetComponent<BossArrow>();
        bossArrowB.target = target;

        //두번 발사하는 패턴, 플레이러를 따라다님.

        yield return new WaitForSeconds(6f);
        //6초뒤 소멸

        Destroy(instantArrowA);
        Destroy(instantArrowB);

        StartCoroutine(Think());
    }

    IEnumerator Taunt()
    {
        tauntVec = target.position + lookVec; //타겟위치에서 자기 위치를 계산
        isLook = false; 
        nav.isStopped = false; //네비 멈춤 해제
        capsuleCollider.enabled = false; // 콜라이더 해제(무적)
        anim.SetTrigger("doTaunt"); //찍는 애니메이션 작동

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = true; //데미지 판정 활성

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;//데미지 판정 비활성

        yield return new WaitForSeconds(1f);
        isLook = true;
        nav.isStopped = true; //네비 멈춤 활성
        capsuleCollider.enabled = true; // 캡슐콜라이더 활성(무적 해제)

        

        StartCoroutine(Think());
    }


}
