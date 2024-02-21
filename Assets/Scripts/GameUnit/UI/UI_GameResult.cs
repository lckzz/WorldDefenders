using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;

public class UI_GameResult : UI_Base,IOpenPanel
{

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

    [SerializeField] private GameObject victoryObj;
    [SerializeField] private GameObject defeatObj;

    [SerializeField] private GameObject victoryParticleObj;
    [SerializeField] private GameObject resultObj;



    [SerializeField] private RectTransform resultRt;

    private bool exitFade = false;
    private bool retryFade = false;

    private int min = 0;
    private int sec = 0;
    private int curmin = 0;
    private int cursec = 0;

    private int stageGold = 0;          //�������� Ŭ���� ���
    private float stageBestTime = .0f;      //���� �������� ����ƮŸ��
    private float stageClearTime = .0f;     //�������� Ŭ���� Ÿ��
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
            //������ ���°� �¸����
            victoryObj.SetActive(true);
            defeatObj.SetActive(false);

            victoryParticleObj.SetActive(true);

            Managers.Game.OneChapterStageInfoList[(int)Managers.Game.CurStageType].clear = true;
            stageGold = Managers.Game.OneChapterStageInfoList[(int)Managers.Game.CurStageType].StageData.gold;
            stageBestTime = Managers.Game.OneChapterStageInfoList[(int)Managers.Game.CurStageType].bestTime;
            Managers.Game.OneChapterStageInfoList[(int)Managers.Game.CurStageType].bestTime = RefreshBestClearTime();
            speed = 1100.0f;

            Managers.Game.Gold += stageGold;
            Managers.Game.FileSave();



        }

        else if (Managers.Game.GetStageStateType() == Define.StageStageType.Defeat)
        {
            victoryObj.SetActive(false);
            defeatObj.SetActive(true);

            stageGold = Managers.Game.OneChapterStageInfoList[(int)Managers.Game.CurStageType].StageData.gold / 10;   
            Managers.Game.Gold += stageGold;
            Managers.Game.FileSave();
            speed = 1100.0f;

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

        OpenRectTransformScaleSet();
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
        Managers.Sound.Clear();

        if (Managers.Game.GetStageStateType() == Define.StageStageType.Victory)
            Managers.Sound.Play("Sounds/Effect/VictoryBig");
        else
            Managers.Sound.Play("Sounds/Effect/GameOver");

    }

    private float RefreshBestClearTime()
    {
        if (stageClearTime < stageBestTime || stageBestTime <= 0.0f) //���� ����ƮŸ���� �������� �ʾҰų� ���� �¸��� ���� Ŭ����Ÿ���� ����Ǿ��ִ� ����Ʈ Ŭ����Ÿ�Ӻ��� �����ٸ�
        {
            bestScoreObj.SetActive(true);
            return stageClearTime;
        }


        return stageBestTime;       //����ƮŸ���� �� �����ٸ� ����Ʈ Ÿ�� �״�� ����

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

        else if (min >= 10 && sec < 10)       //�ʸ� 10�� �̸��϶�
            timer.text = min.ToString() + " : " + "0" + sec.ToString();

        else if (min < 10 && sec >= 10)       //�и� 10�� �̸��϶�
            timer.text = "0" + min.ToString() + " : " + sec.ToString();

        else if ( min < 10 && sec < 10)       //�� ���� 10 �̸��϶�
            timer.text = "0" + min.ToString() + " : " + "0" + sec.ToString();

        else if (min >= 10 && sec < 10)       //�ʰ� 10 �̸��϶�
            timer.text =  min.ToString() + " : " + "0" + sec.ToString();

        else if (min < 10 && sec >= 10)       //�а� 10 �̸��϶�
            timer.text = "0" + min.ToString() + " : " + sec.ToString();

        else   //���δ� 10 �̻��϶�
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
                        Managers.Pool.Clear();
                        Managers.Scene.LoadScene(Define.Scene.Lobby);


                    }
                }
            }
        }

    }

    public void OpenRectTransformScaleSet()
    {
        if (resultRt == null)
            resultObj.TryGetComponent(out resultRt);

        resultRt.localScale = new Vector3(0, 1, 1);

        if (Managers.Game.GetStageStateType() == Define.StageStageType.Victory)
        {
            resultRt.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.2f).SetEase(Ease.OutQuad);
        }
        else
        {
            resultRt.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.7f).SetEase(Ease.OutBounce);

        }
    }
}
