using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TPSCharacterController : MonoBehaviour
{
    public float speed;

    [SerializeField]

    private Transform characterBody;

    [SerializeField]

    private Transform cameraArm;



    Animator animator;

   /*private void LookAround(float rotationDir)
    {
        // 마우스 이동 값 검출
        Vector2 mouseDelta = new Vector2(rotationDir, 0f);
        // ... 생략
        Vector3 camAngle = cameraArm.rotation.eulerAngles;



        float x = camAngle.x - mouseDelta.y;



        if (x < 180f)

        {

            x = Mathf.Clamp(x, -1f, 70f);

        }

        else

        {

            x = Mathf.Clamp(x, 335f, 361f);

        }



        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);




    }*/



     private void LookAround(float rot)

     {

         Vector2 mouseDelta = new Vector2(rot, 0f);

         Vector3 camAngle = cameraArm.rotation.eulerAngles;



         float x = camAngle.x - mouseDelta.y;



         if (x < 180f)

         {

             x = Mathf.Clamp(x, -1f, 70f);

         }

         else

         {

             x = Mathf.Clamp(x, 335f, 361f);

         }



         cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);




     }

    private void Move()

    {
        
     /*   Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        bool isMove = moveInput.magnitude != 0;

        animator.SetBool("isMove", isMove);

        if (isMove)

        {

            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;

            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;

            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;



            characterBody.forward = moveDir;

            transform.position += moveDir * Time.deltaTime * 0.1f;
       
        }*/
       
        if (Input.GetKey(KeyCode.E))
        {
            characterBody.transform.rotation *= Quaternion.Euler(new Vector3(0, 1f, 0));
           // cameraArm.transform.RotateAround(characterBody.position, Vector3.up, speed * Time.deltaTime);
        }
       
        if (Input.GetKey(KeyCode.Q))
        {
            characterBody.transform.rotation *= Quaternion.Euler(new Vector3(0, -1f, 0));
           // characterBody.transform.rotation *= Quaternion.Euler(new Vector3(0, -1f, 0));
        }
        
    }












    void Start()

    {

        animator = characterBody.GetComponent<Animator>();

    }


    // Update is called once per frame
    void Update()
    {
        //  LookAround();
       // float rotDir = 0f;
      //  if (Input.GetKey(KeyCode.Q)) { rotDir -= 1f; }
       // if (Input.GetKey(KeyCode.E)) { rotDir += 1f; }
        //LookAround(rotDir);
        Move();
     
     

    }
}
