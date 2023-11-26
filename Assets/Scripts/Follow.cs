using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    Camera camera;

    // Update is called once per frame



    void Update()
    {
        

        transform.position = target.position + offset;

/*
        if (Input.GetKey(KeyCode.E))
        {
            transform.RotateAround(target.transform.position, Vector3.up, 120 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.RotateAround(target.transform.position, Vector3.down, 120 * Time.deltaTime);
        }*/

        /*
        if (Input.GetKey(KeyCode.E))
        {
            target.transform.rotation *= Quaternion.Euler(new Vector3(0, 1f, 0));
            //this.transform.Rotate(0.0f, 90.0f * Time.deltaTime, 0.0f);
          //  transform.RotateAround(target.transform.position, Vector3.up, 90 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            target.transform.rotation *= Quaternion.Euler(new Vector3(0, -1f, 0));
            //transform.RotateAround(target.transform.position, Vector3.down, 90 * Time.deltaTime);
        }*/
    }
}
