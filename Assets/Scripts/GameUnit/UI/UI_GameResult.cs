using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;

public class UI_GameResult : UI_Base
{

    [SerializeField]
    private Image failOrVictoryImg;
    [SerializeField]
    private Button retryBtn;
    [SerializeField]
    private Button exitBtn;
    [SerializeField]
    private TextMeshProUGUI timer;
    [SerializeField]
    private Image fadeImg;
    [SerializeField]
    private TextMeshProUGUI goldTxt;
    [SerializeField]
    private GameObject bestScoreObj;


    [SerializeField] private Sprite[] resultSprite;
    [SerializeField] private GameObject victoryParticleObj;
    [SerializeField] private GameObject resultObj;

    private RectTransform resultRt;

    private bool exitFade = false;
    private bool retryFade = false;

    private int min = 0;
    private int sec = 0;
    private int curmin = 0;
    private int cursec = 0;

    private int stageGold = 0;          //스테이지 클리어 골드
    private float stageBestTime = .0f;      //현재 스테이지 베스트타임
    private float stageClearTime = .0f;     //스테이지 클리어 타임
    private float speed = 0.0f;

    private Coroutine coroutine = null;

    Define.StageStageType stageType;


    WaitForSeconds timewfs = new WaitForSeconds(0.05f);
    WaitForSeconds wfs = new WaitForSeconds(0.5f);

    // Start is called before the first frame update
    public override void Start()
    {
        if (Managers.Game.GetStageStateType() == Define.StageStageType.Victory)
        {
            //게임의 상태가 승리라면
            failOrVictoryImg.sprite = resultSprite[(int)Managers.Game.GetStageStateType() - 1];
            victoryParticleObj.SetActive(true);
            switch (Managers.Game.CurStageType)
            {
                case Define.SubStage.West:
                    Managers.Game.WestStageClear = true;
                    stageGold = Managers.Game.WestStageGold;
                    stageBestTime = Managers.Game.WestStageBestTime;
                    Managers.Game.WestStageBestTime = RefreshBestClearTime();
                    speed = 500.0f;

                    break;
                case Define.SubStage.East:
                    Managers.Game.EastStageClear = true;
                    stageGold = Managers.Game.EastStageGold;
                    stageBestTime = Managers.Game.EastStageBestTime;
                    Managers.Game.EastStageBestTime = RefreshBestClearTime();
                    speed = 850.0f;

                    break;
                case Define.SubStage.South:
                    Managers.Game.SouthStageClear = true;
                    stageGold = Managers.Game.SouthStageGold;
                    stageBestTime = Managers.Game.SouthStageBestTime;
                    Managers.Game.SouthStageBestTime = RefreshBestClearTime();
                    speed = 1100.0f;

                    break;
            }

            Managers.Game.Gold += stageGold;
            Managers.Game.FileSave();



        }

        else if (Managers.Game.GetStageStateType() == Define.StageStageType.Defeat)
        {
            failOrVictoryImg.sprite = resultSprite[(int)Managers.Game.GetStageStateType() - 1];
            switch (Managers.Game.CurStageType)
            {
                case Define.SubStage.West:
                    stageGold = Managers.Game.WestStageGold / 10;     //실패시 기본골드의 1/10획득
                    break;
                case Define.SubStage.East:
                    stageGold = Managers.Game.EastStageGold / 10;
                    break;
                case Define.SubStage.South:
                    stageGold = Managers.Game.SouthStageGold / 10;
                    break;
            }
            Managers.Game.Gold += stageGold;
            Managers.Game.FileSave();
            speed = 35.0f;

        }

        resultObj.TryGetComponent(out resultRt);


        if (retryBtn != null)
            retryBtn.onClick.AddListener(()=>
            {
                retryFade = true;
            });

        if (exitBtn != null)
            exitBtn.onClick.AddListener(()=>
            {
                
                exitFade = true;

            });

        coroutine = StartCoroutine(ResultGoldCo(speed));



    }

    // Update is called once per frame
    void Update()
    {

        RetryFadeIn();
        ExitFadeIn();


    }

    private void OnEnable()
    {
        Managers.UI.GetSceneUI<UI_GamePlay>().gameObject.SetActive(false);
        TimerSetting();
        ResultSound();
    }

    public void SetStageType(Define.StageStageType type)
    {
        stageType = type;
    }

