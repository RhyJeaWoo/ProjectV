using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAni : MonoBehaviour
{

    //애니메이터 컨트롤러의 전이 관계에서 설명한 번호에 맞춤
    public const int ANI_IDLE = 0;
    public const int ANI_WALK = 1;
    public const int ANI_ATTACK = 2;
    public const int ANI_ATKDLE = 3;
    public const int ANI_DIE = 4;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ChangeAni(int aniNumber)
    {
        anim.SetInteger("aniName", aniNumber);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
