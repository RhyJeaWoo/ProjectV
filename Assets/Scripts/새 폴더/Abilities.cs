using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CreationCharaters.Abilities
{


    public abstract class Abilities : MonoBehaviour
    {
        // Start is called before the first frame update
        public abstract IEnumerator Cast();

    }

}