    private void ResultSound()
    {
        if (Managers.Game.GetStageStateType() == Define.StageStageType.Victory)
            Managers.Sound.Play("Sounds/Effect/VictoryBig");
        else
            Managers.Sound.Play("Sounds/Effect/GameOver");

    }

    private float RefreshBestClearTime()
    {
        if (stageClearTime < stageBestTime || stageBestTime <= 0.0f) //아직 베스트타임이 정해지지 않았거나 만약 승리시 현재 클리어타임이 저장되어있는 베스트 클리어타임보다 빠르다면
        {
            bestScoreObj.SetActive(true);
            return stageClearTime;
        }


        return stageBestTime;       //베스트타임이 더 빠르다면 베스트 타임 그대로 리턴

    }


    private void TimerSetting()
    {
        stageClearTime = Managers.Game.GetInGameTimer();
        min = (int)stageClearTime / 60;
        sec = (int)stageClearTime % 60;
        StartCoroutine(Timer());
    }


    private void TimeSet(int min, int sec)
    {
        if (min < 10 && sec < 10)
            timer.text =  "0" + min.ToString() + " : " + "0" + sec.ToString();

        else if (min >= 10 && sec < 10)       //초만 10초 미만일때
            timer.text = min.ToString() + " : " + "0" + sec.ToString();

        else if (min < 10 && sec >= 10)       //분만 10초 미만일때
            timer.text = "0" + min.ToString() + " : " + sec.ToString();

        else if ( min < 10 && sec < 10)       //초 분이 10 미만일때
            timer.text = "0" + min.ToString() + " : " + "0" + sec.ToString();

        else if (min >= 10 && sec < 10)       //초가 10 미만일때
            timer.text =  min.ToString() + " : " + "0" + sec.ToString();

        else if (min < 10 && sec >= 10)       //분가 10 미만일때
            timer.text = "0" + min.ToString() + " : " + sec.ToString();

        else   //전부다 10 이상일때
            timer.text = min.ToString() + " : " + sec.ToString();
    }

    IEnumerator Timer()
    {

        yield return wfs;

        bool timer = true;
        while(timer)
        {

            if (curmin < min)
                curmin++;

            if (cursec < sec)
                cursec++;

            TimeSet(curmin, cursec);
            

            int curTimer = (curmin * 60) + cursec;

            //if (curTimer == GameManager.instance.timerSec)
            //    timer = false;


            yield return timewfs;
        }
    }

    IEnumerator ResultGoldCo(float speed)
    {
        float count = 0;
        while(true)
        {
            Debug.Log(count);
            count += Time.deltaTime * speed;
            if (((int)count > stageGold))
            {
                goldTxt.text = stageGold.ToString();

                yield break;
            }

            goldTxt.text = ((int)count).ToString();




            yield return null;
        }
    }


    void RetryFadeIn()
    {
        if(retryFade)
        {
            Managers.UI.OnOffSceneUI<UI_GamePlay>(false);

            if (fadeImg != null)
            {
                if (!fadeImg.gameObject.activeSelf)
                {
                    fadeImg.gameObject.SetActive(true);

                }

                if (fadeImg.gameObject.activeSelf && fadeImg.color.a <= 1)
                {
                    Color col = fadeImg.color;
                    if (col.a < 255)
                        col.a += (Time.deltaTime * 1.0f);

                    fadeImg.color = col;


                    if (fadeImg.color.a >= 0.99f)
                    {
                        Managers.UI.ClosePopUp(this);
                        //GameManager.instance.SceneEndOn();
                        Managers.Scene.LoadScene(Define.Scene.BattleStage_Field);


                    }
                }
            }
        }


    }

    void ExitFadeIn()
    {
        if(exitFade)
        {
            Managers.UI.OnOffSceneUI<UI_GamePlay>(false);

            if (fadeImg != null)
            {
                if (!fadeImg.gameObject.activeSelf)
                {
                    fadeImg.gameObject.SetActive(true);

                }

                if (fadeImg.gameObject.activeSelf && fadeImg.color.a <= 1)
                {
                    Color col = fadeImg.color;
                    if (col.a < 255)
                        col.a += (Time.deltaTime * 1.0f);

                    fadeImg.color = col;


                    if (fadeImg.color.a >= 0.99f)
                    {

                        Managers.UI.ClosePopUp(this);
                        //GameManager.instance.SceneEndOn();
                        Managers.Scene.LoadScene(Define.Scene.Lobby);


                    }
                }
            }
        }

    }


}
