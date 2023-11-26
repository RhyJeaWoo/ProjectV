using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public Transform Player;

    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = Player.position - transform.position;
    }


    //카메라가 플레이어의 움직임에 한템포 늦게 움직임을 준다.

    void LateUpdate()
    {
        //플레이어의 위치와 카메라의 위치를 최초 저장한 위치 차이 만큼 자동으로 유지시켜주게 됨
        transform.position = Player.position - offset;
   
    }
    // Update is called once per frame
   // void Update()
  //  {
        
  //  }
}
