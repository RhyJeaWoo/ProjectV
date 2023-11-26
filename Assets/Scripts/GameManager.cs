using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  
    public GameObject gameCam; // 게임에 사용되는 카메라
    public GameObject menuSet; // 메뉴  UI
    public Player player; //플레이어
    public Enemy enemy; //적
    public Boss boss; //보스
    public Transform target; //플레이어 위치

    public float playTime; //플레이 타임을 위해 사용되는 변수지만, 현재는 사용하지않음.
    public bool isBattle; //전투중인지 판별여부
  

   



    public GameObject gamePanel; //게임 전체를 관리하는 패널과
    public GameObject overPanel; //게임끝에서 나타내는 게임오버 패널임.
    public GameObject winPanel;

    public Text playTimeTxt;
    public Text playerHealthTxt;
    public Text playerArrowTxt;
    public Text playerCoinTxt;

    public Image weapon1Img; // ui 장비 이미지1
    public Image weapon2Img; // ui 장비 이미지2

    public Image weaponSImg; // ui 스크롤 이미지

  
    public RectTransform bossHealthGroup; //현재 사용되지않음.
    public RectTransform bossHealthBar; //현재 사용되지않음.



    void Update()
    {
       //esc를 누르면 게임 메뉴창이 뜸.
        if (Input.GetButtonDown("Escape"))
        {
            if (menuSet.activeSelf) 
            {
                menuSet.SetActive(false);
            }
            else
            {
                menuSet.SetActive(true);
            }
        }
        if (isBattle)
        {
            playTime += Time.deltaTime;
        }
     
    }

    public IEnumerator GameOver() // 게임오버시 2초뒤 나타남 코루틴으로 구성
    {
        gamePanel.SetActive(false);
        yield  return new WaitForSeconds(2f);
        overPanel.SetActive(true);

    
    }

    public IEnumerator GameWin() // 보스를 잡았을때 2초뒤 나타남
    {
        gamePanel.SetActive(false);
        yield return new WaitForSeconds(2f);
        winPanel.SetActive(true);

    }


    // Start is called before the first frame update
    void LateUpdate()
    {
        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour *3600) / 60);
        int sec = (int)(playTime % 60);

        playTimeTxt.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", sec);


        playerHealthTxt.text = player.health  + " / " + player.maxHealth; //hp를 ui에서 나타냄
        playerCoinTxt.text = string.Format("{0:n0}", player.coin); //코인을 ui에서 나타냄
        if(player.equipWeapon == null)//화살 갯수를 나타냄 /검 장비시 갯수를 나타내지않음.
        {
            playerArrowTxt.text = "-/" + player.arrow;
            
        }
        else if(player.equipWeapon.type == Weapon.Type.Melee)
        {
            playerArrowTxt.text = "-/" + player.arrow;
        }
        else
        {
            playerArrowTxt.text = player.equipWeapon.curArrow + "/" + player.arrow;
        }

        weapon1Img.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        weapon2Img.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weaponSImg.color = new Color(1, 1, 1, player.hasScrolls > 0 ? 1 : 0);


        /*if (Vector3.Distance(target.position, transform.position) <= 10f && enemy.enumType == Enemy.Type.D)
        {
            bossHealthGroup.anchoredPosition = Vector3.up * 150;
            if (Vector3.Distance(target.position, transform.position) <= 10f && enemy.enumType == Enemy.Type.D && enemy.curHealth > 0)
            {
                bossHealthGroup.anchoredPosition = Vector3.down * 30;
                bossHealthBar.localScale = new Vector3((float)boss.curHealth / boss.maxHealth, 1, 1);
            }
            else
            {
                bossHealthGroup.anchoredPosition = Vector3.up * 150;
            }
        }*/

    }

    public void GameExit()
    {
        MainScene.Instance.LoadScene("MainScene"); //게임 나가기 버튼 함수로 사용됨. 메인으로 돌아감.
    }

}
