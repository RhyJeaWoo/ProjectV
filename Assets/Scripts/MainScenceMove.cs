using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScenceMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      /*  if (Input.GetKey(KeyCode.Space))
        {
            MainScene.Instance.LoadScene("SampleScene");
        }*/

    }

    public void Click()
    {
        MainScene.Instance.LoadScene("ComunicationScenes"); //씬 이동 함수
    }

}
