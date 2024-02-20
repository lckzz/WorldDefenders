using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Define;

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
    [SerializeField] private GameObject tutorialDialogArrowObj;



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
    private bool backFadeCheck = false;

    private GameObject dialogCanvas;
    private DialogueCtrl dialogCtrl;


    private bool gameStart = false;         //게임시작버튼이 눌리면 true

    public bool FadeCheck { get { return fadeCheck; }}
    // Start is called before the first frame update
    public override void Start()
    {
        Managers.Game.FileSave();
        dialogCanvas = this.gameObject.transform.Find("DialogueCanvas").gameObject;
        dialogCanvas?.TryGetComponent(out dialogCtrl);
        paperBg?.TryGetComponent(out paperRt);
        tutorialDialogArrowObj = stageUIObj?.transform.Find("Arrow").gameObject;
        rtSizeDelta = paperRt.sizeDelta;
        rtSizeDelta.x = 0.0f;
        paperRt.sizeDelta = rtSizeDelta;
        gameStart = false;
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
            Debug.Log(onestageSels[ii].gameObject.name);
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


        if (gameStart == true)
            return;



        TutorialGateClick(ii);

        //모든 1챕터를 돌아서 전부 클릭된곳을 꺼준다.
        for (int i = 0; i < onestageSels.Length; i++)
        {
            onestageSels[i].TryGetComponent(out stagenode);
            if(stagenode.StState == Define.StageState.Open)
                stagenode.ClickStageDoOff();

        }

        //해당 스테이지를 누르면 지금 스테이지가 어떤 스테이지인지 확인하고 해당스테이지 몬스터의 정보를 받아온다
        onestageSels[ii].TryGetComponent(out stagenode);
        Managers.Game.CurStageType = stagenode.Stage;


        if (stagenode.StState == Define.StageState.Open)
        {
            Managers.Sound.Play("Effect/StageClick");

            Managers.Game.SetMonsterList(stagenode.StageMonsterList);  //몬스터의 정보들을 받아둔다.
            //if (ui_PlayerCtrl.IsGo == false)
            //    SelectStageTextRefresh(Define.Chapter.One, Managers.Game.CurStageType);
            ui_PlayerCtrl.SetTarget(Managers.Game.CurStageType, true);
            StartBtnActive();
            stagenode.ClickStageDoOn();
            stageInfoObj.SetActive(true);
            if (stageInfo == null)
                stageInfoObj.TryGetComponent(out stageInfo);

            stageInfo.SetStageInfoPosition(stagenode.GetNodePosition());
            stageInfo.StageInfoInit();
        }
        else
        {
            //락걸린 스테이지를 눌럿을때
            Managers.Sound.Play("Effect/Error");

            StartBtnInActive();     //버튼 비활성화 
            stageInfo?.gameObject.SetActive(false);  //열린 스테이지정보 오브젝틀르 꺼준다.

        }



    }


    void TutorialGateClick(int gateIdx)
    {
        if (Managers.Game.TutorialEnd == false && gateIdx == (int)Stage.West)       //튜토리얼상태면서 서부스테이지를 클릭했다면
        {
            Managers.Game.TutorialEnd = true;
            Managers.Game.FileSave();
            HideTutorialArrow();
            dialogCtrl?.StartDialog(DialogKey.tutorialStageInfo.ToString(), DialogType.Dialog, DialogSize.Small, DialogId.EndDialog);
        }
            

    }



    void SelectStageTextRefresh(Define.Chapter mainstage, Define.Stage subStage)
    {
        if(mainstage == Define.Chapter.One)
        {
            switch (subStage)
            {
                case Define.Stage.West:
                    curSelectText.text = "Stage 1 - 1";
                    break;
                case Define.Stage.South:
                    curSelectText.text = "Stage 1 - 2";
                    break;
                case Define.Stage.East:
                    curSelectText.text = "Stage 1 - 3";
                    break;
                case Define.Stage.Boss:
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
            lobby.LobbyUnitInit();
        }


        if(Managers.Game.TutorialEnd == false)
            Managers.UI.GetSceneUI<UI_Lobby>().DialogMaskSet((int)Define.DialogId.DialogMask, (int)Define.DialogOrder.Stage);
        else
            Managers.UI.GetSceneUI<UI_Lobby>().HideDialogMask();

        
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
        startBtn.enabled = false;
        Managers.Sound.Play("Effect/StageStart");
        StartCoroutine(StartGame());
        
        //Managers.Scene.LoadScene(Define.Scene.BattleStage_Field);
    }

    float waitTime = 0f;

    
    IEnumerator StartGame()
    {
        gameStart = true;
        waitTime = 1.0f;
        yield return new WaitForSeconds(waitTime);
        Managers.Game.LobbyToGameScene = true;           //게임을 시작하면 다음에 로비로 돌아올시 스테이지 선택창이 뜨게끔
        stageUIObj.SetActive(false);
        fadeCheck = true;
    }



    IEnumerator StartPaperDeltaSizeDo(float endValue, bool uiObjActive)
    {
        waitTime = 0.7f;
        WaitForSeconds wfs = new WaitForSeconds(waitTime);

        Managers.Sound.Play("Effect/leather_inventory");
        paperRt?.DOSizeDelta(new Vector2(endValue, paperRt.sizeDelta.y), waitTime).OnComplete(StageDialogSelect);
        yield return wfs;

        if (uiObj?.activeSelf == !uiObjActive)
            uiObj.SetActive(uiObjActive);

        yield return null;
    }

    private void StageDialogSelect()
    {
        if (Managers.Game.OneChapterAllClear)               //1챕터 모두 완료하면 켜지는변수라 true가 되면 다이얼로그가 실행되지않음
            return;

        if (Managers.Game.TutorialEnd == false)
        {
            dialogCtrl?.StartDialog(DialogKey.tutorialStage.ToString(), DialogType.Dialog, DialogSize.Small,DialogId.EndDialog);
            Managers.Dialog.dialogEnded -= ShowTutorialArrow;
            Managers.Dialog.dialogEnded += ShowTutorialArrow;

        }

        if (Managers.Game.StageAllClear())           //팝업에 들어왔을때 스테이지가 올클리어가 되면
        {
            Managers.Game.OneChapterAllClear = true;   //올클리어시 월드맵에 들어오면 해당변수가 트루로 된다.
            dialogCtrl?.StartDialog(DialogKey.stage1Ending.ToString(), DialogType.Dialog, DialogSize.Large);  //1챕터 엔딩 다이얼로그실행

        }
    }


    private void ShowTutorialArrow()
    {
        if(tutorialDialogArrowObj == null)
            tutorialDialogArrowObj = stageUIObj?.transform.Find("Arrow").gameObject;


        tutorialDialogArrowObj?.SetActive(true);
    }

    private void HideTutorialArrow()
    {
        tutorialDialogArrowObj?.SetActive(false);
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
                            lobby.LobbyUnitInit();
                        }


                    }
                }
            }
        }

    }



    private void OnDestroy()
    {
        //파괴되면 참조를 끊어준다.
        if(Managers.Instance != null)
            Managers.Dialog.dialogEnded -= ShowTutorialArrow;

    }
}
