using System.Collections;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using UnityEngine;


public class Player : MonoBehaviour
{
    //카메라 회전을 위한 좌표값
    public Transform body;
    public Transform cameraArm;

    //무기 박스콜라이더
    public BoxCollider weaponbox;

    //이동속도
    public float speed;

    //무기(프리팹에서 저장)
    public GameObject[] weapons;

    //무기를 먹었는지 판별하는 변수
    public bool[] hasWeapons;

    //테스트용으로 만든 스크롤 , 스킬 사용대신 스크롤 1개를 소모해서 스킬 발동
    public GameObject[] Scrolls;
    //스크롤 갯수
    public int hasScrolls;

    //스크롤 오브젝트
    public GameObject ScrollObj;

    //따라오는 카메라를 public으로 선언 (카메라 암)으로 붙이기 위함
    public Camera follwowCamera;
    //게임메니저 참조를 위해 붙임
    public GameManager manager;

    //오디오 소스들
    public AudioSource jumpSound;
    public AudioSource AttackSound;
    public AudioSource ShotSound;
    public AudioSource DodgeSound;
    public AudioSource WalkSound;
    public AudioSource HitSound;
    public AudioSource ScrollSound;
    public AudioSource DieSound;
    public AudioSource getSound;
    public AudioSource BowSound;
    public AudioSource BowReSound;
    public AudioSource ScrollUseSound;

    //화살 갯수
    public int arrow;
    //코인갯수
    public int coin;
    //hp
    public int health;

    //최대로 소지할 수 있는 화살갯수
    public int maxArrow;
    //최대 소지 코인
    public int maxCoin;
    //최대hp
    public int maxHealth;

    //최대 스크롤수
    public int maxHasScroll;
    

    //이동하기 위한 변수
    float hAxis;
    float vAxis;


    //작동 변수
    bool wDown;
    bool jDown;
    bool iDown;
    bool fDown;
    bool rDown;
    bool kDown;


    bool sDown1;
    bool sDown2;
    bool skillDown;
    bool uDown;


    //상태변수 (이걸로 제어)
    bool isReload;
    bool isJump;
    bool isDodge;
    bool isSwap;
    bool isFireReady = true;
    bool isBorder;
    bool isDamage;
    bool isShop;
    bool isDead;
    bool isAttack;

    //콤보시스템을 위해 제어되는 변수들

    //콤보 가능 여부
    bool comboPossible;
    //콤보 스텝(카운트)
    public int comboStep;
    //콤보에 스매쉬 섞는 여부
    bool inputSmash;

    

    Vector3 moveVec; // 이동 벡터

    Vector3 dodgeVec; //닷지 벳터(구르기를 사용하기 위한 벡터)

    //점프등을 위해 리지드 바디 선언
    Rigidbody rigid;
    //애니메이션을 위해서 선언
    Animator anim;

    //특정 조건에서 장비 입수 및 상점 이용을 위해 선언한 오브젝트
    GameObject nearObject;

    //무기 스크립트에서 사용하기 위해 만든 변수
    public Weapon equipWeapon;

    //장비 인덱스
    int equipWeaponIndex = -1;

