using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageSelectPopUp : UI_Base
{

    [SerializeField] private Image[] onestageSels = null;
    [SerializeField] private TextMeshProUGUI curSelectText;
    [SerializeField] private GameObject player;

    private UI_PlayerController ui_PlayerCtrl;
    // Start is called before the first frame update
    void Start()
    {
        for(int ii = 0; ii < onestageSels.Length; ii++)
        {
            Debug.Log("테스트");
            OnClickStage(onestageSels[ii].gameObject, ii, GetStageInfo, UIEvnet.PointerDown);
        }

        player.TryGetComponent(out ui_PlayerCtrl);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnClickStage(GameObject stageObj, int idx, Action<int> action, UIEvnet type = UIEvnet.PointerDown)
    {
        UI_EventHandler evt = null;
        stageObj.TryGetComponent(out evt);

        

        if (evt != null)
        {
            switch(type)
            {
                case UIEvnet.PointerDown:
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
        //해당 스테이지를 누르면 지금 스테이지가 어떤 스테이지인지 확인하고 해당스테이지 몬스터의 정보를 받아온다
        onestageSels[ii].TryGetComponent(out StageNode stagenode);
        GlobalData.SetMonsterList(stagenode.StageMonsterList);  //정적변수에 몬스터의 정보들을 받아둔다.
        ui_PlayerCtrl.SetTarget(stagenode.Stage,true);
        SelectStageTextRefresh(Define.MainStage.One, stagenode.Stage);
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
}
