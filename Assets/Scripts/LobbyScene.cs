using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{

    //�κ� ��ü������ �����ϰ� �����ִ� ��ũ��Ʈ

    // Start is called before the first frame update
    void Start()
    {
        //Managers.UI.ShowSceneUI<UI_Lobby>();
        Managers.UI.ShowPopUp<UI_Lobby>();



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
