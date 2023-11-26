using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStageSelectMoveScene : MonoBehaviour
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
        MainScene.Instance.LoadScene("ComunicationScenes");//다이얼로그 장면으로 넘어가게 하는 스크립트
    }

}
