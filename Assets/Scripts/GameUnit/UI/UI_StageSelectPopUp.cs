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


    private bool gameStart = false;         //���ӽ��۹�ư�� ������ true

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
        if (fadeCheck)   //�������� �Ѿ��
            FadeIn();

        //�ڷΰ��ų� ���ö�
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

        //��� 1é�͸� ���Ƽ� ���� Ŭ���Ȱ��� ���ش�.
        for (int i = 0; i < onestageSels.Length; i++)
        {
            onestageSels[i].TryGetComponent(out stagenode);
            if(stagenode.StState == Define.StageState.Open)
                stagenode.ClickStageDoOff();

        }

        //�ش� ���������� ������ ���� ���������� � ������������ Ȯ���ϰ� �ش罺������ ������ ������ �޾ƿ´�
        onestageSels[ii].TryGetComponent(out stagenode);
        Managers.Game.CurStageType = stagenode.Stage;


        if (stagenode.StState == Define.StageState.Open)
        {
            Managers.Sound.Play("Effect/StageClick");

            Managers.Game.SetMonsterList(stagenode.StageMonsterList);  //������ �������� �޾Ƶд�.
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
            //���ɸ� ���������� ��������
            Managers.Sound.Play("Effect/Error");

            StartBtnInActive();     //��ư ��Ȱ��ȭ 
            stageInfo?.gameObject.SetActive(false);  //���� ������������ ������Ʋ�� ���ش�.

        }



    }


    void TutorialGateClick(int gateIdx)
    {
        if (Managers.Game.TutorialEnd == false && gateIdx == (int)Stage.West)       //Ʃ�丮����¸鼭 ���ν��������� Ŭ���ߴٸ�
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

        //��ưȰ��ȭ
        startBtn.enabled = true;
        if(startBtn.TryGetComponent(out Image img))
        {
            img.color = btnImgAlphaOn;
        }

    }

    void StartBtnInActive()
    {

        //��ưȰ��ȭ
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
        Managers.Game.LobbyToGameScene = true;           //������ �����ϸ� ������ �κ�� ���ƿý� �������� ����â�� �߰Բ�
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
        if (Managers.Game.OneChapterAllClear)               //1é�� ��� �Ϸ��ϸ� �����º����� true�� �Ǹ� ���̾�αװ� �����������
            return;

        if (Managers.Game.TutorialEnd == false)
        {
            dialogCtrl?.StartDialog(DialogKey.tutorialStage.ToString(), DialogType.Dialog, DialogSize.Small,DialogId.EndDialog);
            Managers.Dialog.dialogEnded -= ShowTutorialArrow;
            Managers.Dialog.dialogEnded += ShowTutorialArrow;

        }

        if (Managers.Game.StageAllClear())           //�˾��� �������� ���������� ��Ŭ��� �Ǹ�
        {
            Managers.Game.OneChapterAllClear = true;   //��Ŭ����� ����ʿ� ������ �ش纯���� Ʈ��� �ȴ�.
            dialogCtrl?.StartDialog(DialogKey.stage1Ending.ToString(), DialogType.Dialog, DialogSize.Large);  //1é�� ���� ���̾�α׽���

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
        //�ı��Ǹ� ������ �����ش�.
        if(Managers.Instance != null)
            Managers.Dialog.dialogEnded -= ShowTutorialArrow;

    }
}
