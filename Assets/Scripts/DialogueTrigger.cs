using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public Dialogue info;

    public void Start()
    {
        //다이얼 로그 트리거 이벤트 스크립트
        var system = FindObjectOfType<DialogueSystem>();
        system.Begin(info);
    }
}
