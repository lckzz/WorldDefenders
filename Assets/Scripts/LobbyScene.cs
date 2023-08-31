using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{

    //로비를 전체적으로 관리하고 보여주는 스크립트
    [SerializeField] private LobbyUnit[] lobbyUnits;
    [SerializeField] private GameObject fadePanel;

    // Start is called before the first frame update
    void Start()
    {
        //Managers.UI.ShowSceneUI<UI_Lobby>();

        Init();

    }

    // Update is called once per frame
    void Update()
    {
        SceneMove();

    }


    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Lobby;
        Managers.UI.ShowPopUp<UI_Lobby>();
        GlobalData.InitUnitClass();  //이건 로비에서 적용할것 로비에서 유닛클래스를 초기화해준다.
        RefreshUnit();
        Managers.Sound.Play("BGM/LobbyBGM", Define.Sound.BGM);

    }


    void SceneMove()
    {
        if (Managers.UI.PeekPopupUI<UI_StageSelectPopUp>() != null)
        {
            if(Managers.UI.PeekPopupUI<UI_StageSelectPopUp>().FadeCheck)
            {
                if (fadePanel != null && fadePanel.activeSelf == false)
                {
                    fadePanel.SetActive(true);
                }
            }

        }
    }

  


    public void RefreshUnit()
    {

        Debug.Log(lobbyUnits.Length);

        for (int ii = 0; ii < lobbyUnits.Length; ii++)
        {
            Debug.Log(GlobalData.g_SlotUnitClass[ii]);

            //GlobalData.g_SlotUnitClass
            lobbyUnits[ii].E_UniClass = GlobalData.g_SlotUnitClass[ii];
            lobbyUnits[ii].RefreshUnitSet();
            if (lobbyUnits[ii].E_UniClass == UnitClass.Magician)
                lobbyUnits[ii].transform.localScale = new Vector3(-1.3f, 1.3f, 1.3f);
        }
    }

    public override void Clear()
    {
        
    }
}
