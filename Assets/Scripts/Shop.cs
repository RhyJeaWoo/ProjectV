using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    //상점 스크립트 가진 골드로 상점에서 거래를 하게 하는 스크립트

    public RectTransform uiGruop; //ui크룹
    public Animator anim;

    public GameObject[] itemObj;//아이템 리스트
    public int[] itemPrice; //아이템 가격
    public Transform[] itemPos;//아이템 위치
    public Text talkText;
    public string[] talkData;


    Player enterPlayer;

    // Start is called before the first frame update
    public void Enter(Player player) 
    {
        enterPlayer = player; 
        uiGruop.anchoredPosition = Vector3.zero;
    }

    // Update is called once per frame
    public void Exit()
    {
        anim.SetTrigger("doHello");
        uiGruop.anchoredPosition = Vector3.down * 1000;
    }

    public void Buy(int index)
    {
        int price = itemPrice[index];
        if (price > enterPlayer.coin) {
            StopCoroutine(Talk());
            StartCoroutine(Talk());
            return;

        }
        enterPlayer.coin -= price;
        Vector3 ranVec = Vector3.right * Random.Range(-3, 3) + Vector3.forward * Random.Range(-3, 3);
        Instantiate(itemObj[index], itemPos[index].position + ranVec, itemPos[index].rotation);
    }

    IEnumerator Talk()
    {
        talkText.text = talkData[1];
        yield return new WaitForSeconds(2f);
        talkText.text = talkData[0];

    }
}
