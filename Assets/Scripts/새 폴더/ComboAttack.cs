using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttack : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    bool comboPosiible;
    int comboStep;

    public void Attack()
    {
        if (comboStep == 0)
        {
            animator.Play("combo1");
            comboStep = 1;
            return;
        }
        if (comboStep != 0)
        {
            if (comboPosiible)
            {
                comboPosiible = false;
                comboStep += 1;
            }
        }
    }

    public void ComboPossible()
    {
        comboPosiible = true;
    }

    public void Combo()
    {
        if (comboStep == 2)
        {
            animator.Play("Hikick");
        }
        if (comboStep == 3)
        {
            animator.Play("RISINGP");
        }
    }

    public void ComboReset()
    {
        comboPosiible = false;
        comboStep = 0;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }
}
