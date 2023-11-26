using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEditor;

public class MainScene : MonoBehaviour
{

    //싱글톤 패턴으로 작성해서 하나뿐인 로딩 UI를 어디서든지 쓸수 있게 생성
    protected static MainScene instance;

    public static MainScene Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<MainScene>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = Create();
                    //싱글톤 패턴에서 객체를 생성할때 캔버스에 만들어둔 진행바나 배경 이미지등 세팅이 있어야 되므로, 
                    //새 게임 오브젝트를 생성해서 붙이지않고. 리소스 폴더에 넣어둔 UI프레펩을 인스턴스화
                }
            }
            return instance;
        }



        private set

        {

            instance = value;

        }

    }


    //로딩 UI에서 사용될 캔버스 그룹 변수와 이미지 변수와 불러올 씬 이름을 저장할 변수
    [SerializeField]
    private CanvasGroup sceneLoaderCanvasGroup;
    [SerializeField]
    private Image progressBar;

    private string loadSceneName;



    public static MainScene Create()

    {

        var SceneLoaderPrefab = Resources.Load<MainScene>("LoadingUI");

        return Instantiate(SceneLoaderPrefab);

    }


    //여기에서 인스턴스가 자기자신인지 검사해서 일치하지않으면 파괴하고,
    //그렇지 않으면 파괴되지않게 함.
    private void Awake()

    {

        if (Instance != this)

        {

            Destroy(gameObject);

            return;

        }



        DontDestroyOnLoad(gameObject);

    }

    //이 함수를 호출하면 비활성화 되어있던, 로딩씬 컨트롤러 게임오브젝트를 활성화 시키고
    //씬을 로딩하는 과정을 처리함
    public void LoadScene(string sceneName)

    {

        gameObject.SetActive(true);

        //로딩 UI방식은 다음 씬이 불러와지는 순간을 체크해서 로딩 UI를 치워줘야함
        SceneManager.sceneLoaded += LoadSceneEnd;


        loadSceneName = sceneName;

        StartCoroutine(Load(sceneName));//로드씬 프로세스 코루틴 함수를 호출

    }



    private IEnumerator Load(string sceneName)

    {

        progressBar.fillAmount = 0f;
        //게임화면을 가리면서 등장하게 만듬
        //코루틴안에서 스타트코루틴으로 다른 코르틴을 실행 시키면서   
        //yield return 하면 호출한 코르틴이 끝날떄까지 기다리게 할 수 있음.

        yield return StartCoroutine(Fade(true));



        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        op.allowSceneActivation = false;



        float timer = 0.0f;
        //씬 로딩이 끝나지 않은 상태이면 계속하게 반속
        while (!op.isDone)

        {
            //반복문이 한번 반복할때 마다 유니티엔진의 제어권을 넘기게 만듬
            yield return null;

            timer += Time.unscaledDeltaTime;


            //이미지 프로그래스 바를 표시하게 만듬(진행도)
            if (op.progress < 0.9f)

            {

                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);

                if (progressBar.fillAmount >= op.progress)

                {

                    timer = 0f;

                }

            }

            //페이크 로딩(진행도가 90%를 넘으면 작동)
            else

            {

                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);



                if (progressBar.fillAmount == 1.0f)

                {

                    op.allowSceneActivation = true;

                    yield break;

                }

            }

        }

    }



    private void LoadSceneEnd(Scene scene, LoadSceneMode loadSceneMode)

    {
        //scene 과 == loadSceneName이 같다면 페이드 함수를 호출해 FadeOut함.
        if (scene.name == loadSceneName)

        {

            StartCoroutine(Fade(false));

            //로드씬 엔드를 제거함.
            SceneManager.sceneLoaded -= LoadSceneEnd;

        }

    }


    // 씬로딩을 시작하거나 끝낼떄 로딩UI를 fadeIn fadeOut 할수 있게 하는 함수
    private IEnumerator Fade(bool isFadeIn)

    {

        float timer = 0f;

        while (timer <= 1f)

        {

            yield return null;

            timer += Time.unscaledDeltaTime * 3f;

            //true 값이면 0에서 1까지 ,false 값이면 1에서 0까지 캔버스 그룹의 알파에 들어가게함.
            sceneLoaderCanvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);

        }


        //이 로딩UI 게임 오브젝트의 활성을 끔
        if (!isFadeIn)

        {



            gameObject.SetActive(false);

        }

    }

}

