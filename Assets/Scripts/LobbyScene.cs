using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyScene : BaseScene
{

    //로비를 전체적으로 관리하고 보여주는 스크립트
    [SerializeField] private LobbyUnit[] lobbyUnits;
    [SerializeField] private GameObject fadePanel;

    [SerializeField] private GameObject goalCircle;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject ui_Lobby;



    private bool attackAnimCheck = false;
    private string unitName = "";
    LobbyUnitController lobbyUnit;
    LobbyUnitController curLobbyUnit;

    Vector3 saveStartGoalPos;
    Vector3 startPlayerPos;


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
        MouseClick();


    }


    protected override void Init()
    {
        base.Init();
        ui_Lobby?.SetActive(true);

        SceneType = Define.Scene.Lobby;

        GlobalData.InitUnitClass();  //이건 로비에서 적용할것 로비에서 유닛클래스를 초기화해준다.
        RefreshUnit();
        Managers.Sound.Play("BGM/LobbyBGM", Define.Sound.BGM);
        saveStartGoalPos = goalCircle.transform.position;
        startPlayerPos = player.transform.position;
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

    void MouseClick()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            Debug.Log(hit.collider);

            if (hit.collider != null)
            {
                if(hit.collider.CompareTag("Unit"))
                {

                    if(hit.collider.name != unitName)
                    {
                        if (curLobbyUnit != null)
                            curLobbyUnit.SelectUnitCircleOnOff(false);

                        hit.collider.TryGetComponent(out lobbyUnit);
                        curLobbyUnit = lobbyUnit;

                        unitName = hit.collider.name;
                        lobbyUnit.SelectUnitCircleOnOff(true);
                    }
                    else
                    {
                        //어택애님체크가 활성화됫을때 유닛을 클릭하면 공격애니메이션이 나감
                        lobbyUnit?.SetAttackState();
                    }    

                }
                else
                {
                    if(lobbyUnit != null)
                    {
                        attackAnimCheck = false;
                        Vector2 goalPos = hit.point;
                        Vector3 pos = hit.point;
                        pos.y -= 0.9f;
                        goalCircle.transform.position = pos;
                        lobbyUnit.SetMove(goalPos);
                    }
                }
            }
        }

        if(lobbyUnit != null)
        {
            if(lobbyUnit.LobbyState == Define.LobbyUnitState.Idle)
            {
                goalCircle.transform.position = saveStartGoalPos;
            }
        }

    }


  

    public void LobbyTouchUnitInit()
    {
        if (lobbyUnit != null)
        {
            lobbyUnit.SelectUnitCircleOnOff(false);
            lobbyUnit.SetState(Define.LobbyUnitState.Idle);
        }
        lobbyUnit = null;
        curLobbyUnit = null;
        unitName = "";
        goalCircle.transform.position = saveStartGoalPos;
        player.transform.position = startPlayerPos;
        player.transform.localScale = new Vector3(1, 1, 1);
        RefreshUnit();
    }


    public void RefreshUnit()
    {



        for (int ii = 0; ii < lobbyUnits.Length; ii++)
        {

            //GlobalData.g_SlotUnitClass
            lobbyUnits[ii].E_UniClass = GlobalData.g_SlotUnitClass[ii];
            lobbyUnits[ii].RefreshUnitSet();
            if ((int)lobbyUnits[ii].E_UniClass >= (int)UnitClass.Magician)
                lobbyUnits[ii].transform.localScale = new Vector3(-1.3f, 1.3f, 1.3f);
            else
            {
                switch(ii)
                {
                    case 0:
                        LobbyUnitScaleSet(ii, new Vector3(-2.0f, 2.0f, 2.0f));
                        break;
                    case 1:
                        LobbyUnitScaleSet(ii, new Vector3(-1.8f, 1.8f, 1.8f));
                        break;
                    case 2:
                        LobbyUnitScaleSet(ii, new Vector3(-1.7f, 1.7f, 1.7f));
                        break;
                    case 3:
                        LobbyUnitScaleSet(ii, new Vector3(-1.8f, 1.8f, 1.8f));
                        break;
                    case 4:
                        LobbyUnitScaleSet(ii, new Vector3(-2.0f, 2.0f, 2.0f));
                        break;
                }
            }

        }
    }


    void LobbyUnitScaleSet(int idx, Vector3 scaleVec)
    {
        lobbyUnits[idx].transform.localScale = scaleVec;
    }


    public void LobbyUIOnOff(bool isOn)
    {
        if (Managers.UI.GetSceneUI<UI_Lobby>() != null)
            Managers.UI.GetSceneUI<UI_Lobby>().LobbyUIOnOff(isOn);
    }

    public override void Clear()
    {
        
    }
}
