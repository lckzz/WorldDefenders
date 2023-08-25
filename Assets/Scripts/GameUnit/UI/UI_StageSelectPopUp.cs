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

    private bool fadeCheck = false;


    [SerializeField] private GameObject player;

    Color btnImgAlphaOn = new Color32(255, 255, 255, 255);
    Color btnImgAlphaOff = new Color32(255, 255, 255, 105);

    private UI_PlayerController ui_PlayerCtrl;


    public bool FadeCheck { get { return fadeCheck; }}
    // Start is called before the first frame update
    void Start()
    {

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

        GetStageInfo((int)GlobalData.curStage);

    }

    // Update is called once per frame
    void Update()
    {
        if(fadeCheck)
        {
            FadeIn();
        }
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
        //�ش� ���������� ������ ���� ���������� � ������������ Ȯ���ϰ� �ش罺������ ������ ������ �޾ƿ´�
        onestageSels[ii].TryGetComponent(out StageNode stagenode);
        GlobalData.SetMonsterList(stagenode.StageMonsterList);  //���������� ������ �������� �޾Ƶд�.
        if (ui_PlayerCtrl.IsGo == false)
            SelectStageTextRefresh(Define.MainStage.One, stagenode.Stage);
        ui_PlayerCtrl.SetTarget(stagenode.Stage,true);
        StartBtnActive();

    }




    void SelectStageTextRefresh(Define.MainStage mainstage, Define.SubStage subStage)
    {
        if(mainstage == Define.MainStage.One)
        {
            switch (subStage)
            {
                case Define.SubStage.One:
                    curSelectText.text = "Stage 1 - 1";
                    break;
                case Define.SubStage.Two:
                    curSelectText.text = "Stage 1 - 2";
                    break;
                case Define.SubStage.Three:
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
        Managers.UI.ShowPopUp<UI_Lobby>();
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
                    Managers.Scene.LoadScene(Define.Scene.BattleStage_Field);


                }
            }
        }
    }

    void StartInGame()
    {
        Managers.Sound.Play("Effect/UI_Click");

        fadeCheck = true;
        //Managers.Scene.LoadScene(Define.Scene.BattleStage_Field);
    }
}