    //발사 지연 변수
    float fireDeley;

    
    //무기 콜라이더 box on/off 애니메이션 이벤트를 통해 온/오프
    public void ColliderIn()
    {
        weaponbox.enabled = true;
    }
    // 무기 콜라이더 box on/off
    public void ColliderOut()
    {
        weaponbox.enabled = false;
    }


    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponentInChildren<Rigidbody>();
       

    }

    void Start() 
    {
        //무기 콜라이더는 상시 비활성화
        weaponbox.enabled = false;
    }

    void Update()
    {
        //키 함수
        GetInput();
        //이동 함수
        Move();
        //회전 함수
        Turn();
        //점프 함수
        Jump();
        //닷지 함수
        Dodge();
        //반응 함수
        Interaction();
        //무기 교체 함수
        Swap();
        //공격 함수
        Attack();
        //활 장전 함수
        Reload();
        //스킬 사용 함수
        Scroll();
    }

    //사용하기 위한 키를 세팅하고 세팅한 값을 함수로 만들어서 묶음.
    void GetInput() 
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Run");
        jDown = Input.GetButtonDown("Jump");
        fDown = Input.GetButtonDown("Fire1");
        rDown = Input.GetButtonDown("Reload");
        iDown = Input.GetButtonDown("Interaction");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        kDown = Input.GetButtonDown("Dodge");
        skillDown = Input.GetButtonDown("Skill");
        uDown = Input.GetButtonDown("Fire2");
     
    }


    //이동함수
    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized; // 벡터값을 정규화

        if (isDodge) 
        {
            moveVec = dodgeVec; //닷지 중이라면 벡터값을 대입함.
        }

        if (isSwap || !isFireReady || isReload || isDead) //교체/발사/장전/죽은 경우 움직 일 수 없음.
        {
            moveVec = Vector3.zero;
        }

        if (!isBorder)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;//바라보는 방향 정규화
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized; //카메라 우방향 정규화
            Vector3 moveDir = lookForward * moveVec.z + lookRight * moveVec.x;//카메라 이동과 캐릭터 이동을 일치 시키기위해서 세팅

            
            if (moveVec.magnitude != 0f)
            {
                body.forward = moveDir;
            }
            transform.position += moveDir * speed * (wDown ? 3.0f : 1.0f) * Time.deltaTime;
        }

        WalkSound.Play();

        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", wDown);
    }

    void Turn()
    {
        var dir = 0f;
        if (Input.GetKey(KeyCode.Q) && !isDead)
        {
            dir -= 2f;
        }
        if (Input.GetKey(KeyCode.E) && !isDead)
        {
            dir += 2f;
        }
        Vector2 mouseDelta = new Vector2(dir, 0f);
        // 카메라의 원래 각도를 오일러 각으로 저장
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        // 카메라 암 회전 시키기
        cameraArm.rotation = Quaternion.Euler(camAngle.x, camAngle.y + mouseDelta.x, camAngle.z);

    }


    //스크롤
    void Scroll()
    {
        //갯수가 0이면 반환함.
        if(hasScrolls == 0)
        {
            return;
        }
        if(skillDown && !isReload && !isSwap) //장전중이거나, 교체중일땐 사용할 수 없음.
        {
           
            GameObject instantScroll = Instantiate(ScrollObj, transform.position, transform.rotation);

            ScrollSound.Play();
            ScrollUseSound.Play();

            hasScrolls--;
 
        }
    }

    void Jump() // 점프
    {//점프키가 눌리고, 점프,닷지,교체,상점,죽는 도중에는 사용X
        if (jDown /*&& moveVec == Vector3.zero */&&  !isJump && !isDodge && !isSwap && !isShop && !isDead)
        { 
            rigid.AddForce(Vector3.up * 13, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;

           jumpSound.Play();
            Turn();
        }
    }
    void ComboPossioble()
    {
        comboPossible = true;
    }

    void NextAtk()
    {
        if(!inputSmash)
        {
            if (comboStep == 2)
            {
                anim.Play("Combo2");
                AttackSound.Play();
            }
            if (comboStep == 3)
            {
                anim.Play("Combo3");
                AttackSound.Play();
            }
            
        }
        if(inputSmash)
        {
            if (comboStep == 1)
                anim.Play("Smash1");
            if (comboStep == 2)
                anim.Play("Smash2");
            if (comboStep == 3)
                anim.Play("Smash3");
        }
    }

    void ResetCombo()
    {
        comboPossible = false;
        inputSmash = false;
        comboStep = 0;
    }

    void NormalAtttack()
    {
        if(comboStep == 0)
        {
                anim.Play("Combo1");
                AttackSound.Play();

                comboStep = 1;
                return;
            
        }
        if(comboStep !=0)
        {
            if(comboPossible)
            { 
                comboPossible = false;
                comboStep += 1;
            }
        }
    }

    void SmashAttack()
    {
        if(comboPossible)
        {
            comboPossible = false;
            inputSmash = true;
        }
    }

    void Attack()
    {
        if (!isJump)//점프중엔 사용 불가능
        {

            if (equipWeapon == null) // 장비중이지 않을 경우 반환.
            {
                return;
            }

            fireDeley += Time.deltaTime; //공격에 딜레이가 있음.
            isFireReady = equipWeapon.rate < fireDeley; //딜레이가 공속보다 작아졌을때를 bool로 세팅

            if (fDown && isFireReady && !isDodge && !isSwap && !isShop && !isDead) 
            {
                Debug.Log("키눌림");
                equipWeapon.Use();
                //anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doAttack" : "doShot"); 
                if(equipWeapon.type == Weapon.Type.Melee)
                {
                    NormalAtttack();

                }
                else if(equipWeapon.type == Weapon.Type.Range)
                {
                    anim.SetTrigger("doShot");
                }


                    // 트리거 조건을 두가지로 나뉨 검을 장비한 상태로 공격과, 활을 장비한 상태로 공격, 조건에 맞는 애니메이션이 작동됨
                if(equipWeapon.type == Weapon.Type.Range)
                {   ShotSound.Play();
                    BowSound.Play();
                    fireDeley = 0;
                }
               
            }
            else if (uDown && isFireReady && !isDodge && !isSwap && !isShop && !isDead&& equipWeapon.type != Weapon.Type.Range)
            {
                SmashAttack();
                AttackSound.Play();
            }
            isAttack = true;
        }
    }

    //재장전 함수
    void Reload()
    {
        //장비중이지 않으면 반환
        if(equipWeapon == null)
        {
            return;
        }//무기타입이 일치하지않으면 반환
        if(equipWeapon.type == Weapon.Type.Melee)
        {
            return;
        }//활 갯수가 부족하다면 반환
        if(arrow == 0)
        {
            return;
        }
        if(rDown && !isJump && !isDodge && !isSwap && isFireReady && !isShop && !isDead && !isReload)
        {
            BowReSound.Play();

            anim.SetTrigger("doReload");

            isReload = true; // 장전 상태로 on 이때 공격할 수 없음. 장전해야함.

            Invoke("ReloadOut", 1f);

        }
    }
    //장전이 끝나면
    void ReloadOut()
    {
        int reArrow = arrow < equipWeapon.maxArrow ? arrow : equipWeapon.maxArrow;

        equipWeapon.curArrow = reArrow;// 화살갯수 갱신
        arrow -= reArrow; // 장전이 끝나면 가진 화살 갯수가 줄어듬.

        isReload = false; // 장전 상태 해제
    }

    //닷지 함수 구르기 위해 작성
    void Dodge()
    {
        if (kDown && moveVec != Vector3.zero && !isJump && !isDodge && !isSwap && !wDown && !isShop && !isDead)
        {
            dodgeVec = moveVec; 

            speed *= 4; //순간적으로 4배의 속도로 움직임

            DodgeSound.Play();
            
            anim.SetTrigger("doDodge"); // 움직이면서 구르는 애니메이션 작동
            isDodge = true; //닷지 중임
            
            

            Invoke("DodgeOut", 0.5f);

        }
    }

    void DodgeOut()
    {
        speed *= 0.25f; // 닷지가 끝나면 속도 반환
        isDodge = false; //닷지 끝.
    }


    //무기 교체함수
    void Swap()
    { 
        if(sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0)) //인덱스조검이 일치하지않으면 반환
        {return;}
        if(sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
        {return;}
       

        int weaponIndex = -1;
        if(sDown1) weaponIndex = 0;
        if(sDown2) weaponIndex = 1;

        if((sDown1 || sDown2) && !isJump && !isDodge){
            if (equipWeapon != null)
            {
                equipWeapon.gameObject.SetActive(false);
            }
   

            equipWeaponIndex = weaponIndex;

            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>(); //무기 스크립트 참조
            equipWeapon.gameObject.SetActive(true);

            anim.SetTrigger("doSwap"); 

            isSwap = true;

            Invoke("SwapOut", 0.4f);

        }
    }

    void SwapOut()
    {
        isSwap = false;
    }
    //상호 작용
    void Interaction() {

        if (iDown && nearObject != null && !isJump && !isDodge && !isDead)
        {
            getSound.Play();
            if (nearObject.tag == "Weapon") //상호작용하는 타입이 장비일때
            {
                Item item = nearObject.GetComponent<Item>(); // 아이템 스크립트 참조
                int weaponIndex = item.value; 
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject); // 근처에 있는 장비는 습득되고 동시에 오브젝트는 파괴된다.


            }


            else if (nearObject.tag == "Shop") //상호작용하는 타입이 상점일때
            {
                Shop shop = nearObject.GetComponent<Shop>();
                shop.Enter(this);
                isShop = true;


            }
        }
        
    }

    //회전 버그 방지
    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = new Vector3(0, rigid.velocity.y, 0);

    }

    //벽끼임을 방지하기 위해 만듬
    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }

   void FixedUpdate()
    {
        FreezeRotation();
        StopToWall();
    }


    //바닥이면 점프상태 off
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag =="Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item") //아이템 자동 습득을 위한 충돌 처리
        {
            Item item = other.GetComponent<Item>();
            getSound.Play();
            switch (item.type)
            {
                case Item.Type.Arrow: //화살
                    arrow += item.value;
                    if (arrow > maxArrow)
                    {
                        arrow = maxArrow;
                    }
                    break;
                case Item.Type.Gold: //골드
                    coin += item.value;
                    if (coin > maxCoin)
                    {
                        coin = maxCoin;
                    }
                    break;
                case Item.Type.Scroll: //스크롤
                    hasScrolls += item.value;
                    if (hasScrolls > maxHasScroll)
                    {
                        hasScrolls = maxHasScroll;
                    }
                    break;
                case Item.Type.Health: //hp
                    health += item.value;
                    if (health > maxHealth)
                    {
                        health = maxHealth;
                    }
                    break;
            }

            Destroy(other.gameObject);
        }
        else if (other.tag == "EnemyBullet") //적 화살
        {
            if (!isDamage && !isDodge && !isDead) {
                Arrow enemyArrow = other.GetComponent<Arrow>();
                
                health -= enemyArrow.damage;
                bool isBossAtk = other.name == "BossMeleeArea";

                StartCoroutine(OnDamage(isBossAtk));
             }

            if (other.GetComponent<Rigidbody>() != null)
            {
                Destroy(other.gameObject);
            }

        }

    }

    //데미지를 입는 코루틴
    IEnumerator OnDamage(bool isBossAtk)
    {
        if(isDodge)//닷지중일땐 무적 판정이 있음. 데미지를 1초 정도 무시할 수 있음.
        {
            isDamage = false;


            yield return new WaitForSeconds(1f);

            isDamage = true;

            yield return new WaitForSeconds(1f);
        }
        isDamage = true;

        if (!isDead) //죽은 상태가 아니라면 데미지 받는 모션이 작동
        {
            anim.SetTrigger("doHit");
            HitSound.Play();
        }

        if (isBossAtk)
            rigid.AddForce(transform.forward * -50, ForceMode.Impulse);//보스 공격이면 뒤로 밀려남

     
        yield return new WaitForSeconds(0.5f);

        isDamage = false;

        if (health <= 0 && !isDead)
        {
            OnDie();
        }


        if (isBossAtk)
            rigid.velocity = Vector3.zero;

        yield return new WaitForSeconds(0.5f);



    }

    //죽을떄 작동하는 함수
    void OnDie()
    {
        anim.SetTrigger("doDie");
        DieSound.Play();
        isDead = true;
        manager.StartCoroutine(manager.GameOver());
    }

    //nearObject가 충돌 헀을때 발동시키기 위한 충돌트리거Stay (충돌하지않으면 상호작용 불가능)
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon" || other.tag == "Shop")
        {
            nearObject = other.gameObject;

            Debug.Log(nearObject.name);
        }
     
    }


    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = null;
        else if (other.tag == "Shop")
        {
            Shop shop = nearObject.GetComponent<Shop>();
            shop.Exit();
            isShop = false;
            nearObject = null;
        }
    }
    

     
    
}
