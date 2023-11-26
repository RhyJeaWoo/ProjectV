using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCharacterController : MonoBehaviour
{
    [SerializeField]
    Animator animator;


    [SerializeField]
    private Transform cameraTransform;
    private Movement3D movement3D;
    private PlayerAnimator playerAnimator;

    private void Awake()
    {
       

        movement3D = GetComponent<Movement3D>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
    }


    PlayerAni myAni;

    GameObject curEnemy;

    public void Atk()
    {
        animator.SetTrigger("Atk");
    }
    public void OnMovement(float horizontal, float vertical)
    {
        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);
    }





    // Start is called before the first frame update
    void Start()
    {
        myAni = GetComponent<PlayerAni>();
    }

    public void AttakCalulate()
    {
        if(curEnemy == null)
        {
            return;
        }
        curEnemy.GetComponent<EnemyFSM>().ShowHitEffect();
    }


    // Update is called once per frame
    void Update()
    {

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveVector = new Vector3(x, 0, z);
        bool isMove = moveVector.magnitude > 0;
       // playerAnimator.OnMovement(x,0, z);

        /*   if (isMove)
           {
               animator.transform.forward = moveVector;
           }*/



        transform.Translate(new Vector3(x, 0, z).normalized * Time.deltaTime * 1.5f);

        if (Input.GetKey(KeyCode.K))

        {

            animator.Play("Dash");
           

        }

        if(Input.GetKey(KeyCode.J))

        {
          
            Atk();
        }

        


    }
}
