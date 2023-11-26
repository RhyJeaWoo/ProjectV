using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public enum Type { Melee, Range };//변수타입 근접 or 원거리
    public Type type;
    public int damage; //데미지
    public float rate; //공속
    public int maxArrow;
    public int curArrow;
    public BoxCollider meleeArea; 
    public TrailRenderer trailEffect;
    public Transform ArrowtPos;
    public GameObject arrow;
  
     
    public void Use()
    {
        if (type == Type.Melee) //무기 타입에 따라 작동되는 함수 현재는 활/화살로 되어있음.
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");

        }
        else if (type == Type.Range && curArrow > 0)
        {
            curArrow--;
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing()
    {
        //1
        yield return new WaitForSeconds(0.5f); //0.1초 대기
        meleeArea.enabled = true;
        trailEffect.enabled = true;
                           //2
        yield return new WaitForSeconds(0.5f);//1프레임 대기
        meleeArea.enabled = false;
                          //3
        yield return new WaitForSeconds(0.5f);//1프레임 대기
        trailEffect.enabled = false;
    }

    //Use() 메인 루틴 -> Swing() 서브루틴 -> Use() 메인 루틴
    //Use() 메인 루틴 -> Swing() 코루틴(Co-Op)
   IEnumerator Shot()
    {
        //화살 발사
        GameObject instantArrow = Instantiate(arrow, ArrowtPos.position, ArrowtPos.rotation);
        Rigidbody ArrowRigid = instantArrow.GetComponent<Rigidbody>();
        ArrowRigid.velocity = ArrowtPos.forward * 50;
        yield return null;
        //

    }
}
