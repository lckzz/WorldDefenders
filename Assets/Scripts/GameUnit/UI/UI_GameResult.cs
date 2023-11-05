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

    [SerializeField] private Sprite[] resultsprite;
    bool exitFade = false;
    bool retryFade = false;

    bool timecheck = false;
    int min = 0;
    int sec = 0;
    int curmin = 0;
    int cursec = 0;

    Define.StageStageType stageType;


    WaitForSeconds timewfs = new WaitForSeconds(0.05f);
    WaitForSeconds wfs = new WaitForSeconds(0.5f);

    // Start is called before the first frame update
    public override void Start()
    {
        if (Managers.Game.GetStageStateType() == Define.StageStageType.Victory)
        {
            //게임의 상태가 승리라면
            failOrVictoryImg.sprite = resultsprite[(int)Managers.Game.GetStageStateType() - 1];

            switch (Managers.Game.CurStageType)
            {
                case Define.SubStage.West:
                    Managers.Game.WestStageClear = true;
                    break;
                case Define.SubStage.East:
                    Managers.Game.EastStageClear = true;
                    break;
                case Define.SubStage.South:
                    Managers.Game.SouthStageClear = true;
                    break;
            }

            Managers.Game.FileSave();

        }

        else if (Managers.Game.GetStageStateType() == Define.StageStageType.Defeat)
            failOrVictoryImg.sprite = resultsprite[(int)Managers.Game.GetStageStateType() - 1];


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

    }

    // Update is called once per frame
    void Update()
    {
        if(timecheck)
        {
            timecheck = false;
            StartCoroutine(Timer());
        }

        RetryFadeIn();
        ExitFadeIn();


    }

    private void OnEnable()
    {
        Managers.UI.GetSceneUI<UI_GamePlay>().gameObject.SetActive(false);
        TimerSetting();
    }

    public void SetStageType(Define.StageStageType type)
    {
        stageType = type;
    }


    void TimerSetting()
    {

        min = (int)Managers.Game.GetInGameTimer() / 60;
        sec = (int)Managers.Game.GetInGameTimer() % 60;
        timecheck = true;
    }


    private void timeSet(int min, int sec)
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

            timeSet(curmin, cursec);
            

            int curTimer = (curmin * 60) + cursec;

            //if (curTimer == GameManager.instance.timerSec)
            //    timer = false;


            yield return timewfs;
        }
    }


    Sprite ImageSetting(string path)
    {
        return Resources.Load<Sprite>(path);
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
