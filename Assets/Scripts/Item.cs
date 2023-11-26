using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type{ Arrow, Gold, Scroll, Health, Weapon }; //아이템 타입
    public Type type;
    public int value;

    Rigidbody rigid;
    SphereCollider sphereCollider;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    void Update()
    {
        transform.Rotate(Vector3.right * 20 * Time.deltaTime); // 아이템들이  회전함.
    }


    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            rigid.isKinematic = true;
            sphereCollider.enabled = false;
        }
    }

}
