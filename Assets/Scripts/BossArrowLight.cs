using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//보스 패턴 라이트 애로우 관련 스크립트 , Arrow 스크립트를 상속받음
public class BossArrowLight : Arrow
{
    //리지드 바디
    Rigidbody rigid;

    
    float angulaPower = 2;
    float scaleValue = 0.1f;

    //발사여부
    bool isShoot;


    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        //코루틴함수
        StartCoroutine(GainPowerTime());
        //코루틴함수
        StartCoroutine(GainPower());
    }

    //발사하기 위해 작성된 코루틴 함수
    IEnumerator GainPowerTime()
    {
        yield return new WaitForSeconds(2.2f);
        isShoot = true;
    }
    //발사하기전 기를 모으는 코루틴 함수
    IEnumerator GainPower()
    {
        while (!isShoot)
        {
            angulaPower += 0.05f;//플레이어 방향으로 0.05씩 속도가 증가함.
            scaleValue += 0.005f;//스케일이 0.005씩 증가함
            transform.localScale = Vector3.one * scaleValue;
            //플레이어 방향으로 발사
            rigid.AddTorque(transform.right * angulaPower, ForceMode.Acceleration);
            yield return null;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
