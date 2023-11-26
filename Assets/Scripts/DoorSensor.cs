using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSensor : MonoBehaviour
{

    public AudioSource FirstBgm;
    public AudioSource SecondBgm;
    public GameObject boss;
    

    //보스를 출현 시키기 위해 작성된 스크립트

    void OnTriggerEnter(Collider other)
    {
        //특정 구간을 충돌하고 그 구간이 플레이어면 보스가 소환됨.
        //새 bgm이 재생됨, 기존 bgm이 중지됨
        if (other.tag == "Player")
        {
            FirstBgm.Stop();
            SecondBgm.Play();
            boss.SetActive(true);

            
        }
    }
}
