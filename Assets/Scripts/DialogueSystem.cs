
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
   // public Dialogue info;
    //public Text txtName;

    //다이얼로그 시스템 스크립트
    public Text txtSentence;
    public GameObject Dialogue;

    public bool notEvent;



    Queue<string> sentences = new Queue<string>();



    public void Begin(Dialogue info)
    {
        sentences.Clear();
        //전부 지움

        //txtName.text = info.name;

        foreach(var sentence in info.sentence) //생성한 다이얼로그를 불러옴.
        {
            sentences.Enqueue(sentence);
        }

        Next();
    }

    //생성한 다이얼로그를 인덱스 순으로 불러오는 함수
    public void Next()
    {
      
       
        if (sentences.Count == 0)
        {
            End();
            return;

        }
        //txtSentence.text = sentences.Dequeue();
        txtSentence.text = string.Empty;
        StopAllCoroutines();
        StartCoroutine(TypeSentece(sentences.Dequeue())); // 다읽으면 제거
    }

    IEnumerator TypeSentece(string sentence)
    {
        foreach(var letter in sentence)
        {
            txtSentence.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void End()
    {
        if (notEvent)
        {
            txtSentence.text = string.Empty;
            Destroy(Dialogue);
            
        }

        else
        {
            txtSentence.text = string.Empty;
            MainScene.Instance.LoadScene("FirstStage"); // 다음씬으로 이동
        }
    }



}
