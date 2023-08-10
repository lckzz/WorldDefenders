using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{

    //�κ� ��ü������ �����ϰ� �����ִ� ��ũ��Ʈ
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
        GlobalData.InitUnitClass();  //�̰� �κ񿡼� �����Ұ� �κ񿡼� ����Ŭ������ �ʱ�ȭ���ش�.
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
