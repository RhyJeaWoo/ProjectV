using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Enemy : MonoBehaviour
{
    public enum Type { A,B,C,D };
    public Type enumType;
    public int maxHealth;
    public int curHealth;

    public GameObject gameObject;

    public AudioSource hitSound;
    public AudioSource hitBoiceSound;
    public AudioSource hitAttackSound;
    public AudioSource hitDieSound;

    //보스를 쓰러트릴경우 게임메니저를 통해 메뉴를 불러 올것임.
    public GameManager manager;




    public Transform target;
    public Transform originPos;
    public BoxCollider meleeArea;
    public GameObject enemyArrow;
    public GameObject[] coins;
    public bool isChaise;
    public bool isAttack;
    public bool isDead;
    public bool isScroll;
    //public AudioSource Hitsound;

 

    public NavMeshAgent nav;
    public Animator anim;


    private Color originColor;
    private SkinnedMeshRenderer meshRenderer;

    public Rigidbody rigid;
    public CapsuleCollider capsuleCollider; 



    void Awake()
    {
       
        rigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        

        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        originColor = meshRenderer.material.color;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        if (enumType != Type.D)
        {
            Invoke("ChaiseStart", 2);
        }

    }

    void ChaiseStart()
    {
        isChaise = true;
        anim.SetBool("isWalk", true);
    }

    void Update()

    {
        

        if (Vector3.Distance(target.position, transform.position) <= 15f && nav.enabled && enumType != Type.D && enumType != Type.C)
        {
            anim.SetBool("isStop", false);
            nav.isStopped = false;
            nav.SetDestination(target.position);
            nav.isStopped = !isChaise;
        }
        else if (Vector3.Distance(target.position, transform.position) > 10f && nav.enabled && enumType != Type.D)
        {
            nav.SetDestination(originPos.position);
            if (Vector3.Distance(originPos.position, transform.position) < 2f)
            {
                nav.isStopped = true;
                anim.SetBool("isStop", true);
            }

        }
        else if (Vector3.Distance(target.position, transform.position) < 25f && nav.enabled && enumType != Type.D && enumType != Type.A && enumType != Type.B)
        {
            anim.SetBool("isStop", false);
            nav.isStopped = false;
            nav.SetDestination(target.position);
            nav.isStopped = !isChaise;
        }
      
     


    }

    void FreezeVelocity()
    {
        if (isChaise)
        {
            rigid.angularVelocity = Vector3.zero;
            rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
            rigid.velocity = Vector3.zero;
        }

    }

    void Targeting()
    {
        if (!isDead && enumType != Type.D)
        {
            float targetRadius = 0;
            float targetRange = 0;

            switch (enumType)
            {
                case Type.A:
                    targetRadius = 0.5f;
                    targetRange = 2.5f;
                    break;
                case Type.B:
                    targetRadius = 1f;
                    targetRange = 2.5f;
                    break;
                case Type.C:
                    targetRadius = 3f;
                    targetRange = 25f;
                    break;

            }



            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position,
                targetRadius,
                transform.forward,
                targetRange,
                LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        isChaise = false;
        isAttack = true;
        anim.SetBool("isAttack", true);
        hitAttackSound.Play();

        switch (enumType)
        {
            case Type.A:
                yield return new WaitForSeconds(0.1f);
                meleeArea.enabled = true;
               
                yield return new WaitForSeconds(0.5f);

                meleeArea.enabled = false;

                yield return new WaitForSeconds(3f);

                break;

            case Type.B:
                yield return new WaitForSeconds(0.1f);
                rigid.AddForce(transform.forward * 20, ForceMode.Impulse);
              
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                rigid.velocity = Vector3.zero;
                meleeArea.enabled = false;


                yield return new WaitForSeconds(2f);
                break;

            case Type.C:
                yield return new WaitForSeconds(1.5f);
                GameObject instantArrow = Instantiate(enemyArrow, transform.position, transform.rotation);
                Rigidbody rigidArrow = instantArrow.GetComponent<Rigidbody>();
                rigidArrow.velocity = transform.forward * 20;

                yield return new WaitForSeconds(2f);
                break;
        }


        /* yield return new WaitForSeconds(0.2f);
         meleeArea.enabled = true;

         yield return new WaitForSeconds(1f);

         meleeArea.enabled = false;

         yield return new WaitForSeconds(1f);

         */

        isChaise = true;
        isAttack = false;
        anim.SetBool("isAttack", false);

    }

    void FixedUpdate()
    {
        Targeting();
        FreezeVelocity();

   
    }




    

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            if (!isDead && enumType != Type.D)
            {

                hitSound.Play();
                hitBoiceSound.Play();
                anim.SetTrigger("doHit");

            }
          
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;

            StartCoroutine(OnDamage(reactVec));

            // Debug.Log("Melee: " + curHealth);
        }
        else if (other.tag == "Arrow")
        {
            Arrow arrow = other.GetComponent<Arrow>();
            if (!isDead && enumType != Type.D)
            {
                anim.SetTrigger("doHit");
                hitBoiceSound.Play();
            }
            curHealth -= arrow.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            if (!isScroll)
            {
                Destroy(other.gameObject);
            }
            

            // Debug.Log("Range: " + curHealth);
            StartCoroutine(OnDamage(reactVec));
        }
    }

    IEnumerator OnDamage(Vector3 reactVec)
    {
        if (!isDead && enumType != Type.D)
        { meshRenderer.material.color = Color.red; }
       
        yield return new WaitForSeconds(0.1f);
        if (!isDead && enumType != Type.D)
        {
            meshRenderer.material.color = originColor;
        }

        if (curHealth <= 0)
        {
            if (!isDead && enumType != Type.D)
            {
                meshRenderer.material.color = Color.gray;
            }
            gameObject.layer = 13;
            isDead = true;
            isChaise = false;
            nav.enabled = false;

            gameObject.GetComponent<CapsuleCollider>().enabled = false;

            hitDieSound.Play();
            capsuleCollider.enabled = false;
            anim.SetTrigger("doDie");
            if(enumType == Type.D)
            {
                manager.StartCoroutine(manager.GameWin());
            }

            Player player = target.GetComponent<Player>();
            int ranCoin = Random.Range(0, 3);

            Instantiate(coins[ranCoin], transform.position, Quaternion.identity);

            if (!isDead && enumType != Type.D)
            {


                reactVec = reactVec.normalized;
                reactVec += Vector3.up;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            }



            if (enumType != Type.D)
            {
                Destroy(gameObject, 4);
            }
        }
    }
}
