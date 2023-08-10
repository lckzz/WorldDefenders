using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{

    //로비를 전체적으로 관리하고 보여주는 스크립트
    [SerializeField] private LobbyUnit[] lobbyUnits;
    // Start is called before the first frame update
    void Start()
    {
        //Managers.UI.ShowSceneUI<UI_Lobby>();
        Managers.UI.ShowPopUp<UI_Lobby>();

        Init();

    }

    // Update is called once per frame
    void Update()
    {
        

    }


    void Init()
    {
        GlobalData.InitUnitClass();  //이건 로비에서 적용할것 로비에서 유닛클래스를 초기화해준다.
        RefreshUnit();

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
        }
    }
    
}
