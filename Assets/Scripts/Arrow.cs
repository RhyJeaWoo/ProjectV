using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//화살 스크립트임
public class Arrow : MonoBehaviour
{

    //참조시키기 위해 선언한 변수들임.
    public int damage;
    public bool isMelee; // 무기 타입 여부를 확인하기 위한 변수 (화살)
    public bool isLight;
    public bool isBullet;
    public bool isExp;
   

    void OnCollisionEnter(Collision collision)
    {
        if(!isLight &&collision.gameObject.tag == "Floor") //바닥에 닿으면 파괴
        {
            Destroy(gameObject, 3);
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isMelee && other.gameObject.tag == "Wall" ) //벽에 닿으면 파괴
        {
            
            
                Destroy(gameObject);

        }
    }
}
