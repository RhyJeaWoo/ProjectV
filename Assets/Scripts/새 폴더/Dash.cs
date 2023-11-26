using CreationCharaters.Abilities;
using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CreationCharaters.Abilities
{
    public class Dash : Abilities
    {
        [SerializeField] private float dashForce;
        [SerializeField] private float dashDuration;

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }


        private void Update()
        {

            
            if(Input.GetKeyDown(KeyCode.K))
            {
                StartCoroutine(Cast());
            }
        }

        public override IEnumerator Cast()
        {
            rb.AddForce(Camera.main.transform.forward * dashForce, ForceMode.VelocityChange);
         
            yield return new WaitForSeconds(dashDuration);

            rb.velocity = Vector3.zero;
        }
    }

}
