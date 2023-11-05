using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_StageSelectPopUp : UI_Base
{

    [SerializeField] private Image[] onestageSels = null;
    [SerializeField] private TextMeshProUGUI curSelectText;
    [SerializeField] private Button backBtn;
    [SerializeField] private Button startBtn;
    [SerializeField] private Image fadeImg;
    [SerializeField] private GameObject uiObj;
    [SerializeField] private GameObject paperBg;
    [SerializeField] private GameObject stageInfoObj;
    [SerializeField] private GameObject stageUIObj;


    private RectTransform paperRt;
    private Vector2 rtSizeDelta;

    private float openSelectPaper = 1432.0f;

    private bool fadeCheck = false;
    private StageNode stagenode;

    [SerializeField] private GameObject player;

    Color btnImgAlphaOn = new Color32(255, 255, 255, 255);
    Color btnImgAlphaOff = new Color32(255, 255, 255, 105);

    private UI_PlayerController ui_PlayerCtrl;
    private StageInfo stageInfo;
    bool backFadeCheck = false;


    public bool FadeCheck { get { return fadeCheck; }}
    // Start is called before the first frame update
    public override void Start()
    {
        Managers.Game.FileSave();
        paperBg?.TryGetComponent(out paperRt);
        rtSizeDelta = paperRt.sizeDelta;
        rtSizeDelta.x = 0.0f;
        paperRt.sizeDelta = rtSizeDelta;
        if(stageUIObj != null)
        {
            if (stageUIObj.activeSelf)
                stageUIObj.SetActive(false);
        }


        StartCoroutine(StartPaperDeltaSizeDo(openSelectPaper, true));
        if (backBtn != null)
            backBtn.onClick.AddListener(ClosePopUp);
        if (startBtn != null)
            startBtn.onClick.AddListener(StartInGame);

        for (int ii = 0; ii < onestageSels.Length; ii++)
        {
            OnClickStage(onestageSels[ii].gameObject, ii, GetStageInfo, UIEvent.PointerDown);
        }
        player.TryGetComponent(out ui_PlayerCtrl);
        StartBtnInActive();

        //GetStageInfo((int)GlobalData.curStage);
        startFadeOut = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (fadeCheck)   //다음씬을 넘어갈떄
            FadeIn();

        //뒤로가거나 들어올때
        //Util.FadeOut(ref startFadeOut, fadeImg);
        //BackFadeIn(fadeImg, this, backFadeCheck);
    }


    void OnClickStage(GameObject stageObj, int idx, Action<int> action, UIEvent type = UIEvent.PointerDown)
    {
        UI_EventHandler evt = null;
        stageObj.TryGetComponent(out evt);

        

        if (evt != null)
        {
            switch(type)
            {
                case UIEvent.PointerDown:
                    {
                        evt.OnPointerDownIntHandler -= (unUsed) => action(idx);
                        evt.OnPointerDownIntHandler += (unUsed) => action(idx);

                        break;
                    }
            }
        }


    }




    void GetStageInfo(int ii)
    {
        //모든 1챕터를 돌아서 전부 클릭된곳을 꺼준다.
        for(int i = 0; i < onestageSels.Length; i++)
        {
            onestageSels[i].TryGetComponent(out stagenode);
            stagenode.ClickStageDoOff();

        }

        //해당 스테이지를 누르면 지금 스테이지가 어떤 스테이지인지 확인하고 해당스테이지 몬스터의 정보를 받아온다
        onestageSels[ii].TryGetComponent(out stagenode);
        Managers.Game.CurStageType = stagenode.Stage;
        

        if(stagenode.StState == Define.StageState.Open)
        {
            GlobalData.SetMonsterList(stagenode.StageMonsterList);  //정적변수에 몬스터의 정보들을 받아둔다.
            if (ui_PlayerCtrl.IsGo == false)
                SelectStageTextRefresh(Define.MainStage.One, Managers.Game.CurStageType);
            ui_PlayerCtrl.SetTarget(Managers.Game.CurStageType, true);
            StartBtnActive();
        }

        stagenode.ClickStageDoOn();
        stageInfoObj.SetActive(true);
        if (stageInfo == null)
            stageInfoObj.TryGetComponent(out stageInfo);

        stageInfo.SetStageInfoPosition(stagenode.GetNodePosition());
        stageInfo.StageInfoInit();

    }




    void SelectStageTextRefresh(Define.MainStage mainstage, Define.SubStage subStage)
    {
        if(mainstage == Define.MainStage.One)
        {
            switch (subStage)
            {
                case Define.SubStage.West:
                    curSelectText.text = "Stage 1 - 1";
                    break;
                case Define.SubStage.East:
                    curSelectText.text = "Stage 1 - 2";
                    break;
                case Define.SubStage.South:
                    curSelectText.text = "Stage 1 - 3";
                    break;
                case Define.SubStage.Boss:
                    curSelectText.text = "Stage 1 - 4";
                    break;
                 default:
                    curSelectText.text = "Select Stage!";
                    break;
            }

        }
    }


    void ClosePopUp()
    {
        Managers.Sound.Play("Effect/UI_Click");

        Managers.UI.ClosePopUp(this);
        if (Managers.Scene.CurrentScene is LobbyScene lobby)
        {
            lobby.LobbyUIOnOff(true);
            lobby.LobbyTouchUnitInit();
        }
    }


    void StartBtnActive()
    {

        //버튼활성화
        startBtn.enabled = true;
        if(startBtn.TryGetComponent(out Image img))
        {
            img.color = btnImgAlphaOn;
        }

    }

    void StartBtnInActive()
    {

        //버튼활성화
        startBtn.enabled = false;
        if (startBtn.TryGetComponent(out Image img))
        {
            img.color = btnImgAlphaOff;
        }

    }


    void FadeIn()
    {
        if(fadeImg != null)
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
                    Managers.Loading.LoadScene(Define.Scene.BattleStage_Field);


                }
            }
        }
    }

    void StartInGame()
    {
        Managers.Sound.Play("Effect/UI_Click");
        GlobalData.g_LobbyToGameScene = true;           //게임을 시작하면 다음에 로비로 돌아올시 스테이지 선택창이 뜨게끔
        stageUIObj.SetActive(false);
        fadeCheck = true;
        
        //Managers.Scene.LoadScene(Define.Scene.BattleStage_Field);
    }

    float waitTime = 0.7f;
    IEnumerator StartPaperDeltaSizeDo(float endValue, bool uiObjActive)
    {
        WaitForSeconds wfs = new WaitForSeconds(waitTime);

        paperRt?.DOSizeDelta(new Vector2(endValue, paperRt.sizeDelta.y), waitTime);
        yield return wfs;

        if (uiObj?.activeSelf == !uiObjActive)
            uiObj.SetActive(uiObjActive);

        yield return null;
    }

    public void BackFadeIn(Image fadeImg, UI_Base closePopup, bool fadeCheck)
    {
        if (fadeCheck)
        {
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
                        col.a += (Time.deltaTime * 2.0f);

                    fadeImg.color = col;


                    if (fadeImg.color.a >= 0.99f)
                    {
                        Managers.UI.ClosePopUp(closePopup);
                        if (Managers.Scene.CurrentScene is LobbyScene lobby)
                        {
                            lobby.LobbyUIOnOff(true);
                            lobby.LobbyTouchUnitInit();
                        }


                    }
                }
            }
        }

    }
}
